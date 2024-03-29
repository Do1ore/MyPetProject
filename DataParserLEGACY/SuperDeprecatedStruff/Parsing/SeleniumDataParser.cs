﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataParserLEGACY.SuperDeprecatedStruff.SupportingLogics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace DataParserLEGACY.SuperDeprecatedStruff.Parsing
{
    public class SeleniumDataParser
    {
        public static bool Continue { get; set; } = true;
        public static int PageCounter { get; set; } = 2;

        public static List<string> hrefList;

        public static async Task<List<string>> SelectProductHref(string url)
        {
            hrefList = new List<string>();

            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(url);
            int i = 1;
            int exceptionCounter = 0;
            while (i <= 7)
            {
                try
                {
                    i++;
                    driver.FindElement(By.XPath($"/html/body/div[1]/div/div/div/div/div/div[2]/div[1]/div[4]/div[3]/div[4]/div[{i}]/div/div[3]/div[2]/div[1]")).Click();
                    exceptionCounter = 0;
                }
                catch (NoSuchElementException)
                {
                    exceptionCounter++;
                    if (exceptionCounter >= 5)
                    {
                        break;
                    }
                    continue;

                }
                if (driver.Url == url) continue;

                if (ProductDbHelper.CheckForRepeat(driver.Url))
                {
                    driver.Navigate().Back();
                    continue;
                }
                hrefList.Add(driver.Url);
                driver.Navigate().Back();
                Thread.Sleep(3000);
            }
            driver.Quit();
            return hrefList;
        }

        public async static Task<string> TranslateSingleStrokeAsync(string text)
        {
            IWebDriver driver = new ChromeDriver();
            string result = "";
            await Task.Run(() =>
            {
                driver.Manage().Window.Minimize();
                driver.Navigate().GoToUrl("https://translate.yandex.com/?source_lang=ru&target_lang=en");

                Thread.Sleep(new Random().Next(1200, 1400));
                try
                {
                    IWebElement inputField = driver.FindElement(By.CssSelector("#fakeArea"));
                    inputField.SendKeys(text);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(d => d.FindElement(By.CssSelector("#translation")));
                    Thread.Sleep(5000);
                    // Parse the translated list from the output field

                    IWebElement outputField = driver.FindElement(By.CssSelector("#translation"));
                    result = outputField.GetAttribute("textContent");

                    // Output the translated list to the console

                    // Close the browser window
                }

                catch (DriverServiceNotFoundException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
                // Find the input field and enter the Russian list to be translated

                driver.Quit();
            });
            return result;
        }

        public async static Task<List<string?>> TranslateListAsync(List<string?> list)
        {
            IWebDriver driver = new ChromeDriver();
            List<string?> result = new();
            await Task.Run(() =>
            {
                driver.Manage().Window.Minimize();
                driver.Navigate().GoToUrl("https://translate.yandex.com/?source_lang=ru&target_lang=en");
                for (int i = 0; i < list.Count; i++)
                {
                    Thread.Sleep(new Random().Next(1200, 2400));
                    try
                    {
                        IWebElement inputField = driver.FindElement(By.CssSelector("#fakeArea"));
                        inputField.SendKeys(list[i]);

                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                        wait.Until(d => d.FindElement(By.CssSelector("#translation")));
                        Thread.Sleep(5000);
                        // Parse the translated list from the output field

                        IWebElement outputField = driver.FindElement(By.CssSelector("#translation"));
                        result.Add(outputField.GetAttribute("textContent"));

                        inputField.SendKeys(Keys.Alt + "d");
                     
                    }

                    catch (DriverServiceNotFoundException)
                    {
                        Thread.Sleep(5000);
                    }
                 
                    // Find the input field and enter the Russian list to be translated
                }
                driver.Quit();
            });
            return result;
        }

        public async static Task<List<List<string?>>> TranslateListOfLists(List<List<string?>> list)
        {
            var MainResult = new List<List<string?>>();
            await Task.Run(async () =>
            {
        await Task.Run(() =>
        {
            IWebDriver driver = new ChromeDriver();
            List<string?> result = new();
            driver.Manage().Window.Minimize();
            driver.Navigate().GoToUrl("https://translate.yandex.com/?source_lang=ru&target_lang=en");

            for (int i = 0; i < list[0].Count; i++)
            {
                Thread.Sleep(new Random().Next(1200, 2400));
                try
                {
                    IWebElement inputField = driver.FindElement(By.CssSelector("#fakeArea"));
                    inputField.SendKeys(list[0][i]);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(d => d.FindElement(By.CssSelector("#translation")));
                    Thread.Sleep(5000);

                    IWebElement outputField = driver.FindElement(By.CssSelector("#translation"));
                    result.Add(outputField.GetAttribute("textContent"));

                    inputField.SendKeys(Keys.Alt + "d");

                }

                catch (DriverServiceNotFoundException)
                {
                    Thread.Sleep(5000);
                }
                catch (Exception)
                {
                    throw;
                }
                // Find the input field and enter the Russian list to be translated
            }
            driver.Quit();
            MainResult.Add(result);

        });
        await Task.Run(() =>
        {
            IWebDriver driver2 = new ChromeDriver();
            List<string?> result = new();
            driver2.Manage().Window.Minimize();
            driver2.Navigate().GoToUrl("https://translate.yandex.com/?source_lang=ru&target_lang=en");

            for (int i = 0; i < list[0].Count; i++)
            {
                Thread.Sleep(new Random().Next(1200, 2400));
                try
                {
                    IWebElement inputField = driver2.FindElement(By.CssSelector("#fakeArea"));
                    inputField.SendKeys(list[1][i]);

                    WebDriverWait wait = new WebDriverWait(driver2, TimeSpan.FromSeconds(10));
                    wait.Until(d => d.FindElement(By.CssSelector("#translation")));
                    Thread.Sleep(5000);

                    IWebElement outputField = driver2.FindElement(By.CssSelector("#translation"));
                    result.Add(outputField.GetAttribute("textContent"));

                    inputField.SendKeys(Keys.Alt + "d");

                }

                catch (DriverServiceNotFoundException)
                {
                    Thread.Sleep(5000);
                }
                catch (Exception)
                {
                    throw;
                }
                // Find the input field and enter the Russian list to be translated
            }
            driver2.Quit();
            MainResult.Add(result);

        });
        await Task.Run(() =>
        {
            IWebDriver driver = new ChromeDriver();
            List<string?> result = new();
            driver.Manage().Window.Minimize();
            driver.Navigate().GoToUrl("https://translate.yandex.com/?source_lang=ru&target_lang=en");

            for (int i = 0; i < list[0].Count; i++)
            {
                Thread.Sleep(new Random().Next(1200, 2400));
                try
                {
                    IWebElement inputField = driver.FindElement(By.CssSelector("#fakeArea"));
                    inputField.SendKeys(list[2][i]);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(d => d.FindElement(By.CssSelector("#translation")));
                    Thread.Sleep(5000);

                    IWebElement outputField = driver.FindElement(By.CssSelector("#translation"));
                    result.Add(outputField.GetAttribute("textContent"));

                    inputField.SendKeys(Keys.Alt + "d");

                }

                catch (DriverServiceNotFoundException)
                {
                    Thread.Sleep(5000);
                }
                catch (Exception)
                {
                    throw;
                }
                // Find the input field and enter the Russian list to be translated
            }
            driver.Quit();
            MainResult.Add(result);

        });
        await Task.Run(() =>
        {
            IWebDriver driver = new ChromeDriver();
            List<string?> result = new();
            driver.Manage().Window.Minimize();
            driver.Navigate().GoToUrl("https://translate.yandex.com/?source_lang=ru&target_lang=en");

            for (int i = 0; i < list[0].Count; i++)
            {
                Thread.Sleep(new Random().Next(1200, 2400));
                try
                {
                    IWebElement inputField = driver.FindElement(By.CssSelector("#fakeArea"));
                    inputField.SendKeys(list[3][i]);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(d => d.FindElement(By.CssSelector("#translation")));
                    Thread.Sleep(5000);

                    IWebElement outputField = driver.FindElement(By.CssSelector("#translation"));
                    result.Add(outputField.GetAttribute("textContent"));

                    inputField.SendKeys(Keys.Alt + "d");

                }

                catch (DriverServiceNotFoundException)
                {
                    Thread.Sleep(5000);
                }
                catch (Exception)
                {
                    throw;
                }
                // Find the input field and enter the Russian list to be translated
            }
            driver.Quit();
            MainResult.Add(result);
        });
        await Task.Run(() =>
        {
            IWebDriver driver = new ChromeDriver();
            List<string?> result = new();
            driver.Manage().Window.Minimize();
            driver.Navigate().GoToUrl("https://translate.yandex.com/?source_lang=ru&target_lang=en");

            for (int i = 0; i < list[0].Count; i++)
            {
                Thread.Sleep(new Random().Next(1200, 2400));
                try
                {
                    IWebElement inputField = driver.FindElement(By.CssSelector("#fakeArea"));
                    inputField.SendKeys(list[4][i]);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(d => d.FindElement(By.CssSelector("#translation")));
                    Thread.Sleep(5000);

                    IWebElement outputField = driver.FindElement(By.CssSelector("#translation"));
                    result.Add(outputField.GetAttribute("textContent"));

                    inputField.SendKeys(Keys.Alt + "d");

                }

                catch (DriverServiceNotFoundException)
                {
                    Thread.Sleep(5000);
                }
                catch (Exception)
                {
                    throw;
                }
                // Find the input field and enter the Russian list to be translated
            }
            MainResult.Add(result);
            driver.Quit();
        });
            });
            return MainResult;
        }
    }
}