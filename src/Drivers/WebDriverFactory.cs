using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using YoutubeTests.Configuration;

namespace YoutubeTests.Drivers
{
    public class WebDriverFactory
    {
        public static IWebDriver CreateDriver(BrowserSettings settings)
        {
            if (settings.BrowserType.Equals("Chrome", StringComparison.OrdinalIgnoreCase))
            {
                return CreateChromeDriver(settings);
            }

            throw new NotSupportedException($"Browser type '{settings.BrowserType}' is not supported.");
        }

        private static IWebDriver CreateChromeDriver(BrowserSettings settings)
        {
            new DriverManager().SetUpDriver(new ChromeConfig());

            ChromeOptions options = new ChromeOptions();

            if (settings.Headless)
            {
                options.AddArgument("--headless");
            }

            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--start-maximized");

            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(settings.ImplicitWaitSeconds);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(settings.PageLoadTimeoutSeconds);

            return driver;
        }
    }
}
