using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

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
            string[] buf = File.ReadAllLines("accounts.txt");
            
            var MyIni = new IniFiles("Settings.ini");
            int i = !MyIni.KeyExists("start_pos") ?
                  Int32.Parse(MyIni.Write("start_pos", "0" )) :
                  Int32.Parse(MyIni.Read("start_pos"));

            acc[] _acc = new acc[buf.Length-i];

            for (;i<buf.Length;) {
                _acc[i] = new acc(buf[i]);

                var options = new ChromeOptions();
                //options.AddArgument("no-sandbox");
                options.AddArguments("--disable-extensions");
                // options.AddArgument("no-sandbox");
                options.AddArgument("--incognito");
                // options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");  //--disable-media-session-api
                                                       //options.AddArgument("--remote-debugging-port=9222");

                ChromeDriver driver = new ChromeDriver(options);//открываем сам браузер

                driver.LocationContext.PhysicalLocation = new OpenQA.Selenium.Html5.Location(55.751244, 37.618423, 152);
                
                
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); //время ожидания компонента страницы после загрузки страницы
                driver.Manage().Cookies.DeleteAllCookies();

                driver.Navigate().GoToUrl("https://passport.yandex.ua/auth?origin=direct&retpath=https://direct.yandex.ua/");



                IWebElement element = driver.FindElement(By.CssSelector("[name='login']"));
                element.SendKeys(_acc[i].login);
                System.Threading.Thread.Sleep(2000);
                element = driver.FindElement(By.CssSelector("[name='passwd']"));
                element.SendKeys(_acc[i].pass);
                System.Threading.Thread.Sleep(2000);
                element = driver.FindElement(By.CssSelector(".passport-Button"));
                element.SendKeys(Keys.Enter);
                string text = "";
                Boolean isError = false;
                try
                {
                    isError = driver.isSelectorExist(By.CssSelector(".passport-Domik-Form-Error_active"));
                    text = driver.FindElement(By.CssSelector(".passport-Domik-Form-Error_active")).Text;                    
                }
                catch(Exception e)
                {
                    Console.WriteLine("Ошибочка:" + e.Message);
                } 
                    
                //if (text.ToLower().IndexOf("введите") != -1)
                //{

                //    Console.WriteLine("Enter captch:");
                //    var captch = Console.ReadLine();

                //    element = driver.FindElement(By.CssSelector("[name='captcha_answer']"));
                //    element.SendKeys(captch);
                //    System.Threading.Thread.Sleep(1000);
                //    element = driver.FindElement(By.CssSelector("[name='passwd']"));
                //    element.Clear();
                //    element.SendKeys(_acc[i].pass);
                //    System.Threading.Thread.Sleep(1000);
                //    element = driver.FindElement(By.CssSelector(".passport-Button"));
                //    element.SendKeys(Keys.Enter);
                //}

                if (isError)
                {
                    if (!File.Exists("bad_results_a.txt"))
                        File.Create("bad_results_a.txt");
                    try
                    {
                        StreamWriter sw = File.AppendText("bad_results_a.txt");
                        sw.WriteLine(_acc[i].login + ";" + _acc[i].pass + ";" + text);
                        sw.Close();
                    }
                    catch(Exception e) { Console.WriteLine(e.Message); }

                }

                if (!isError)
                {
                    if (!File.Exists("good_results_a.txt"))
                        File.Create("good_results_a.txt");

                    try
                    {
                        StreamWriter sw = File.AppendText("good_results_a.txt");
                        sw.WriteLine(_acc[i].login + ";" + _acc[i].pass);
                        sw.Close();
                    }catch(Exception e) { Console.WriteLine(e.Message); }


                } 
                i++;
                MyIni.Write("start_pos", "" + i);
                driver.Close();
               }

        }

      
    }
}
