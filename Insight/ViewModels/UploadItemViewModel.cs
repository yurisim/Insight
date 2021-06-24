using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;
using System;
using System.Diagnostics;

using Windows.Storage.Pickers;
using Windows.Storage;
using Insight.Helpers;

namespace Insight.ViewModels
{
    public class UploadItemViewModel : ObservableObject
    {
        private ICommand _openFileDialogCommand;
        public ICommand OpenFileDialogCommand => _openFileDialogCommand ?? (_openFileDialogCommand = new RelayCommand(OpenFileDialog));




        public UploadItemViewModel()
        {      

        }

        private async void OpenFileDialog()
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.Downloads
            };

            picker.FileTypeFilter.Add(".xlsx");
            picker.FileTypeFilter.Add(".xls");
            picker.FileTypeFilter.Add(".csv");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                //this.textBlock.Text = "Picked photo: " + file.Name;
                ReadFile.HandleFile(file);
            }
            else
            {
                //    this.textBlock.Text = "Operation cancelled.";
            }
        }
    }
}
