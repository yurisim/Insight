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
    }
}
