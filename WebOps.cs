using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using IWebElementArray = System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement>;
using StringBuilder = System.Text.StringBuilder;

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
        string[] details;
        Dictionary<string, string> personDetails;

        /// <summary>
        /// Constructor for WebOps class. Initializes WebDriver, WebDriverWait and Actions.
        /// </summary>
        /// <param name="personDetails">The details of the person applying for the job.</param>
        public WebOps(Dictionary<string, string> personDetails)
        {
            this.personDetails = personDetails;
            details = personDetails.Keys.ToArray();
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
        IWebElement WaitAndGetElement(By searchBy)
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
        IWebElementArray WaitAndGetElements(By searchBy)
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
        string OpenLinkInNewTab(IWebElement link)
        {
            actions.KeyDown(ctrlKey)
                    .Click(link)
                    .KeyUp(ctrlKey)
                    .Build().Perform();
            driverWait.Until(d => d.WindowHandles.Count > 1);
            return driver.WindowHandles.ElementAt(1);
        }

        /// <summary>
        /// Returns a string with only the characters from a-z or A-Z.
        /// </summary>
        /// <param name="str">The string to return only characters from.</param>
        /// <returns>The string with only characters from a-z or A-Z.</returns>
        static string ReturnOnlyChars(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Loads Google and performs a search.
        /// </summary>
        /// <param name="searchString">The string to search for.</param>
        public void LoadGoogle(string[] searchStrings)
        {
            string webUrl = "https://www.google.com/";

            foreach (string searchString in searchStrings)
            {
                driver.Navigate().GoToUrl(webUrl);
                mainTab = driver.WindowHandles.ElementAt(0);
                IWebElement searchBox = WaitAndGetElement(By.TagName("textArea"));
                searchBox.SendKeys(searchString);
            }
        }


        public void SearchGoogle(List<string> searchStrings)
        {
            string webUrl = "https://www.google.com/";

            


            

            

            foreach (var search in searchStrings)
            {
                
                //mainTab = driver.WindowHandles.ElementAt(0);
                driver.SwitchTo().Window(driver.WindowHandles.First());
                driver.Navigate().GoToUrl(webUrl);
                IWebElement searchBox = WaitAndGetElement(By.TagName("textArea"));
                searchBox.SendKeys(search);
                searchBox.Submit();
                OpenResultsAndApply();
            }

            



        }



        /// <summary>
        /// !!! Warning: This method is for testing purposes only. !!!
        /// Opens a job and applies for it.
        /// </summary>
        /// <param name="webUrl">The URL of the job to open and apply for.</param>
        public void _OpenJobAndApply(string webUrl)
        {
            driver.Navigate().GoToUrl(webUrl);
            mainTab = driver.WindowHandles.ElementAt(0);
            ApplyForJob();
        }

        /// <summary>
        /// Opens the search results and applies for the job.
        /// </summary>
        public void OpenResultsAndApply()
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
        //void ApplyForJob()
        //{
        //    IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
        //    javaScriptExecutor.ExecuteScript("window.scrollBy(0,document.body.scrollHeight)");

        //    IWebElement application = WaitAndGetElement(By.CssSelector("iframe[title='Greenhouse Job Board']"));
        //    actions.MoveToElement(application).Perform();
        //    driver.SwitchTo().Frame(application);

        //    IWebElementArray textBoxes = WaitAndGetElements(By.TagName("input"));

        //    foreach (IWebElement textBox in textBoxes)
        //    {
        //        string field = ReturnOnlyChars(textBox.GetAttribute("id"));
        //        Console.WriteLine($"field: {field}");
        //        if (field.Length < 1) continue;

        //        for (int i = 0; i < details.Length; i++)
        //        {
        //            if (details[i].Contains(field))
        //            {
        //                textBox.SendKeys(personDetails[details[i]]);
        //                break;
        //            }
        //        }
        //    }
            

        //     Display a JavaScript alert
        //    javaScriptExecutor.ExecuteScript("alert('Simple MessageBox');");

        //     Close the current tab
        //    driver.Close();
        //}


        void ApplyForJob()
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            javaScriptExecutor.ExecuteScript("window.scrollBy(0,document.body.scrollHeight)");

            IWebElement application = WaitAndGetElement(By.CssSelector("iframe[title='Greenhouse Job Board']"));
            actions.MoveToElement(application).Perform();
            driver.SwitchTo().Frame(application);

            IWebElementArray textBoxes = WaitAndGetElements(By.TagName("input"));

            foreach (IWebElement textBox in textBoxes)
            {
                string field = ReturnOnlyChars(textBox.GetAttribute("id"));
                Console.WriteLine($"field: {field}");
                if (field.Length < 1) continue;

                for (int i = 0; i < details.Length; i++)
                {
                    if (details[i].Contains(field))
                    {
                        textBox.SendKeys(personDetails[details[i]]);
                        break;
                    }
                }
            }


            // Display a JavaScript alert
            javaScriptExecutor.ExecuteScript("alert('Simple MessageBox');");

            // Close the current tab
            driver.Close();
        }



    }
}