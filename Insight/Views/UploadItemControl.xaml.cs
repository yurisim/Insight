using Insight.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Insight.ViewModels;
using Windows.Storage.Pickers;
using Windows.Storage;
using Insight.Helpers;
using Insight.Core.Services.FileProcessors;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var filesLines = await GetFile();
            if (filesLines != null)
            {
                switch (FileType)
                {
                    case "AEF":
                        Debug.WriteLine(FileType);
                        var digestAEF = new DigestAEF(filesLines);
                        digestAEF.DigestLines();
                        break;
                    case "Alpha Roster":
                        Debug.WriteLine(FileType);
                        var digestAlpha = new DigestAlphaRoster(filesLines);
                        digestAlpha.DigestLines();
                        break;
                    case "PEX":
                        Debug.WriteLine(FileType);
                        break;
                    default:
                        Debug.WriteLine("OOPS");
                        break;
                }
            }
        }

        private static async Task<IList<string>> GetFile()
        {
            //TODO feature idea - make title of file dialog show what type of file you're uploading (AEF, alpha, etc)
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.Downloads
            };

            picker.FileTypeFilter.Add(".csv");

            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                // Move file to Future Access List
                string fileToken = FileService.RememberFile(file);

                var fileObject = await FileService.GetFileForToken(fileToken);

                var fileLines = await FileIO.ReadLinesAsync(fileObject);

                FileService.ForgetFile(fileToken);

                return fileLines;
            }
            else
            {
                Debug.WriteLine("File is null.");

                return null;

                //    this.textBlock.Text = "Operation cancelled.";
            }
        }
    }
}
