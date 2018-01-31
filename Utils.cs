using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class Utils
    {
        public static bool isSelectorExist(this ChromeDriver driver, By selector)
        {
            return driver.FindElements(selector).Count != 0;
        }
    }
}
