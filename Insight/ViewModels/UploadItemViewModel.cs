﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;
using System;
using System.Diagnostics;

using Windows.Foundation;
using Windows.Storage.Pickers;
using Windows.Storage;
using Insight.Helpers;
using Insight.Core.Services.FileProcessors;
using System.Collections.Generic;
using Windows.Storage.AccessCache;
using Windows.UI.Popups;

namespace Insight.ViewModels
{
    public class UploadItemViewModel : ObservableObject
    {
        private ICommand _openFileDialogCommand;
        public ICommand OpenFileDialogCommand => _openFileDialogCommand ?? (_openFileDialogCommand = new RelayCommand(OpenFileDialog));

        /// <summary>
        ///
        /// </summary>
        private static async void OpenFileDialog()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.Downloads
            };

            //picker.FileTypeFilter.Add(".xlsx");
            //picker.FileTypeFilter.Add(".xls");
            picker.FileTypeFilter.Add(".csv");

            var file = await picker.PickSingleFileAsync();


            if (file != null)
            {
                // Move file to Future Access List
                string fileToken = FileService.RememberFile(file);

                var fileObject = await FileService.GetFileForToken(fileToken);

                var fileLines = await FileIO.ReadLinesAsync(fileObject);

                FileService.ForgetFile(fileToken);

                var digestAlpha = new DigestAlphaRoster(fileLines);
                digestAlpha.DigestLines();

                //Debug.WriteLine("Yo Yo" + fileObject.Path);

                //if (ReadFile.ReadText(fileObject.Path, out var output))
                //{
                //    var digestAlpha = new DigestAlphaRoster(output);
                //    digestAlpha.DigestLines();
                //    Debug.WriteLine("System has digested lines");
                //}
                //else
                //{
                //    Debug.WriteLine("System has not digested lines.");
                //}
                // Push file through file streamer

                // Read

                // Application now has read/write access to the picked file
            }
            else
            {
                Debug.WriteLine("File is null.");

                //    this.textBlock.Text = "Operation cancelled.";
            }
        }
    }
}
