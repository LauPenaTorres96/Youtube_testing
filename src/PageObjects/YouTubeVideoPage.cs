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
        private By AdProgressBar = By.ClassName("ytp-ad-persistent-progress-bar");

        public YouTubeVideoPage(IWebDriver driver) : base(driver)
        {
        }

        public bool IsAdProgressComplete()
        {
            try
            {
                var progressBars = Driver.FindElements(AdProgressBar);
                if (progressBars.Count > 0)
                {
                    string widthStyle = progressBars[0].GetAttribute("style");
                    // Check if width is 100% (ad finished)
                    if (widthStyle.Contains("width: 100%"))
                    {
                        System.Diagnostics.Debug.WriteLine("Ad progress bar is at 100%");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking ad progress: {ex.Message}");
            }
            return false;
        }

        public void WaitForAdToFinishOrSkip()
        {
            System.Diagnostics.Debug.WriteLine("Starting ad handling...");
            int attemptCount = 0;
            const int maxAttempts = 600; // Wait up to 10 minutes (600 seconds with 1 second intervals)
            bool skipButtonClicked = false;

            while (attemptCount < maxAttempts)
            {
                try
                {
                    // Check if ad progress is complete
                    if (IsAdProgressComplete())
                    {
                        System.Diagnostics.Debug.WriteLine("Ad finished! Progress bar at 100%");
                        System.Threading.Thread.Sleep(2000); // Wait for video to start
                        break;
                    }

                    // Check if skip button is present
                    var skipButtons = Driver.FindElements(SkipAdButton);
                    if (skipButtons.Count > 0 && !skipButtonClicked)
                    {
                        System.Diagnostics.Debug.WriteLine($"Skip button found! Attempt: {attemptCount}");
                        
                        try
                        {
                            // Wait a moment for the button to be fully clickable
                            System.Threading.Thread.Sleep(500);
                            
                            IWebElement skipButton = skipButtons[0];
                            
                            // Try regular click first
                            try
                            {
                                skipButton.Click();
                                System.Diagnostics.Debug.WriteLine("Skip button clicked successfully");
                                skipButtonClicked = true;
                                System.Threading.Thread.Sleep(2000); // Wait after clicking skip
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Regular click failed: {ex.Message}, trying JavaScript click");
                                
                                // Fallback to JavaScript click
                                try
                                {
                                    ((OpenQA.Selenium.IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", skipButton);
                                    System.Diagnostics.Debug.WriteLine("JavaScript click successful");
                                    skipButtonClicked = true;
                                    System.Threading.Thread.Sleep(2000); // Wait after clicking skip
                                }
                                catch (Exception jsEx)
                                {
                                    System.Diagnostics.Debug.WriteLine($"JavaScript click also failed: {jsEx.Message}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error handling skip button: {ex.Message}");
                        }
                    }

                    // Check if video content has loaded (watch info is visible)
                    var watchInfoElements = Driver.FindElements(WatchInfoContainer);
                    if (watchInfoElements.Count > 0)
                    {
                        try
                        {
                            if (watchInfoElements[0].Displayed)
                            {
                                System.Diagnostics.Debug.WriteLine("Video page loaded successfully");
                                // Video has loaded, exit the loop
                                break;
                            }
                        }
                        catch { }
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
