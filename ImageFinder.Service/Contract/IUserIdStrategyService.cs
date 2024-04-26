using ImageFinder.Model;

namespace ImageFinder.Service
{
    public interface IUserIdStrategyService
    {
        bool Applies(string userId);
        Task<ImageModel> GetImageUrl(string userId);
    }
}
