using Newtonsoft.Json;

namespace YoutubeTests.Configuration
{
    public class AppSettings
    {
        [JsonProperty("browserSettings")]
        public BrowserSettings BrowserSettings { get; set; }

        [JsonProperty("youtubeSettings")]
        public YoutubeSettings YoutubeSettings { get; set; }
    }

    public class BrowserSettings
    {
        [JsonProperty("browserType")]
        public string BrowserType { get; set; }

        [JsonProperty("headless")]
        public bool Headless { get; set; }

        [JsonProperty("implicitWaitSeconds")]
        public int ImplicitWaitSeconds { get; set; }

        [JsonProperty("pageLoadTimeoutSeconds")]
        public int PageLoadTimeoutSeconds { get; set; }
    }

    public class YoutubeSettings
    {
        [JsonProperty("baseUrl")]
        public string BaseUrl { get; set; }

        [JsonProperty("searchQuery")]
        public string SearchQuery { get; set; }
    }
}
