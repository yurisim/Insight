﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Insight.Core.Services.File;
using Insight.Helpers;
using Insight.ViewModels;
using System.Resources;

namespace Insight.Views
{
	public sealed partial class UploadItemControl
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
			var (fileContents, failedFileNames) = await FileService.GetContentsOfFiles();

			var contentsToDigest = new List<IDigest>();

			// This orders the file contents in the right 
			foreach (var linesOfFile in fileContents)
			{
				// Refactor this to be a static method
				var detectedFiletype = Detector.DetectFileType(linesOfFile);

				//null is passed for dbContextOptions so that the InsightController built down the road defaults to using the live database.
				var digestor = DigestFactory.GetDigestor(detectedFiletype, linesOfFile, null);

				// If the file is an undetectable file type, it is null
				if (digestor != null)
				{
					contentsToDigest.Add(digestor);
				}
			}

			contentsToDigest.Sort((a, b) => a.Priority.CompareTo(b.Priority));

			foreach (var content in contentsToDigest)
			{ 
				content.CleanInput();
				content.DigestLines();
			}

			// why can't I link the resource file? doesn't seem to work in Strings or in Resource.resw :(
			const string uploadItem_FilesSuccess = "Files were successfully uploaded!";
			const string uploadItem_FilesFailure = "One or more items have failed. The following files could not be digested:";

			var dialog = new ContentDialog
			{
				Title = "Upload Status",
				CloseButtonText = "OK",

				// Make steps to concatenate all filenames into 1 string
				Content = failedFileNames.Count == 0 ? uploadItem_FilesSuccess : uploadItem_FilesFailure,

				DefaultButton = ContentDialogButton.Close
			};

			var result = await dialog.ShowAsync();

		}
	}
}
