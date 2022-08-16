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
        By moreButton = By.Id("orbit-more-button");
        By tvButton = By.XPath("//*[@id='orbit-more-drawer']//li[@class='orb-nav-tv']/a");
        By listProgramm = By.XPath("//*[@id='morning']//span[@class='timezone--time']");
        By listNameProgramm = By.XPath("//*[@id='morning']//li//a//span[contains(@class, 'programme__title')]//span");
        By currentData = By.ClassName("date");

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
            driver.FindElement(accept_button).Click();
        }

        [Test]
        public void BBCParsingTVProgramTest()
        {
            driver.FindElement(moreButton).Click();
            driver.FindElement(tvButton).Click();

            var list_time = driver.FindElements(listProgramm).ToList();
            var list_program_name = driver.FindElements(listNameProgramm).ToList();
            string date = driver.FindElement(currentData).Text;

            for (int i = 0; i < list_program_name.Count; i++)
            {
                TestContext.Out.WriteLine(list_time[i].Text.Remove(5,12) + " : " + list_program_name[i].Text + " : " + date);
            }

        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }
    }
}
