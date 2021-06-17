using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;
using System;
using Windows.Storage.Pickers;

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
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.Downloads;
            picker.FileTypeFilter.Add(".xlsx");
            picker.FileTypeFilter.Add(".xls");
            picker.FileTypeFilter.Add(".csv");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                //this.textBlock.Text = "Picked photo: " + file.Name;
                Console.WriteLine(file.Path);
                
            }
            else
            {
            //    this.textBlock.Text = "Operation cancelled.";
            }
        }

    }
}
