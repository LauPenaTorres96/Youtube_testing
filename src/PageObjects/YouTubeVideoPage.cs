using OpenQA.Selenium;

namespace YoutubeTests.PageObjects
{
    public class YouTubeVideoPage : BasePage
    {
        // Locators for video page elements
        private By VideoTitle = By.XPath("//h1[@class='style-scope ytd-video-primary-info-renderer']//yt-formatted-string");
        private By ViewCount = By.XPath("//ytd-video-view-count-renderer//span[@class='view-count style-scope yt-formatted-string']");
        private By UploadDate = By.XPath("//div[@class='style-scope yt-formatted-string']//span[contains(text(), 'ago') or contains(text(), 'Watched') or contains(text(), 'Started streaming')]");
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
            return GetText(ViewCount);
        }

        public string GetUploadDate()
        {
            return GetText(UploadDate);
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
            return WaitForElementPresence(VideoTitle);
        }
    }
}
