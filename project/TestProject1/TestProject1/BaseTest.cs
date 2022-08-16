using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace AssessmentQA
{
    class BaseTest 
    {
        private IWebDriver driver;
        private string path_to_file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.txt");
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
        }

        public void GetSceen()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string name = TestContext.CurrentContext.Test.MethodName + ".png";
                string screenshotFile = Path.Combine(TestContext.CurrentContext.WorkDirectory, name);
                screenshot.SaveAsFile(screenshotFile, ScreenshotImageFormat.Png);
            }
        }
    }
}
