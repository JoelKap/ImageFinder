namespace ImageFinder.Dal
{
    public interface IImageDataAccess
    {
        Task<string> GetImageUrlByIdAsync(string id);
    }
}
