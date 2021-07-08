using System;
using Windows.Storage;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;

namespace Insight.Helpers
{
    public static class FileService
    {
        //public static void HandleFile(StorageFile file)
        //{
        //    string filePath = file.Path;

        //    Debug.WriteLine(filePath.Substring(filePath.LastIndexOf(".") + 1));

        //    switch (filePath.Substring(filePath.LastIndexOf(".")))
        //    {
        //        case ".xlsx":
        //            break;

        //        case ".xls":
        //            ReadXLSX(filePath);
        //            break;

        //        default:
        //            break;
        //    }
        //}

        /// <summary>
        /// This remembers the file that the user selects so that it can be accessed
        /// by the program. It returns a token so that it can be accessed at a later date. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string RememberFile(StorageFile file)
        {
            // Generates a unique token ID
            var token = Guid.NewGuid().ToString();

            // Adds to future access list
            StorageApplicationPermissions.MostRecentlyUsedList.AddOrReplace(token, file);

            return token;
        }

        /// <summary>
        /// Exchanges a token for a storage file
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<StorageFile> GetFileForToken(string token)
        {
            return await StorageApplicationPermissions.MostRecentlyUsedList.GetFileAsync(token);
        }

        /// <summary>
        /// Forgets a file
        /// </summary>
        /// <param name="token"></param>
        public static void ForgetFile(string token)
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem(token))
            {
                StorageApplicationPermissions.FutureAccessList.Remove(token);
            }
        }

        //public static void ReadXLSX(string filePath)
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
        //    {
        //        ExcelWorkbook workBook = package.Workbook;
        //        Debug.WriteLine(package.File == null);
        //        Debug.WriteLine("*" + package.File);
        //        //var sheet = workBook.Names

        //        //sheet.Cells["A2"].Value = "SIM, YURA";

        //        ExcelWorksheet tempSheet = workBook.Worksheets.Add("Cater is Cool");

        //        tempSheet.Cells["A1"].Value = "Cater is Even Cooler!";

        //        package.Save();
        //    }
        //    //}
        //}
    }
}
