using Insight.ViewModels;

using Microsoft.Toolkit.Uwp.UI.Animations;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Insight.Views
{
	public sealed partial class OverviewDetailPage : Page
	{
		public OverviewDetailViewModel ViewModel { get; set; }

		public OverviewDetailPage()
		{
			InitializeComponent();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModel = new OverviewDetailViewModel(e.Parameter.ToString());
			this.RegisterElementForConnectedAnimation("animationKeyOverview", itemHero);
			await ViewModel.LoadDataAsync();
		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			base.OnNavigatingFrom(e);
			if (e.NavigationMode == NavigationMode.Back)
			{
				//NavigationService.Frame.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
			}
		}
	}
}
