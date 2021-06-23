using Insight.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Insight.Views
{
    public sealed partial class UploadPage : Page
    {
        public UploadViewModel ViewModel { get; } = new UploadViewModel();

        public UploadPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
