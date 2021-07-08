using Insight.Helpers;
using Insight.Services;
using Insight.ViewModels;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Insight.Views
{
    public sealed partial class OverviewPage : Page
    {
        public OverviewViewModel ViewModel { get; } = new OverviewViewModel();

        public OverviewPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }

        private void StackPanel_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            NavigationService.Navigate<OverviewDetailPage>();
        }
    }
}
