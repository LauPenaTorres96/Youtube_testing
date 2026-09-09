using OpenQA.Selenium;
using System;
using System.Threading;

namespace YoutubeTests.PageObjects
{
    public class YouTubeVideoPage : BasePage
    {
        // Locators for video page elements
        private By VideoTitle = By.XPath("//yt-formatted-string[@class='style-scope ytd-watch-metadata']");
        private By ViewCountElement = By.CssSelector("#info-container span.style-scope.yt-formatted-string:nth-child(1)");
        private By UploadDateElement = By.CssSelector("#info-container span.style-scope.yt-formatted-string:nth-child(3)");
        private By ChannelName = By.CssSelector("#upload-info ytd-channel-name a");
        private By VideoDuration = By.CssSelector(".ytp-time-duration");
        private By ChannelLink = By.CssSelector("#upload-info ytd-channel-name a");

        public YouTubeVideoPage(IWebDriver driver) : base(driver)
        {
        }

        public string GetVideoTitle()
        {
            return GetText(VideoTitle);
        }

        public string GetViewCount()
        {
            try
            {
                // Use your framework's internal explicit wait helper to give it time to populate
                string views = GetText(ViewCountElement);

                // Safety check fallback
                if (string.IsNullOrEmpty(views))
                {
                    views = Driver.FindElement(ViewCountElement).GetAttribute("textContent");
                }

                Console.WriteLine($"[INFO] Captured View Count: '{views}'");
                return views.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Could not retrieve view count: {ex.Message}");
                return string.Empty;
            }
        }

        public string GetUploadDate()
        {
            try
            {
                // Wait for the exact upload date node to render text contents
                string uploadDate = GetText(UploadDateElement);

                if (string.IsNullOrEmpty(uploadDate))
                {
                    uploadDate = Driver.FindElement(UploadDateElement).GetAttribute("textContent");
                }

                Console.WriteLine($"[INFO] Captured Upload Date: '{uploadDate}'");
                return uploadDate.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Could not retrieve upload date: {ex.Message}");
                return string.Empty;
            }
        }


        public string GetChannelName()
        {
            try
            {
                // Try standard visible text extraction first
                string name = GetText(ChannelName);
                if (string.IsNullOrEmpty(name))
                {
                    name = Driver.FindElement(ChannelName).GetAttribute("textContent");
                }
                Console.WriteLine($"[INFO] Captured Channel Name: '{name.Trim()}'");
                return name.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Could not retrieve channel name: {ex.Message}");
                return string.Empty;
            }
        }

        public string GetVideoDuration()
        {
            try
            {
                // 1. Force a small wait to allow the player configuration parameters to settle down
                System.Threading.Thread.Sleep(2000);

                // 2. Execute a native YouTube API script block to pull total duration directly in seconds
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;

                // This targets the core player element state directly
                var rawDuration = jsExecutor.ExecuteScript(
                    "var player = document.getElementById('movie_player') || document.querySelector('.html5-video-player');" +
                    "return player ? player.getDuration() : 0;"
                );

                double durationInSeconds = Convert.ToDouble(rawDuration);
                Console.WriteLine($"[DEBUG] Native YouTube API Reported Duration (Seconds): {durationInSeconds}");

                if (durationInSeconds <= 0)
                {
                    // Fallback strategy: If API script execution fails, pull the textContent from the layout string
                    string fallbackDuration = Driver.FindElement(VideoDuration).GetAttribute("textContent");
                    Console.WriteLine($"[WARN] API failed, using fallback layout selector value: '{fallbackDuration}'");
                    return fallbackDuration.Trim();
                }

                // 3. Format the raw seconds float/int value into standard video clock notation (MM:SS or HH:MM:SS)
                TimeSpan timeSpan = TimeSpan.FromSeconds(durationInSeconds);
                string formattedDuration;

                if (timeSpan.Hours > 0)
                {
                    formattedDuration = timeSpan.ToString(@"h\:mm\:ss");
                }
                else
                {
                    // Trims leading zero out for standard shorter clips (e.g., "05:14" becomes "5:14" to match YouTube text layout style)
                    formattedDuration = timeSpan.ToString(@"m\:ss");
                }

                Console.WriteLine($"[INFO] Captured Real Formatted Video Duration: '{formattedDuration}'");
                return formattedDuration;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Could not retrieve video duration using API scripts: {ex.Message}");
                return string.Empty;
            }
        }

        public void ClickChannelLink()
        {
            Console.WriteLine("[ACTION] Clicking on the channel name link...");

            // Reuses your existing locator variable cleanly
            Driver.FindElement(ChannelLink).Click();
        }

        public bool IsVideoPageLoaded()
        {
            Console.WriteLine("[INFO] Synchronizing page and waiting for video title to become visible...");
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(Driver, TimeSpan.FromSeconds(15));

            try
            {
                // Rely cleanly on native explicit visibility waits
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(VideoTitle));
                Console.WriteLine("[SUCCESS] Video title element loaded successfully.");

                // Return verification statuses to the caller framework
                return WaitForElementPresence(VideoTitle) && WaitForElementPresence(ViewCountElement);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("[ERROR] Timed out waiting for the core video page components to appear!");
                return false;
            }
        }
    }
}
