    using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataParser
{
    public class SelenuimDataParser
    {

        public static List<string> hrefList;

        public static List<string> SelectProductHref(string url)
        {
            hrefList = new List<string>();

            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            for (int i = 2; i < 3; i++)
            {
                try
                {
                    driver.FindElement(By.XPath($"/html/body/div[1]/div/div/div/div/div/div[2]/div[1]/div[4]/div[3]/div[4]/div[{i}]/div/div[3]/div[2]/div[1]")).Click();

                }
                catch (Exception)
                {
                    continue;
                }
                hrefList.Add(driver.Url);
                driver.Navigate().Back();
                Thread.Sleep(3000);
            }
            driver.Quit();
            return hrefList;
        }

        public async Task<string> TranslateData(string text)
        {
            IWebDriver driver = new ChromeDriver();
            string result = "";
            await Task.Run(() =>
            {
                driver.Manage().Window.Minimize();
                driver.Navigate().GoToUrl("https://translate.yandex.com/?source_lang=ru&target_lang=en");
                
                Thread.Sleep(1000);
  
                // Find the input field and enter the Russian text to be translated
                IWebElement inputField = driver.FindElement(By.CssSelector("#fakeArea"));
                inputField.SendKeys(text);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(By.CssSelector("#translation")));
                Thread.Sleep(5000);
                // Parse the translated text from the output field

                IWebElement outputField = driver.FindElement(By.CssSelector("#translation"));
                result = outputField.GetAttribute("textContent");

                // Output the translated text to the console

                // Close the browser window
                driver.Quit();
            });
            return result;
        }
    }
}
