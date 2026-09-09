using OpenQA.Selenium;
using System;

namespace YoutubeTests.PageObjects
{
    public class YouTubeChannelPage : BasePage
    {
        // Locators for channel page elements
        private By ChannelName = By.CssSelector("yt-dynamic-text-view-model.ytPageHeaderViewModelTitle h1");
        private By ChannelSubscriberCount = By.CssSelector("yt-page-header-view-model .page-header-metadata span:nth-of-type(1)");
        private By ChannelHeaderName = By.CssSelector("yt-dynamic-text-view-model.ytPageHeaderViewModelTitle h1");
        
        public YouTubeChannelPage(IWebDriver driver) : base(driver)
        {
        }

        public string GetChannelName()
        {
            try
            {
                // Target the h1 element we located
                IWebElement headerElement = Driver.FindElement(ChannelHeaderName);

                // 💡 CRITICAL FIX: Pull the raw inner text content directly from the DOM layout node
                string channelText = headerElement.GetAttribute("textContent");

                // YouTube sometimes appends the word ", Verified" into the label attribute or text stream
                if (channelText.Contains(","))
                {
                    channelText = channelText.Split(',')[0];
                }

                Console.WriteLine($"[INFO] Successfully extracted Channel Page Name: '{channelText.Trim()}'");
                return channelText.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Failed to read channel name element: {ex.Message}");
                return string.Empty;
            }
        }

        public string GetSubscriberCount()
        {
            return GetText(ChannelSubscriberCount);
        }


        public bool IsChannelPageLoaded()
        {
            // Make sure it references the updated variable identifier
            return WaitForElementPresence(ChannelHeaderName);
        }

        
    }
}
