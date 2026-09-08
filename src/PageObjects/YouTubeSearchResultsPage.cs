using OpenQA.Selenium;

namespace YoutubeTests.PageObjects
{
    public class YouTubeSearchResultsPage : BasePage
    {
        // Locators for video items in search results
        private By VideoItems = By.XPath("//div[@id='contents']//ytd-video-renderer");
        private By VideoTitle(int index) => By.XPath($"(//div[@id='contents']//ytd-video-renderer)[{index}]//a[@id='video-title']");
        private By VideoViewCount(int index) => By.XPath($"(//div[@id='contents']//ytd-video-renderer)[{index}]//span[contains(text(), 'views')]");
        private By VideoUploadDate(int index) => By.XPath($"(//div[@id='contents']//ytd-video-renderer)[{index}]//span[@class='style-scope yt-formatted-string'][contains(text(), 'ago') or contains(text(), 'Streamed')]");

        public YouTubeSearchResultsPage(IWebDriver driver) : base(driver)
        {
        }

        public int GetVideoCount()
        {
            return Driver.FindElements(VideoItems).Count;
        }

        public string GetVideoTitle(int index)
        {
            return GetText(VideoTitle(index));
        }

        public string GetVideoViewCount(int index)
        {
            return GetText(VideoViewCount(index));
        }

        public string GetVideoUploadDate(int index)
        {
            return GetText(VideoUploadDate(index));
        }

        public void ClickOnVideo(int index)
        {
            Click(VideoTitle(index));
        }
    }
}
