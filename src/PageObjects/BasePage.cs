using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace YoutubeTests.PageObjects
{
    public abstract class BasePage
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        protected IWebElement WaitForElement(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        protected bool WaitForElementPresence(By locator)
        {
            try
            {
                Wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        protected string GetText(By locator)
        {
            return WaitForElement(locator).Text;
        }

        protected string GetAttribute(By locator, string attributeName)
        {
            return WaitForElement(locator).GetAttribute(attributeName);
        }

        protected void Click(By locator)
        {
            WaitForElement(locator).Click();
        }

        protected void SendKeys(By locator, string keys)
        {
            WaitForElement(locator).SendKeys(keys);
        }
    }
}
