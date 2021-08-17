using Insight.Helpers;
using Insight.Services;
using Insight.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

		/// <summary>
		/// Opens the OverviewDetailPage and sends the selected org to it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StackPanel_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			List<TextBlock> textBlocks = new List<TextBlock>();
			textBlocks.AddRange(((sender as StackPanel).Children.OfType<TextBlock>()));
			string org = textBlocks.FirstOrDefault(x => x.Tag.Equals("OrgBlock"))?.Text;
            NavigationService.Navigate<OverviewDetailPage>(org);
        }
    }
}
