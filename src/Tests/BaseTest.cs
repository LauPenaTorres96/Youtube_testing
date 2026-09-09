using NUnit.Framework;
using OpenQA.Selenium;
using YoutubeTests.Configuration;
using YoutubeTests.Drivers;

namespace YoutubeTests.Tests
{
    [TestFixture]
    public abstract class BaseTest
    {
        protected IWebDriver Driver;
        protected AppSettings AppSettings;

        [SetUp]
        public void SetUp()
        {
            AppSettings = ConfigurationLoader.LoadSettings();
            Driver = WebDriverFactory.CreateDriver(AppSettings.BrowserSettings);
        }

        [TearDown]
        public void TearDown()
        {
            Driver?.Quit();
        }
    }
}
