using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace fill_form
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.google.com/");
        }
    }
}
