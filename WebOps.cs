using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using IWebElementArray = System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement>;

namespace fill_form
{
    /// <summary>
    /// Class for performing web operations using Selenium WebDriver.
    /// </summary>
    class WebOps
    {
        IWebDriver driver;
        WebDriverWait driverWait;
        Actions actions;
        readonly string ctrlKey;
        string mainTab = "";

        /// <summary>
        /// Constructor for WebOps class. Initializes WebDriver, WebDriverWait and Actions.
        /// </summary>
        public WebOps()
        {
            driver = new ChromeDriver();
            driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            actions = new Actions(driver);
            ctrlKey = Environment.OSVersion.ToString().Contains("Unix") ?
                        Keys.Command : Keys.LeftControl;
        }

        /// <summary>
        /// Waits for an element to exist and then returns it.
        /// </summary>
        /// <param name="searchBy">The criteria to search the element by.</param>
        /// <returns>The found element as an IWebElement.</returns>
        public IWebElement WaitAndGetElement(By searchBy)
        {
            IWebElement? element = default;
            while (element == null)
            {
                try
                {
                    element = driverWait.Until(ExpectedConditions.ElementExists(searchBy));
                }
                catch (Exception)
                {
                    Console.WriteLine("Element not found yet!");
                }
            }
            Console.WriteLine("Element found.");
            return element;
        }

        /// <summary>
        /// Waits for elements to exist and then returns them.
        /// </summary>
        /// <param name="searchBy">The criteria to search the elements by.</param>
        /// <returns>The found elements as a System.Collections.ObjectModel.ReadOnlyCollection of OpenQA.Selenium.IWebElement.</returns>
        public IWebElementArray WaitAndGetElements(By searchBy)
        {
            IWebElementArray? elements = null;
            while (elements == null)
            {
                try
                {
                    driverWait.Until(ExpectedConditions.ElementExists(searchBy));
                    elements = driver.FindElements(searchBy);
                }
                catch (Exception)
                {
                    Console.WriteLine("Element not found yet!");
                }
            }
            Console.WriteLine("Element found.");
            return elements;
        }

        /// <summary>
        /// Opens a link in a new tab and returns the window handle of the new tab.
        /// </summary>
        /// <param name="link">The link to open.</param>
        /// <returns>The handle of the new tab as a string.</returns>
        public string OpenLinkInNewTab(IWebElement link)
        {
            actions.KeyDown(ctrlKey)
                    .Click(link)
                    .KeyUp(ctrlKey)
                    .Build().Perform();
            driverWait.Until(d => d.WindowHandles.Count == 2);
            return driver.WindowHandles.ElementAt(1);
        }

        /// <summary>
        /// Loads Google and performs a search.
        /// </summary>
        /// <param name="searchString">The string to search for.</param>
        public void LoadGoogle(string searchString)
        {
            string webUrl = "https://www.google.com/";
            driver.Navigate().GoToUrl(webUrl);
            mainTab = driver.WindowHandles.ElementAt(0);
            IWebElement searchBox = WaitAndGetElement(By.TagName("textArea"));
            searchBox.SendKeys(searchString);
        }

        /// <summary>
        /// Opens the search results and applies for the job.
        /// </summary>
        public void OpenResultsAndApply(Dictionary<string, string> personDetails)
        {
            IWebElementArray links = WaitAndGetElements(By.TagName("h3"));

            foreach (IWebElement link in links)
            {
                string newTab = OpenLinkInNewTab(link);
                driver = driver.SwitchTo().Window(newTab);
                ApplyForJob();
                break;
            }
        }

        /// <summary>
        /// Applies for a job by filling out the application form.
        /// </summary>
        public void ApplyForJob()
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            javaScriptExecutor.ExecuteScript("window.scrollBy(0,document.body.scrollHeight)");

            IWebElement application = WaitAndGetElement(By.CssSelector("iframe[title='Greenhouse Job Board']"));
            driver.SwitchTo().Frame(application);

            IWebElementArray textBoxes = WaitAndGetElements(By.TagName("input"));

            foreach (IWebElement textBox in textBoxes)
            {
                Console.WriteLine(textBox.TagName);
                Console.WriteLine(textBox.GetAttribute("id"));
            }
        }

    }
}