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
using Insight.Core.Models;

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
		// TODO: No Longer need this.
        public static readonly DependencyProperty FileTypeProperty = DependencyProperty.Register("FileType", typeof(string), typeof(UploadItemControl), null);

        /// <summary>
		/// TODO: Need to move this to view model. No
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnFileDialog_Click(object sender, RoutedEventArgs e)
        {
			var contentsOfFiles = await FileService.GetFiles();

			Debug.WriteLine("FilesRead");

			List<IDigest> FileDigest = new List<IDigest>();

            foreach (List<string> linesOfFile in contentsOfFiles)
            {
                // Refactor this to be a static method
				Core.Models.FileType detectedFiletype = Detector.DetectFileType(linesOfFile);

				if (detectedFiletype == Core.Models.FileType.Unknown)
				{
					throw new Exception("Unsupported file type");
				}

				//null is passed for dbContextOptions so that the InsightController built down the road defaults to using the live database.
				FileDigest.Add(DigestFactory.GetDigestor(fileType: detectedFiletype, fileContents: linesOfFile, dbContextOptions:null));
			}

			FileDigest.Sort((a, b) => a.Priority.CompareTo(b.Priority));

			foreach (var digest in FileDigest)
			{
				digest.CleanInput();
				digest.DigestLines();
			}
        }


	}
}
