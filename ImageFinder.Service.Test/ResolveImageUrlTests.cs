using ImageFinder.Dal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Contrib.HttpClient;

namespace ImageFinder.Service.Test
{
    [TestClass]
    public class ResolveImageUrlTests
    {
        private Mock<IImageDataAccess> _mockDataAccess;
        private ImageService _imageService;
        private Mock<HttpMessageHandler> _mockHandler;
        private HttpClient _mockHttpClient;

        [TestInitialize]
        public void SetUp()
        {
            _mockDataAccess = new Mock<IImageDataAccess>();

            _mockHandler = new Mock<HttpMessageHandler>();
            _mockHttpClient = new HttpClient(_mockHandler.Object);

            var config = new UserIdImageResolverConfig
            {
                StaticUrlDigits = new[] { '6', '7', '8', '9' },
                BaseStaticUrl = "https://my-jsonserver.typicode.com/ck-pacificdev/tech-test/images/",
                DatabaseLookupDigits = new[] { '1', '2', '3', '4', '5' },
                FetchUrlByIdFunc = _mockDataAccess.Object.GetImageUrlByIdAsync,
                VowelCharacters = new[] { 'a', 'e', 'i', 'o', 'u' },
                VowelBasedStaticUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed={0}&size=150",
                NonAlphaNumericCharacters = new[] { '@', '#', '$', '%', '&', '*', '!', '?', ';' },
                NonAlphaNumericBasedStaticUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed={0}&size=150"
            };

            _imageService = new ImageService(new IUserIdStrategyService[] {
            new UserIdImageResolverService(_mockHttpClient, config)
        });
        }

        [TestMethod]
        public async Task ResolveImageUrl_LastDigitHigh_ReturnsExternalUrl()
        {
            //Act
            var userId = "user1239";
            var expectedUrl = "https://my-jsonserver.typicode.com/ck-pacificdev/tech-test/images/9";

            _mockHandler.SetupRequest(HttpMethod.Get, expectedUrl)
                        .ReturnsResponse("{'url':'" + expectedUrl + "'}", "application/json");

            // Arrange
            var result = await _imageService.ResolveImageUrl(userId);

            // Assert
            Assert.AreEqual(expectedUrl, result.Url);
        }

        [TestMethod]
        public async Task ResolveImageUrl_LastDigitLow_ReturnsDatabaseUrl()
        {
            //Act
            var userId = "user1234";
            var expectedUrl = "http://example.com/4";

            _mockDataAccess.Setup(m => m.GetImageUrlByIdAsync("4")).ReturnsAsync(expectedUrl);

            //Arrange
            var result = await _imageService.ResolveImageUrl(userId);

            //Assert
            Assert.AreEqual(expectedUrl, result.Url);   
        }

        [TestMethod]
        public async Task ResolveImageUrl_ContainsVowel_ReturnsVowelUrl()
        {
            //Act
            var userId = "usaeior";
            var expectedUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed=a&size=150";
           
            //Arrange
            var result = await _imageService.ResolveImageUrl(userId);

            //Assert
            Assert.AreEqual(expectedUrl, result.Url);
        }

        [TestMethod]
        public async Task ResolveImageUrl_ContainsNonAlphaNumeric_ReturnsRandomUrl()
        {
            //Act
            var userId = "usr123@";

            //Arrange
            var result = await _imageService.ResolveImageUrl(userId);

            //Assert
            StringAssert.Matches(result.Url, new System.Text.RegularExpressions.Regex(@"https://api.dicebear.com/8.x/pixel-art/png\?seed=[1-5]&size=150"));

        }

        [TestMethod]
        public async Task ResolveImageUrl_NoApplicableRules_ReturnsDefaultUrl()
        {
            //Act
            var userId = "0xq";
            var expectedUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";

            //Arrange
            var result = await _imageService.ResolveImageUrl(userId);

            //Assert
            Assert.AreEqual(expectedUrl, result.Url);
        }
    }
}