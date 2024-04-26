using ImageFinder.Model;
using Newtonsoft.Json;

namespace ImageFinder.Service
{
    public class UserIdImageResolverService : IUserIdStrategyService
    {
        private readonly HttpClient _httpClient;
        private readonly UserIdImageResolverConfig _config;

        public UserIdImageResolverService(HttpClient httpClient, UserIdImageResolverConfig config)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool Applies(string userId)
        {
            return userId.Any() &&
                        (_config.StaticUrlDigits.Contains(userId[^1]) ||
                            _config.DatabaseLookupDigits.Contains(userId[^1]) ||
                            userId.Any(ch => _config.VowelCharacters.Contains(char.ToLower(ch))) ||
                            userId.Any(ch => _config.NonAlphaNumericCharacters.Contains(char.ToLower(ch))));
        }

        public async Task<ImageModel> GetImageUrl(string userId)
        {
            char lastChar = userId[^1];

            if (IsStaticUrlDigit(lastChar))
                return await FetchStaticUrl(lastChar);

            if (IsDatabaseLookupDigit(lastChar))
                return await FetchDatabaseUrl(lastChar);

            return ProcessVowelsAndSpecialChars(userId);

            throw new InvalidOperationException("The userId does not match any applicable rules.");
        }

        private bool IsStaticUrlDigit(char ch) => _config.StaticUrlDigits.Contains(ch);
        
        private bool IsDatabaseLookupDigit(char ch) => _config.DatabaseLookupDigits.Contains(ch);
        
        private async Task<ImageModel> FetchStaticUrl(char lastChar)
        {
            var url = $"{_config.BaseStaticUrl}{lastChar}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ImageModel>(responseBody);
            }
            catch (Exception e)
            {
                //adding a log would be beneficial
                throw new Exception("Error fetching user image", e.InnerException);
            }
        }

        private async Task<ImageModel> FetchDatabaseUrl(char lastChar)
        {
            string url = await _config.FetchUrlByIdFunc(lastChar.ToString());
            return CreateImageModel(lastChar.ToString(), url);
        }
        private ImageModel CreateImageModel(string id, string url)
        {
            return new ImageModel { Id = id, Url = url };
        }

        private ImageModel ProcessVowelsAndSpecialChars(string userId)
        {
            var vowel = _config.VowelCharacters.Where(character => userId.Contains(character)).FirstOrDefault().ToString();
            var specialChar = _config.NonAlphaNumericCharacters.Where(nonNumeric => userId.Contains(nonNumeric)).FirstOrDefault();

            if (_config.VowelBasedStaticUrl.Contains(vowel.ToLower()))
                return HandleVowelCharacter(vowel, string.Format(_config.VowelBasedStaticUrl, vowel));

            if (_config.NonAlphaNumericCharacters.Contains(specialChar))
                return HandleSpecialCharacter(specialChar.ToString());

            return null;
        }

        private ImageModel HandleVowelCharacter(string userId, string url)
        {
            return CreateImageModel(userId, url);
        }

        private ImageModel HandleSpecialCharacter(string specialChar)
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 5);
            string url = string.Format(_config.NonAlphaNumericBasedStaticUrl, randomNumber);
            return CreateImageModel(specialChar, url);
        }
    }
}
