using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

namespace Insight.Helpers
{
	public static class FileService
	{
		/// <summary>
		/// This remembers the file that the user selects so that it can be accessed
		/// by the program. It returns a token so that it can be accessed at a later date. 
		/// </summary>
		/// <param name="files">collection of storage files</param>
		/// <param name="fileNames">names of aforementioned files</param>
		/// <returns>A list of token strings corresponding to the files that were placed there</returns>
		private static List<string> RememberFiles(IEnumerable<StorageFile> files)
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
		private static async Task<StorageFile> GetFileFromToken(string token)
		{
			return await StorageApplicationPermissions.MostRecentlyUsedList.GetFileAsync(token);
		}

		/// <summary>
		/// Forgets a file
		/// </summary>
		/// <param name="token"></param>
		private static void ForgetFile(string token)
		{
			if (StorageApplicationPermissions.FutureAccessList.ContainsItem(token))
			{
				StorageApplicationPermissions.FutureAccessList.Remove(token);
			}
		}

		/// <summary>
		/// Method to 
		/// </summary>
		/// <returns></returns>
		public static async Task<(List<List<string>> fileContents, List<string> fileNames)> GetFiles()
		{

			// This is the list of files that failed to process. If this list stays empty, that means there were no issues. 
			var failedFileNames = new List<string>();

			// Represents the collection of files, with each element being their contents as an List
			// of strings
			var fileCollection = new List<List<string>>();

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
				List<string> fileTokens = RememberFiles(files.ToArray());

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

			return (fileCollection, failedFileNames);
		}
	}
}
