using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace AssessmentQA
{
    public class GoogleTests
    {
        private IWebDriver driver;
        private string path_to_file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.txt");
        By nlButton = By.XPath(".//div[text()='nl']");
        By enButton = By.XPath(".//li[contains(text(),'English')]");
        By acceptButton = By.XPath(".//div[text()='Accept all']");
        By search = By.XPath("//input[@title='Search']");
        By listLink = By.XPath("//a/h3/..");

        [SetUp]
        public void Setup()
        {
            var data = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines(path_to_file))
                data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
            TestContext.Out.WriteLine("Start using " + data["brow"]);

            if ((data["brow"]).ToString() == "chrome")
            {
                driver = new OpenQA.Selenium.Chrome.ChromeDriver();
            }

            if ((data["brow"]).ToString() == "firefox")
            {
                driver = new OpenQA.Selenium.Firefox.FirefoxDriver();
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            driver.Navigate().GoToUrl("https://www.google.com/");
            driver.Manage().Window.Maximize();
            driver.FindElement(nlButton).Click();
            driver.FindElement(enButton).Click();
            driver.FindElement(acceptButton).Click();
        }

        [Test]
        public void CollectValidSearchLinksTest()
        {
            IWebElement search_field = driver.FindElement(search);
            search_field.SendKeys("top it companies in netherlands");
            search_field.SendKeys(Keys.Return);

            var list_links = driver.FindElements(listLink).Where(x => !string.IsNullOrEmpty(x.GetAttribute("href")))
            .Select(x => x.GetAttribute("href"))
            .ToList();

            for (int i = 0; i < list_links.Count; i++)
            {
                TestContext.Out.WriteLine(list_links[i]);
            }
        }

        [Test]
        public void CheckInvalidSearchDataTest()
        {
            IWebElement search_field = driver.FindElement(search);
            search_field.SendKeys("qwertyuiopasdffghjklweterrdfgdfdgfwfefwefwfsdvsv1234t345e");
            search_field.SendKeys(Keys.Return);
            var list_links = driver.FindElements(listLink).Where(x => !string.IsNullOrEmpty(x.GetAttribute("href")))
            .Select(x => x.GetAttribute("href"))
            .ToList();
            string result = driver.FindElement(By.Id("res")).Text;
            TestContext.Out.WriteLine(result);

            Assert.IsEmpty(list_links);
            Assert.IsTrue(result.Contains("did not match any documents"), result + "doesn't contains did not match any documents ");
        }

        [Test]
        public void FailTestForVerifySaveScreenTest()
        {
            IWebElement search_field = driver.FindElement(search);
            search_field.SendKeys("qwertyuiopasdffghjklweterrdfgdfdgfwfefwefwfsdvsv1234t345e");
            search_field.SendKeys(Keys.Return);
            var list_links = driver.FindElements(listLink).Where(x => !string.IsNullOrEmpty(x.GetAttribute("href")))
            .Select(x => x.GetAttribute("href"))
            .ToList();
            string result = driver.FindElement(By.Id("res")).Text;

            Assert.IsTrue(result.Contains("haha"), result + "doesn't contains did not match any documents ");
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string name = TestContext.CurrentContext.Test.MethodName + ".png";
                string screenshotFile = Path.Combine(TestContext.CurrentContext.WorkDirectory, name);
                screenshot.SaveAsFile(screenshotFile, ScreenshotImageFormat.Png);
            }

            driver.Close();
        }

    }
}