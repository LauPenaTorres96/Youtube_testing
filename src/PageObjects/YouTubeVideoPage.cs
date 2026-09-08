using OpenQA.Selenium;

namespace YoutubeTests.PageObjects
{
    public class YouTubeVideoPage : BasePage
    {
        // Locators for video page elements - using ytd-watch-info-text for reliability
        private By VideoTitle = By.XPath("//h1[@class='style-scope ytd-video-primary-info-renderer']//yt-formatted-string");
        private By WatchInfoContainer = By.XPath("//ytd-watch-info-text[@id='ytd-watch-info-text']");
        private By ChannelName = By.XPath("//ytd-channel-name//a[@class='yt-simple-endpoint style-scope yt-formatted-string']");
        private By VideoDuration = By.XPath("//span[@class='style-scope yt-formatted-string'][contains(text(), ':')]");

        public YouTubeVideoPage(IWebDriver driver) : base(driver)
        {
        }

        public string GetVideoTitle()
        {
            return GetText(VideoTitle);
        }

        public string GetViewCount()
        {
            // Get view count from the watch info text element
            // Format: "555,863 views"
            string watchInfo = GetText(WatchInfoContainer);
            // Extract the view count (first number pattern followed by "views")
            var viewCountMatch = System.Text.RegularExpressions.Regex.Match(watchInfo, @"([\d,]+)\s+views");
            if (viewCountMatch.Success)
            {
                return viewCountMatch.Groups[1].Value + " views";
            }
            return string.Empty;
        }

        public string GetUploadDate()
        {
            // Get upload date from the watch info text element
            // Format: "Premiered Oct 18, 2025" or "Watched Oct 18, 2025" etc.
            string watchInfo = GetText(WatchInfoContainer);
            // Extract the date part (after views, before hashtags)
            var dateMatch = System.Text.RegularExpressions.Regex.Match(watchInfo, @"(Premiered|Watched|Started streaming|Uploaded)\s+([A-Za-z]+\s+\d+,\s+\d+)");
            if (dateMatch.Success)
            {
                return dateMatch.Groups[0].Value; // Returns full match like "Premiered Oct 18, 2025"
            }
            return string.Empty;
        }

        public string GetChannelName()
        {
            return GetText(ChannelName);
        }

        public string GetVideoDuration()
        {
            return GetText(VideoDuration);
        }

        public bool IsVideoPageLoaded()
        {
            return WaitForElementPresence(VideoTitle) && WaitForElementPresence(WatchInfoContainer);
        }
    }
}
