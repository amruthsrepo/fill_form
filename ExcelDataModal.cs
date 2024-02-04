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
    }

}
