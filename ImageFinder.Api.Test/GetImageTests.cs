using ImageFinder.Controllers;
using ImageFinder.Model;
using ImageFinder.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImageFinder.Api.Test
{
    [TestClass]
    public class GetImageTests
    {
        [TestClass]
        public class ImageControllerTests
        {
            private Mock<IImageService> _mockImageService;
            private ImageController _controller;

            [TestInitialize]
            public void SetUp()
            {
                _mockImageService = new Mock<IImageService>();
                _controller = new ImageController(_mockImageService.Object);
            }

            [TestMethod]
            public async Task Get_ReturnsOkObjectResult_WithImageUrl()
            {
                // Arrange
                var userId = "user123";
                var expectedUrl = "https://example.com/image.jpg";
                _mockImageService.Setup(service => service.ResolveImageUrl(It.IsAny<string>()))
                    .ReturnsAsync(new ImageModel { Url = "https://example.com/image.jpg" });

                // Act
                var result = await _controller.GetImage(userId);

                // Assert
                Assert.IsNotNull(result);

                var okResult = result as OkObjectResult;
                var imageResponse = okResult.Value as ImageModel;

                Assert.IsNotNull(okResult);
                Assert.AreEqual(expectedUrl, imageResponse.Url);
            }

            [TestMethod]
            public async Task Get_ReturnsBadRequest_WhenUserIdIsEmpty()
            {
                // Arrange
                var userId = "";

                // Act
                var result = await _controller.GetImage(userId);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            }
        }
    }
}