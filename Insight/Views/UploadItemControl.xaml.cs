using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Insight.Core.Services.File;
using Insight.Helpers;
using Insight.ViewModels;

namespace Insight.Views
{
	public sealed partial class UploadItemControl : UserControl
	{
		// Using a DependencyProperty as the backing store for FileType.  This enables animation, styling, binding, etc...
		// TODO: No Longer need this.
		public static readonly DependencyProperty FileTypeProperty =
			DependencyProperty.Register("FileType", typeof(string), typeof(UploadItemControl), null);

		public UploadItemControl()
		{
			InitializeComponent();
		}

		public UploadItemViewModel ViewModel { get; } = new UploadItemViewModel();

		public string FileType
		{
			get => (string) GetValue(FileTypeProperty);
			set => SetValue(FileTypeProperty, value);
		}

		/// <summary>
		///     TODO: Need to move this to view model. No
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnFileDialog_Click(object sender, RoutedEventArgs e)
		{
			// This is the file dialog returns a array of arrays of file contents
			var contentsOfFiles = await FileService.GetFiles();

			var contentsToDigest = new List<IDigest>();

			// This orders the file contents in the right 
			foreach (var linesOfFile in contentsOfFiles.fileContents)
			{
				// Refactor this to be a static method
				var detectedFiletype = Detector.DetectFileType(linesOfFile);

				//if (detectedFiletype == Core.Models.FileType.Unknown) throw new Exception("Unsupported file type");

				//null is passed for dbContextOptions so that the InsightController built down the road defaults to using the live database.
				contentsToDigest.Add(DigestFactory.GetDigestor(detectedFiletype, linesOfFile, null));
			}

			contentsToDigest.Sort((a, b) => a.Priority.CompareTo(b.Priority));

			foreach (var content in contentsToDigest)
			{
				content.CleanInput();
				content.DigestLines();
			}

			var dialog = new ContentDialog();
			dialog.Title = "Upload Status";
			dialog.CloseButtonText = "OK";

			// Make steps to concatenate all filenames into 1 string

			dialog.Content = contentsOfFiles.fileNames[0];


			dialog.DefaultButton = ContentDialogButton.Close;
			//dialog.Content = new ContentDialogContent();

			var result = await dialog.ShowAsync();

		}
	}
}
