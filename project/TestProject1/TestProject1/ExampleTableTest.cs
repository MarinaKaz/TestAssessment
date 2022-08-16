using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace AssessmentQA
{
    [TestFixture]
    class ExampleTableTest
    {
        private IWebDriver driver;
        private string path_to_file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.txt");

        [OneTimeSetUp]
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
            driver.Navigate().GoToUrl("https://example.com/");
            driver.Manage().Window.Maximize();
            driver.FindElement(By.XPath("//a[text()='More information...']")).Click();
        }

        [TestCase("1", TestName = "Table_1RowHighlightedTest")]
        [TestCase("2", TestName = "Table_2RowHighlightedTest")]
        [TestCase("3", TestName = "Table_3RowHighlightedTest")]
        [TestCase("4", TestName = "Table_4RowHighlightedTest")]
        [TestCase("5", TestName = "Table_5RowHighlightedTest")]
        [TestCase("6", TestName = "Table_6RowHighlightedTest")]
        [TestCase("7", TestName = "Table_7RowHighlightedTest")]
        [TestCase("8", TestName = "Table_8RowHighlightedTest")]
        [TestCase("9", TestName = "Table_9RowHighlightedTest")]
        [TestCase("10", TestName = "Table_10RowHighlightedTest")]
        [TestCase("11", TestName = "Table_11RowHighlightedTest")]
        public void TableTest(int row_number)
        {
            int upRow_number = 0;
            if (row_number > 1)
            {
                upRow_number = row_number - 1;
            }
            else
            {
                upRow_number = 2;
            }

            IWebElement line = driver.FindElement(By.XPath(".//*[@id='arpa-table']/tbody//tr[" + row_number + "]//td[3]"));
            IWebElement line_upRow = driver.FindElement(By.XPath(".//*[@id='arpa-table']/tbody//tr[" + (upRow_number) + "]//td[3]"));

            Actions a = new Actions(driver);
            a.MoveToElement(line).Perform();
            string color = line.GetCssValue("background-color");
            string color_upRow = line_upRow.GetCssValue("background-color");
            string name_row = line.Text;
            TestContext.Out.WriteLine("name row: " + name_row + ", and color:  " + color + ". Color of up row is " + color_upRow);

            Assert.IsTrue(color == "rgba(240, 240, 248, 1)");
            Assert.IsFalse(color == color_upRow);
        }

        [OneTimeTearDown]
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
