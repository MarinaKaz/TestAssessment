using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace AssessmentQA
{
    class BBCTest
    {
        private IWebDriver driver;
        private string path_to_file =  Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.txt");
        By accept_button = By.XPath("//p[@class ='fc-button-label'][text()='Consent']");

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
            driver.Navigate().GoToUrl("https://www.bbc.com/");
            driver.Manage().Window.Maximize();

            //           driver.FindElement(By.XPath("//p[@class ='fc-button-label'][text()='Consent']")).Click();

            driver.FindElement(accept_button).Click();
        }

        [Test]
        public void BBCParsingTVProgramTest()
        {
            driver.FindElement(By.Id("orbit-more-button")).Click();
            driver.FindElement(By.XPath("//*[@id='orbit-more-drawer']//li[@class='orb-nav-tv']/a")).Click();

            var list_time = driver.FindElements(By.XPath("//*[@id='morning']//span[@class='timezone--time']")).ToList();
            var list_program_name = driver.FindElements(By.XPath("//*[@id='morning']//li//a//span[@class='programme__title delta']//span")).ToList();

            //*[@id='morning']//li//a//span[contains(@class, 'programme__title')]//span
            
            string date = driver.FindElement(By.ClassName("date")).Text;

            for (int i = 0; i < list_program_name.Count; i++)
            {
                TestContext.Out.WriteLine(list_time[i].Text.Remove(5,12) + " : " + list_program_name[i].Text + " : " + date);
            }

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
