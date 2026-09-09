using OpenQA.Selenium;
using System;

namespace YoutubeTests.PageObjects
{
    public class YouTubeSearchResultsPage : BasePage
    {
        // Locators for video items in search results (excluding ads)
        private By VideoItems = By.XPath("//div[@id='contents']//ytd-video-renderer[not(ancestor::ytd-ad-slot-renderer)]");
        private By VideoTitle(int index) => By.XPath($"(//div[@id='contents']//ytd-video-renderer[not(ancestor::ytd-ad-slot-renderer)])[{index}]//a[@id='video-title']");
        private By VideoDuration(int index) => By.XPath($"(//div[@id='contents']//ytd-video-renderer[not(ancestor::ytd-ad-slot-renderer)])[{index}]//div[contains(@class, 'thumbnail-overlay-badge-shape')]//div[@class='ytBadgeShapeText']");

        // Use the metadata-line spans for view count, upload date and duration
        private By VideoViewCount(int index) => By.XPath($"(//div[@id='contents']//ytd-video-renderer[not(ancestor::ytd-ad-slot-renderer)])[{index}]//span[@class='inline-metadata-item style-scope ytd-video-meta-block'][contains(text(), 'views')]");
        private By VideoUploadDate(int index) => By.XPath($"(//div[@id='contents']//ytd-video-renderer[not(ancestor::ytd-ad-slot-renderer)])[{index}]//span[@class='inline-metadata-item style-scope ytd-video-meta-block'][contains(text(), 'ago') or contains(text(), 'Streamed')]");
        

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
        public string GetVideoDurationByIndex(int index)
        {
            try
            {
                // Reuses the updated dynamic locator variable method
                string duration = Driver.FindElement(VideoDuration(index)).GetAttribute("textContent");
                Console.WriteLine($"[INFO] Parsed Duration for search item #{index}: '{duration.Trim()}'");
                return duration.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Could not parse video duration badge item at index {index}: {ex.Message}");
                return string.Empty;
            }
        }

        public void ClickOnVideo(int index)
        {
            Click(VideoTitle(index));
        }
    }
}
