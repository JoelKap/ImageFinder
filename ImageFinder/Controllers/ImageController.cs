using ImageFinder.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        // GET api/<ImageController>/5
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetImage(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            var imageUrl = await _imageService.ResolveImageUrl(userId);
            return Ok(imageUrl);
        }
    }
}
