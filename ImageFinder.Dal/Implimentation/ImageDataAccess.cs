using Microsoft.EntityFrameworkCore;

namespace ImageFinder.Dal
{
    public class ImageDataAccess : IImageDataAccess
    {
        private readonly ImageDbContext _context;

        public ImageDataAccess(ImageDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetImageUrlByIdAsync(string id)
        {
            if (!int.TryParse(id, out var imageId))
            {
                //adding a log would be beneficial
                return "https://i.postimg.cc/cCm8ChJM/Image-not-available.png";
            }

            try
            {
                var image = await _context.Images.SingleOrDefaultAsync(img => img.Id == imageId);
                if(image is null)
                    return "https://i.postimg.cc/cCm8ChJM/Image-not-available.png";

                return $"{image.Url}{id}";
            }
            catch (Exception e)
            {
                // adding a log would be beneficial
                throw new InvalidOperationException("Error retrieving an image", e.InnerException);
            }
        }
    }
}