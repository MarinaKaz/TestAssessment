using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AssessmentQA
{
    public class GoogleListLinksTest
    {
        private IWebDriver driver;
        private string path_to_file = "D:\\job\\1\\IsSoft\\assessment\\project\\properties.txt";

        [SetUp]
        public void Setup()
        {
            var data = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines(path_to_file))
            data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));

            if ((data["brow"]).ToString() == "chrome")
            {
                driver = new OpenQA.Selenium.Chrome.ChromeDriver();
            }

            if ((data["brow"]).ToString() == "firefox")
            {
                driver = new OpenQA.Selenium.Firefox.FirefoxDriver();
            }
            driver.Navigate().GoToUrl("https://www.google.com/");
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test()
        {
            //Assert.Pass();
            IWebElement element = driver.FindElement(By.XPath(".//input[@title='Search']"));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }

    }
}