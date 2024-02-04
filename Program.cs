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

            // Display sheet names with indices
            Console.WriteLine("Sheet Names and Indices:");
            foreach (var kvp in sheetNamesWithIndex)
            {
                Console.WriteLine($"{kvp.Key} - Index: {kvp.Value}");
            }



            //exceldatamodal exceldatamodal = new exceldatamodal(path, sheetindex);
            //dictionary<string, string> data = exceldatamodal.readdata();



        }
    }
}
