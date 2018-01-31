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
        const string GOOD_ACCOUNT_FILE_NAME = "good_results_a.txt";
        const string BAD_ACCOUNT_FILE_NAME = "bad_results_a.txt";
        class acc
        {
            public string login { get; set; }
            public string pass { get; set; }
          
            public acc(String str,char delimeter = ' ')
            {
                this.login = str.Split(delimeter)[0];
                this.pass = str.Split(delimeter)[1];
            }
        }
        static void Main(string[] args)
        {
            string[] buf = File.ReadAllLines("accounts.txt");
            
            var MyIni = new IniFiles("Settings.ini");
            int count = !MyIni.KeyExists("start_pos") ?
                  Int32.Parse(MyIni.Write("start_pos", "0" )) :
                  Int32.Parse(MyIni.Read("start_pos"));

            acc[] _acc = new acc[buf.Length-count];

            for (int i = count;i<buf.Length - count;) {
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
                    if (!File.Exists(BAD_ACCOUNT_FILE_NAME))
                        File.Create(BAD_ACCOUNT_FILE_NAME);
                    try
                    {
                        StreamWriter sw = File.AppendText(BAD_ACCOUNT_FILE_NAME);
                        sw.WriteLine(_acc[i].login + ";" + _acc[i].pass + ";" + text);
                        sw.Close();
                    }
                    catch(Exception e) { Console.WriteLine(e.Message); }

                }

                if (!isError)
                {
                    if (!File.Exists(GOOD_ACCOUNT_FILE_NAME))
                        File.Create(GOOD_ACCOUNT_FILE_NAME);

                    try
                    {
                        StreamWriter sw = File.AppendText(GOOD_ACCOUNT_FILE_NAME);
                        sw.WriteLine(_acc[i].login + ";" + _acc[i].pass);
                        sw.Close();
                    }catch(Exception e) { Console.WriteLine(e.Message); }


                } 
                i++;
                MyIni.Write("start_pos", "" + i);
                driver.Close();
               }
            
            buf = File.ReadAllLines(BAD_ACCOUNT_FILE_NAME);

            int count_bad = !MyIni.KeyExists("start_pos_bad") ?
                  Int32.Parse(MyIni.Write("start_pos_bad", "0")) :
                  Int32.Parse(MyIni.Read("start_pos_bad"));

            acc[] bad_acc = new acc[buf.Length - count_bad];

            for (int i = count_bad; i < buf.Length - count_bad;)
            {
                _acc[i] = new acc(buf[i],';');

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
                catch { }

                if (isError)
                {
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();

                    try
                    {
                        isError = driver.isSelectorExist(By.CssSelector(".passport-Domik-Form-Error_active"));
                        text = driver.FindElement(By.CssSelector(".passport-Domik-Form-Error_active")).Text;
                    }
                    catch { }
                }

                if (!isError)
                {
                    if (!File.Exists(GOOD_ACCOUNT_FILE_NAME))
                        File.Create(GOOD_ACCOUNT_FILE_NAME);

                    try
                    {
                        StreamWriter sw = File.AppendText(GOOD_ACCOUNT_FILE_NAME);
                        sw.WriteLine(_acc[i].login + ";" + _acc[i].pass);
                        sw.Close();
                    }
                    catch (Exception e) { Console.WriteLine(e.Message); }
                }              

                i++;
                MyIni.Write("start_pos_bad", "" + i);
                driver.Close();
            }

        }

      
    }
}
