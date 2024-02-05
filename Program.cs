using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Reflection;


namespace fill_form
{
    class Program
    {
        static void Main(string[] args)
        {
 
            var relativePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string path = (relativePath + @"\Data\Info\MasterList.xlsx");
            Console.WriteLine(path);
            
            //Create an instance of the ExcelDataModal class
            ExcelDataModal excelDataModal = new ExcelDataModal(path, 1);

            //Get All the sheetname with indices
            Dictionary<string, int> sheetNamesWithIndex = excelDataModal.GetSheetNamesWithIndex();

            //Get data from all sheets
            Dictionary < string,  Dictionary<string, string> > dataFromAllSheets = excelDataModal.GetDataFromAllSheets();


            //Create an instance of the WebOps class for Person details
            WebOps web = new(dataFromAllSheets["Details"]);

            //Create Search string for each jobtype in each sheet tab
            //Iterate over items in dataFromAllSheets starting from the second item
            Console.WriteLine("Data from each sheet (excluding the first sheet):");
            foreach (var sheetName in dataFromAllSheets.Skip(1))
            {
                Console.WriteLine($"Sheet Name: {sheetName.Key}");

                List<string> searchStrings = excelDataModal.GenerateSearchString(dataFromAllSheets[sheetName.Key]);

                Console.WriteLine("Search  Strings:",searchStrings);

                
                // Load google and search using the searchStrings array.
                web.SearchGoogle(searchStrings);

                // Open the first result and apply for the job.
                web.OpenResultsAndApply();
                
                
               

            }



            //string[] searchStrings = { "\"software\" developer engineer site:greenhouse.io \n" };








            ///// Load google and search using the searchStrings array.
            //web.LoadGoogle(searchStrings);
            /// Open the first result and apply for the job.
            web.OpenResultsAndApply();








        }
    }
}
