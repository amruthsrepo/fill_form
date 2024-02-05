
namespace fill_form
{
    class SeleniumAutomation
    {
       

        public SeleniumAutomation()
        {

            Dictionary<string, string> person = new()
            {
                { "firstname", "John" },
                { "lastname", "Doe" },
                { "phone", "999-999-9999" },
                { "email", "test@gmail.com" },
                { "city", "testCity" }
            };
            string[] searchStrings = { "\"software\" developer engineer site:greenhouse.io \n" };

            WebOps web = new(person);

            /// Load google and search using the searchStrings array.
            web.LoadGoogle(searchStrings);
            /// Open the first result and apply for the job.
            web.OpenResultsAndApply();

            /// This function is for testing purposes only.
            // web._OpenJobAndApply("https://asana.com/jobs/apply/5607560?gh_jid=5607560");
        }

       

       
    }
}
