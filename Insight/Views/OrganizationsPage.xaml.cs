using System;

using Insight.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Insight.Views
{
    public sealed partial class OrganizationsPage : Page
    {
        public OrganizationsViewModel ViewModel { get; } = new OrganizationsViewModel();

        public OrganizationsPage()
        {
            InitializeComponent();
            Loaded += OrganizationsPage_Loaded;
        }

        private async void OrganizationsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(ListDetailsViewControl.ViewState);
        }
    }
}
