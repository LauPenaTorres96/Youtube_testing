using OpenQA.Selenium;
using YoutubeTests.Configuration;

namespace YoutubeTests.PageObjects
{
    public class YouTubeHomePage : BasePage
    {
        // Locators
        private By SearchBox = By.Name("search_query");

        public YouTubeHomePage(IWebDriver driver) : base(driver)
        {
        }

        public void NavigateTo(string baseUrl)
        {
            Driver.Navigate().GoToUrl(baseUrl);
            System.Threading.Thread.Sleep(2000); // Wait for YouTube to load
        }

        public void SearchForVideo(string searchQuery)
        {
            SendKeys(SearchBox, searchQuery);
            // Press Enter instead of clicking button - more reliable
            Driver.FindElement(SearchBox).SendKeys(Keys.Enter);
            System.Threading.Thread.Sleep(3000); // Wait for search results to load
        }
    }
}
