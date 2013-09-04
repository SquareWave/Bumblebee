using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Bumblebee.Extensions
{
    public static class WebElementConvenience
    {
        public static IList<IWebElement> GetElements(this ISearchContext driver, string cssSelector)
        {
            return driver.GetElements(By.CssSelector(cssSelector));
        }

        public static IList<IWebElement> GetElements(this ISearchContext driver, By by)
        {
            return driver.FindElements(by).Where(el => el.Displayed).ToList();
        }

        public static IWebElement GetElement(this ISearchContext driver, string cssSelector)
        {
            return driver.GetElement(By.CssSelector(cssSelector));
        }

        public static IWebElement GetElement(this ISearchContext driver, By by)
        {
            var element = driver.FindElement(by);
            try
            {
                return element.Displayed ? element : driver.FindElements(by).First(el => el.Displayed);
            }
            catch (InvalidOperationException)
            {
                throw new NoSuchElementException("There exists an element with the given selector, but it is not displayed.");
            }
        }

        public static IWebDriver GetDriver(this IWebElement element)
        {
            return ((IWrapsDriver)element).WrappedDriver;
        }

        public static string GetID(this IWebElement element)
        {
            return element.GetAttribute("id");
        }

        public static IEnumerable<string> GetClasses(this IWebElement element)
        {
            return element.GetAttribute("class").Split(' ');
        }

        public static bool HasClass(this IWebElement element, string className)
        {
            return element.GetClasses().Any(@class => @class.Equals(className));
        }

        public static void SetAttribute(this IWebElement element, string attribute, string value)
        {
            element.GetDriver().ExecuteScript<object>("arguments[0].setAttribute(arguments[1], arguments[2])", element, attribute, value);
        }
    }
}