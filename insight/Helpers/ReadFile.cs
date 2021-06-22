using System;
using Windows.Storage;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Diagnostics;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Insight.Helpers
{
    public static class ReadFile
    {
        public static void HandleFile(StorageFile file)
        {
            string filePath = file.Path;
            Debug.WriteLine(filePath.Substring(filePath.LastIndexOf(".") + 1));
            switch (filePath.Substring(filePath.LastIndexOf(".")))
            {
                case ".xlsx":
                    break;
                case ".xls":
                    ReadXLSX(filePath);
                    break;
            }
        }

        public static void ReadXLSX(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var workBook = package.Workbook;
                Debug.WriteLine(package.File == null);
                Debug.WriteLine("*" + package.File);
                //var sheet = workBook.Names

                //sheet.Cells["A2"].Value = "SIM, YURA";

                var tempSheet = workBook.Worksheets.Add("Cater is Cool");

                tempSheet.Cells["A1"].Value = "Cater is Even Cooler!";

                package.Save();
            }

        }
    }
}
