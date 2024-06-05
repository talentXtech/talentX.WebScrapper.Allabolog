using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace talentX.WebScrapper.Allabolog.Extensions
{
    public static class WebScrapExtensions
    {
        public static void ClickButtonById(this IWebElement parentElement, string str)
        {
            parentElement?.FindElement(By.Id(str)).Click();

        }
        public static void ClickButtonByClass(this ChromeDriver driver, string str)
        {
            driver.FindElement(By.ClassName(str)).Click();
        }

        public static void ClickButtonByClass(this IWebElement parentElement, string str)
        {
            parentElement?.FindElement(By.ClassName(str)).Click();
        }

        public static void ClickButtonByTag(this IWebElement parentElement, string str)
        {
            parentElement?.FindElement(By.TagName(str)).Click();
        }


        //extracting text from driver
        public static string FindElementTextById(this ChromeDriver driver, string id)
        {
            var element = driver.FindElements(By.Id(id));
            if (element.Count > 0)
            {
                var text = element[0].Text;
                return text;
            }
            return "";
        }

        public static string FindElementTextByClass(this ChromeDriver driver, string className)
        {
            var element = driver.FindElements(By.ClassName(className));
            if (element.Count > 0)
            {
                var text = element[0].Text;
                return text;
            }
            return "";
        }

        public static string FindElementTextByXPath(this ChromeDriver driver, string xpath)
        {
            var element = driver.FindElements(By.XPath(xpath));
            if (element.Count > 0)
            {
                var text = element[0].Text;
                return text;
            }
            return "";

        }

        public static string FindElementTextByTag(this ChromeDriver driver, string tag)
        {
            var element = driver.FindElements(By.TagName(tag));
            if (element.Count > 0)
            {
                var text = element[0].Text;
                return text;
            }
            return "";
        }

        //extracting text from parent element
        public static string FindElementTextFromParentByTag(this IWebElement parentElement, string tag)
        {
            var element = parentElement.FindElements(By.TagName(tag));
            if (element.Count > 0)
            {
                var text = element[0].Text;
                return text;
            }
            return "";
        }
        public static string FindElementTextFromParentBySelector(this IWebElement parentElement, string selector)
        {
            var element = parentElement.FindElement(By.CssSelector(selector));
            var text = element.Text;
            return text;
        }

        public static string FindElementTextFromParentByClass(this IWebElement parentElement, string className)
        {
            var element = parentElement.FindElement(By.ClassName(className));
            var text = element.Text;
            return text;
        }

        // element with child elements
        public static string FindElementTextBySelectorWithChildDivElement(this IWebElement parentElement, string selector)
        {
            var childElement = parentElement.FindElement(By.CssSelector(selector));
            var element = childElement.FindElements(By.TagName("div"));
            if (element.Count > 0)
            {
                var text = element[0].Text;
                return text;
            }
            return "";
        }
        
        // finding element
        public static IWebElement FindElementByClass(this ChromeDriver driver, string className)
        {
            var element = driver.FindElement(By.ClassName(className));
            return element;
        }

        public static IWebElement FindElementById(this ChromeDriver driver, string id)
        {
            var element = driver.FindElement(By.Id(id));
            return element;
        }


        // finding all elements

        public static ReadOnlyCollection<IWebElement> FindAllByClass(this ChromeDriver driver, string className)
        {
            var elements = driver.FindElements(By.ClassName(className));
            return elements;
        }



        public static ReadOnlyCollection<IWebElement> FindAllByTag(this IWebElement parentElement, string tag)
        {
            var elements = parentElement.FindElements(By.TagName(tag));
            return elements;
        }

        public static ReadOnlyCollection<IWebElement> FindAllByClass(this IWebElement parentElement, string className)
        {
            var elements = parentElement.FindElements(By.ClassName(className));
            return elements;
        }


    }
}