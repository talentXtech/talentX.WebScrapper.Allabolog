using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace talentX.WebScrapper.Allabolog.Utils
{
    public class ChromeDriverUtils
    {
        public static ChromeDriver CreateChromeDriver(string url)
        {
            var options = new ChromeOptions();
            options.AddArguments("--ignore-ssl-errors", "--verbose", "--disable-dev-shm-usage");
            //        "--headless",
            //        "--verbose",
            //        "--disable-dev-shm-usage"
            var driver = new ChromeDriver(options);


            driver.Navigate().GoToUrl(url);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            driver.Manage().Window.Maximize();

            return driver;
        }

        public static ChromeDriver CreateChromeDriverHeadless(string url)
        {
            var options = new ChromeOptions();
            options.AddArguments("--ignore-ssl-errors", "--headless", "--verbose", "--disable-dev-shm-usage");
            //        "--headless",
            //        "--verbose",
            //        "--disable-dev-shm-usage"
            var driver = new ChromeDriver(options);


            driver.Navigate().GoToUrl(url);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            driver.Manage().Window.Maximize();


            return driver;
        }

        public static void ScrollToBottmOfPage(ChromeDriver driver, int sleepMs = 2000)
        {
            // Keep track of the last height
            var lastHeight = (long)driver.ExecuteScript("return document.body.scrollHeight");

            while (true)
            {
                // Scroll down to the bottom of the page
                driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                // Wait for the new content to load
                Thread.Sleep(sleepMs);

                // Check the new scroll height and compare it with the last scroll height
                var newHeight = (long)driver.ExecuteScript("return document.body.scrollHeight");

                if (newHeight == lastHeight)
                {
                    // End of page, break the loop
                    break;
                }
                lastHeight = newHeight;
            }
        }

        public static void CloseComplainaceOverlay(ChromeDriver driver)
        {
            var complianceOverlayElement = driver.FindElements(By.Id("qc-cmp2-ui"));

            if (complianceOverlayElement.Count() > 0) { complianceOverlayElement[0].FindElement(By.ClassName("css-iq8lad")).Click(); }
        }
    }
    }