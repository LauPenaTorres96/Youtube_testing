using OpenQA.Selenium;

namespace YoutubeTests.PageObjects
{
    public class YouTubeChannelPage : BasePage
    {
        // Locators for channel page elements
        private By ChannelName = By.XPath("//div[@id='channel-header']//yt-formatted-string[@class='style-scope ytd-channel-name']");
        private By ChannelSubscriberCount = By.XPath("//yt-formatted-string[@class='style-scope yt-formatted-string'][contains(text(), 'subscribers')]");

        public YouTubeChannelPage(IWebDriver driver) : base(driver)
        {
        }

        public string GetChannelName()
        {
            return GetText(ChannelName);
        }

        public string GetSubscriberCount()
        {
            return GetText(ChannelSubscriberCount);
        }

        public bool IsChannelPageLoaded()
        {
            return WaitForElementPresence(ChannelName);
        }
    }
}
