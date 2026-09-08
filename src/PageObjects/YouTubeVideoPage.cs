using OpenQA.Selenium;
using System;
using System.Threading;

namespace YoutubeTests.PageObjects
{
    public class YouTubeVideoPage : BasePage
    {
        // Locators for video page elements
        private By VideoTitle = By.XPath("//h1[@class='style-scope ytd-video-primary-info-renderer']//yt-formatted-string");
        private By WatchInfoContainer = By.XPath("//ytd-watch-info-text[@id='ytd-watch-info-text']");
        private By ChannelName = By.XPath("//ytd-channel-name//a[@class='yt-simple-endpoint style-scope yt-formatted-string']");
        private By VideoDuration = By.XPath("//span[@class='style-scope yt-formatted-string'][contains(text(), ':')]");
        
        // Ad-related locators
        private By SkipAdButton = By.ClassName("ytp-skip-ad-button");
        private By AdContainer = By.ClassName("video-ads");

        public YouTubeVideoPage(IWebDriver driver) : base(driver)
        {
        }

        public void WaitForAdToFinishOrSkip()
        {
            // Wait up to 20 seconds for either the ad to finish or skip button to appear
            int maxAttempts = 20;
            int attemptCount = 0;

            while (attemptCount < maxAttempts)
            {
                try
                {
                    // Check if skip button is present and visible
                    var skipButtons = Driver.FindElements(SkipAdButton);
                    if (skipButtons.Count > 0 && skipButtons[0].Displayed)
                    {
                        try
                        {
                            skipButtons[0].Click();
                            System.Threading.Thread.Sleep(1000); // Wait after clicking skip
                            break; // Exit loop after skipping
                        }
                        catch (Exception ex)
                        {
                            // Skip button might not be clickable yet, continue waiting
                            System.Diagnostics.Debug.WriteLine($"Could not click skip button: {ex.Message}");
                        }
                    }

                    // Check if video content has loaded (watch info is visible)
                    var watchInfoElements = Driver.FindElements(WatchInfoContainer);
                    if (watchInfoElements.Count > 0 && watchInfoElements[0].Displayed)
                    {
                        // Video has loaded, exit the loop
                        break;
                    }

                    attemptCount++;
                    System.Threading.Thread.Sleep(1000); // Wait 1 second before next check
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error waiting for ad: {ex.Message}");
                    attemptCount++;
                    System.Threading.Thread.Sleep(1000);
                }
            }

            // Final wait to ensure video is fully loaded
            System.Threading.Thread.Sleep(2000);
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
            // Call WaitForAdToFinishOrSkip to handle ads first
            WaitForAdToFinishOrSkip();
            
            // Then check if video page elements are present
            return WaitForElementPresence(VideoTitle) && WaitForElementPresence(WatchInfoContainer);
        }
    }
}
