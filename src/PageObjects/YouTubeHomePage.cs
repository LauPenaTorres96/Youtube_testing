using OpenQA.Selenium;
using YoutubeTests.Configuration;

namespace YoutubeTests.PageObjects
{
    public class YouTubeHomePage : BasePage
    {
        // Locators
        private By SearchBox = By.Name("search_query");
        private By SearchButton = By.Id("search-icon-legacy");

        public YouTubeHomePage(IWebDriver driver) : base(driver)
        {
        }

        public void NavigateTo(string baseUrl)
        {
            Driver.Navigate().GoToUrl(baseUrl);
        }

        public void SearchForVideo(string searchQuery)
        {
            SendKeys(SearchBox, searchQuery);
            Click(SearchButton);
        }
    }
}
