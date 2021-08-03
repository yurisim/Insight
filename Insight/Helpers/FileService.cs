using System;
using Windows.Storage;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage.Pickers;

namespace Insight.Helpers
{
    public static class FileService
    {
        /// <summary>
        /// This remembers the file that the user selects so that it can be accessed
        /// by the program. It returns a token so that it can be accessed at a later date. 
        /// </summary>
        /// <param name="files"></param>
        /// <returns>A list of token strings corresponding to the files that were placed there</returns>
        public static List<string> RememberFiles(StorageFile[] files)
        {
			var tokenStrings = new List<string>();

			foreach (var file in files)
			{
				var token = Guid.NewGuid().ToString();
				tokenStrings.Add(token);

				StorageApplicationPermissions.MostRecentlyUsedList.AddOrReplace(token, file);
			}

            return tokenStrings;
        }

        /// <summary>
        /// Exchanges a token for a storage file
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<StorageFile> GetFileFromToken(string token)
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

		public static async Task<List<List<string>>> GetFiles()
		{
			// Represents the collection of files, with each element being their contents as an List
			// of strings
			var fileCollection = new List<List<string>>();

			//TODO feature idea - make title of file dialog show what type of file you're uploading (AEF, alpha, etc)
			var picker = new FileOpenPicker
			{
				ViewMode = PickerViewMode.Thumbnail,
				SuggestedStartLocation = PickerLocationId.Downloads
			};

			picker.FileTypeFilter.Add(".csv");

			// Allow user to pick multiple files
			var files = await picker.PickMultipleFilesAsync();

			if (files != null)
			{
				// Move file to Future Access List
				var fileTokens = RememberFiles(files.ToArray());

				// for each item in the collection of fileTokens, fetch that item and add it to the filecollection
				foreach (var fileToken in fileTokens)
				{
					// get the file object
					var fileObject = await GetFileFromToken(fileToken);

					// get the lines from the file object
					var fileLines = await FileIO.ReadLinesAsync(fileObject);

					// add to collection
					fileCollection.Add(fileLines.ToList());

					// forget the file
					ForgetFile(fileToken);
				}
			}

			return fileCollection;
		}
	}
}
