using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        class acc
        {
            public string login { get; set; }
            public string pass { get; set; }
            public acc(String str)
            {
                this.login = str.Split(' ')[0];
                this.pass = str.Split(' ')[1];
            }
        }
        static void Main(string[] args)
        {
            List<string> goodResults = new List<string>();
            string[] buf = File.ReadAllLines("accounts.txt");
            acc[] _acc = new acc[buf.Length];
            int i = 0;
            foreach (string s in buf) {
                _acc[i] = new acc(s);

                var options = new ChromeOptions();
                //options.AddArgument("no-sandbox");
                options.AddArguments("--disable-extensions");
                // options.AddArgument("no-sandbox");
                //options.AddArgument("--incognito");
                // options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");  //--disable-media-session-api
                                                       //options.AddArgument("--remote-debugging-port=9222");

                ChromeDriver driver = new ChromeDriver(options);//открываем сам браузер

                driver.LocationContext.PhysicalLocation = new OpenQA.Selenium.Html5.Location(55.751244, 37.618423, 152);

                driver.Manage().Window.Maximize();//открываем браузер на полный экран
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); //время ожидания компонента страницы после загрузки страницы

                driver.Navigate().GoToUrl("https://passport.yandex.ua/auth?origin=direct&retpath=https://direct.yandex.ua/");



                IWebElement element = driver.FindElement(By.CssSelector("[name='login']"));
                element.SendKeys(_acc[i].login);
                System.Threading.Thread.Sleep(2000);
                element = driver.FindElement(By.CssSelector("[name='passwd']"));
                element.SendKeys(_acc[i].pass);
                System.Threading.Thread.Sleep(2000);
                element = driver.FindElement(By.CssSelector(".passport-Button"));
                element.SendKeys(Keys.Enter);

                if (driver.Url.IndexOf("https://passport.yandex.ru/") == -1)
                    goodResults.Add(_acc[i].login + ";" + _acc[i].pass);

                i++;
                driver.Close();
               }

            File.WriteAllLines("good_results.txt", goodResults.ToArray());

        }
    }
}
