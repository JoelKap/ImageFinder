using ImageFinder.Model;

namespace ImageFinder.Service
{
    public interface IImageService
    {
        Task<ImageModel> ResolveImageUrl (string userId);
    }
}
 