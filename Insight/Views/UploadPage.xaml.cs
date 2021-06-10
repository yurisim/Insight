using Insight.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Insight.Views
{
    public sealed partial class UploadPage : Page
    {
        public UploadViewModel ViewModel { get; } = new UploadViewModel();

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on StatusPage.xaml.
        // For help see http://docs.telerik.com/windows-universal/controls/raddatagrid/gettingstarted
        // You may also want to extend the grid to work with the RadDataForm http://docs.telerik.com/windows-universal/controls/raddataform/dataform-gettingstarted
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
