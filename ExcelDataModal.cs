using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;


namespace fill_form
{
    class ExcelDataModal
    {

        string path = "";
     

        public ExcelDataModal(string path, int sheetIndex)
        {
            this.path = path;
  
        }

        public Dictionary<string, int> GetSheetNamesWithIndex()
        {
            Dictionary<string, int> sheetNamesWithIndex = new Dictionary<string, int>();


            // Set the LicenseContext property to comply with EPPlus licensing
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

           

            

            using (var package = new ExcelPackage(new FileInfo(path)))
            {

                Console.WriteLine(package.Workbook.Worksheets.Count);
                for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                {
                    // Access the worksheet by position
                    var worksheet = package.Workbook.Worksheets[i];
                    sheetNamesWithIndex.Add(worksheet.Name, i);
                }
            }

            return sheetNamesWithIndex;
        }

        public Dictionary<string, Dictionary<string, string>> GetDataFromAllSheets()
        {

            Dictionary<string, Dictionary<string, string>> sheetNamesWithDetails = new Dictionary<string, Dictionary<string, string>>();


            // Set the LicenseContext property to comply with EPPlus licensing
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;





            using (var package = new ExcelPackage(new FileInfo(path)))
            {

                Console.WriteLine(package.Workbook.Worksheets.Count);
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    // Access the worksheet data
                    Dictionary<string, string> sheetData = GetDataFromAllSheets(worksheet);
                    
                    sheetNamesWithDetails.Add(worksheet.Name, sheetData);
                }
            }

            return sheetNamesWithDetails;

        }

        private Dictionary<string, string> GetDataFromAllSheets(ExcelWorksheet worksheet)
        {
            Dictionary<string, string> sheetData = new Dictionary<string, string>();

            int rowCount = worksheet.Dimension.Rows;
            Console.WriteLine($"Row Count:{rowCount}");

            for (int row = 2; row <= rowCount; row++)
            {
                string key = worksheet.Cells[row, 1].Text;
                string value = worksheet.Cells[row, 2].Text;

                //Add the key and value to the dictionary
                sheetData.Add(key, value);

               
            }

            return sheetData;
        }

        public List<string> GenerateSearchString(Dictionary<string, string> data)
        {

            List<string> searchStrings = new List<string>();

            string exactPhrase = data.ContainsKey("exactPhrase") ? data["exactPhrase"] : "";
            string keywords = data.ContainsKey("Keywords") ? data["Keywords"] : "";
            string experience = data.ContainsKey("Experience") ? data["Experience"] : "";
            string skills = data.ContainsKey("skills") ? data["skills"] : "";
            string site = "greenhouse.io";

            // Split skills into individual skills
            string[] skillList = skills.Split(';');

            // Generate combinations
            foreach (var skill in skillList)
            {
                StringBuilder combinationBuilder = new StringBuilder();

                combinationBuilder.Append($"\"{exactPhrase}\" {keywords} site:{site} ");
                searchStrings.Add(combinationBuilder.ToString());

                combinationBuilder = new StringBuilder();
                combinationBuilder.Append($"\"{exactPhrase}\" {keywords} +\"{experience} years\" site:{site} ");
                searchStrings.Add(combinationBuilder.ToString());

                combinationBuilder = new StringBuilder();
                combinationBuilder.Append($"\"{exactPhrase}\" {keywords} +\"{experience} years\" {skill} site:{site} ");
                searchStrings.Add(combinationBuilder.ToString());
            }



            return searchStrings;
        }
    }

}
