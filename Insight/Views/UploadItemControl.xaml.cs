using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Insight.ViewModels;
using Windows.Storage.Pickers;
using Windows.Storage;
using Insight.Helpers;
using Insight.Core.Services.File;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Insight.Views
{
    public sealed partial class UploadItemControl : UserControl
    {
        public UploadItemViewModel ViewModel { get; } = new UploadItemViewModel();

        public UploadItemControl()
        {
            InitializeComponent();
        }

        public string FileType
        {
            get { return (string)GetValue(FileTypeProperty); }
            set { SetValue(FileTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileTypeProperty = DependencyProperty.Register("FileType", typeof(string), typeof(UploadItemControl), null);

        private async void btnFileDialog_Click(object sender, RoutedEventArgs e)
        {
			List<List<string>> contentsOfFiles = await GetFiles();

			Debug.WriteLine("FilesRead");

			List<IDigest> FileDigest = new List<IDigest>();

            foreach (var linesOfFile in contentsOfFiles)
            {
                // Refactor this to be a static method
                var detectMe = new Detector(linesOfFile);  // detect file type
				Core.Models.FileType detectedFiletype = detectMe.DetectFileType();

				FileDigest.Add(DigestFactory.GetDigestor(fileType: detectedFiletype, fileContents: linesOfFile));
			}

			FileDigest.Sort((a, b) => a.Priority.CompareTo(b.Priority));

			foreach (var digest in FileDigest)
			{
				digest.DigestLines();
			}

            // if (filesLines != null)
            // {
            //     switch (FileType)
            //     {
            //         case "AEF":
            //             Debug.WriteLine(FileType);
            //             var digestAEF = new DigestAEF(filesLines);
            //             digestAEF.DigestLines();
            //             break;
            //         case "Alpha Roster":
            //             Debug.WriteLine(FileType);
            //             var digestAlpha = new DigestAlphaRoster(filesLines);
            //             digestAlpha.DigestLines();
            //             break;
            //         case "PEX":
            //             Debug.WriteLine(FileType);
            //             var digestPEX = new DigestPEX(filesLines);
            //             digestPEX.DigestLines();
            //             break;
			// 		case "ETMS":
			// 			Debug.WriteLine(FileType);
			// 			var digestETMS = new DigestPEX(filesLines);
			// 			digestETMS.DigestLines();
			// 			break;
			// 		case "LoX":
			// 			Debug.WriteLine(FileType);
			// 			var digestLOX = new DigestLOX(filesLines);
			// 			digestLOX.DigestLines();
			// 			break;
			// 		default:
            //             Debug.WriteLine("OOPS");
            //             break;
            //     }
            // }
        }

        private static async Task<List<List<string>>> GetFiles()
        {
			// Represents the collection of files, with each element being their contents as an IList
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
                var fileTokens = FileService.RememberFiles(files.ToArray());

				// for each item in the collection of fileTokens, fetch that item and add it to the filecollection
				foreach (var fileToken in fileTokens)
				{
					// get the file object
					var fileObject = await FileService.GetFileFromToken(fileToken);

					// get the lines from the file object
					var fileLines = await FileIO.ReadLinesAsync(fileObject);

					// add to collection
					fileCollection.Add(fileLines.ToList());

					// forget the file
					FileService.ForgetFile(fileToken);
				}
            }

			return fileCollection;
		}
	}
}
