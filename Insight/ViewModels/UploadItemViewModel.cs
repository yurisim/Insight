using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace Insight.ViewModels
{
    public class UploadItemViewModel : ObservableObject
    {

        //private ICommand _uploadFileCommand;
        //public ICommand UploadFileCommand => _uploadFileCommand ?? (_uploadFileCommand = new RelayCommand(OpenFileDialog));

        //private string _fileType;

        //public string FileType
        //{
        //    get { return _fileType; }
        //    set { SetProperty(ref _fileType, value); }
        //}

        ////Using a DependencyProperty as the backing store for FileType.This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty FileTypeProperty =
        //     DependencyProperty.Register("FileType", typeof(string), typeof(UploadItemControl), null);

    }
}
