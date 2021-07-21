using Insight.ViewModels;

using Xunit;

namespace Insight.Tests.XUnit
{
	// TODO WTS: Add appropriate tests
	public class Tests
	{
		[Fact]
		public void TestMethod1()
		{
		}

		// TODO WTS: Add tests for functionality you add to OrganizationsViewModel.
		[Fact]
		public void TestOrganizationsViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			OrganizationsViewModel vm = new OrganizationsViewModel();
			Assert.NotNull(vm);
		}

		// TODO WTS: Add tests for functionality you add to OverviewViewModel.
		[Fact]
		public void TestOverviewViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			OverviewViewModel vm = new OverviewViewModel();
			Assert.NotNull(vm);
		}

		// TODO WTS: Add tests for functionality you add to SettingsViewModel.
		[Fact]
		public void TestSettingsViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			SettingsViewModel vm = new SettingsViewModel();
			Assert.NotNull(vm);
		}

		// TODO WTS: Add tests for functionality you add to StatusViewModel.
		[Fact]
		public void TestStatusViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			StatusViewModel vm = new StatusViewModel();
			Assert.NotNull(vm);
		}

		// TODO WTS: Add tests for functionality you add to WebViewViewModel.
		[Fact]
		public void TestWebViewViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			WebViewViewModel vm = new WebViewViewModel();
			Assert.NotNull(vm);
		}
	}
}