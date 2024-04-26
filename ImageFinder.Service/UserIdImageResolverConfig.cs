namespace ImageFinder.Service
{
    public class UserIdImageResolverConfig
    {
        public char[] StaticUrlDigits { get; set; }
        public string BaseStaticUrl { get; set; }
        public char[] DatabaseLookupDigits { get; set; }
        public Func<string, Task<string>> FetchUrlByIdFunc { get; set; }
        public char[] VowelCharacters { get; set; }
        public string VowelBasedStaticUrl { get; set; }
        public char[] NonAlphaNumericCharacters { get; set; }
        public string NonAlphaNumericBasedStaticUrl { get; set; }
    }
}
