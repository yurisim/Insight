using Microsoft.Toolkit.Mvvm.ComponentModel;
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

namespace Insight.ViewModels
{
    public class UploadItemViewModel : ObservableObject
    {
        private ICommand _openFileDialogCommand;
        public ICommand OpenFileDialogCommand => _openFileDialogCommand ?? (_openFileDialogCommand = new RelayCommand(OpenFileDialog));

        public UploadItemViewModel()
        {
        }

        /// <summary>
        ///
        /// </summary>
        private async void OpenFileDialog()
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.Downloads
            };

            //picker.FileTypeFilter.Add(".xlsx");
            //picker.FileTypeFilter.Add(".xls");
            picker.FileTypeFilter.Add(".csv");

            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                // Move file to Future Access List
                _ = StorageApplicationPermissions.FutureAccessList.Add(file, file.Name);

                //
                var fileName = StorageApplicationPermissions.FutureAccessList.GetFileAsync(file.Name).GetResults();

                if (ReadFile.ReadText(fileName.Path, out List<string> output))
                {
                    var ARDigest = new DigestAlphaRoster(output);
                    ARDigest.DigestLines();
                    Debug.WriteLine("System has digested lines");
                }
                else
                {
                    Debug.WriteLine("System has not digested lines.");
                }
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
