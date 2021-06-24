using Insight.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Insight.ViewModels;

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
        public static readonly DependencyProperty FileTypeProperty =
            DependencyProperty.Register("FileType", typeof(string), typeof(UploadItemControl), null);
    }
}
