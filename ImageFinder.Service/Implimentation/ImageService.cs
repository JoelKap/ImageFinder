using ImageFinder.Model;

namespace ImageFinder.Service
{
    public class ImageService : IImageService
    {
        private readonly List<IUserIdStrategyService> _strategies;
        private const string defaultUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";

        public ImageService(IEnumerable<IUserIdStrategyService> strategies)
        {
            _strategies = strategies.ToList() ?? throw new ArgumentNullException(nameof(strategies));
        }

        public async Task<ImageModel> ResolveImageUrl(string userId)
        {
            foreach (var strategy in _strategies)
            {
                if (strategy.Applies(userId))
                {
                    return await strategy.GetImageUrl(userId);
                }
            }

            return new ImageModel() { Id = "", Url = defaultUrl };
        }
    }
}
