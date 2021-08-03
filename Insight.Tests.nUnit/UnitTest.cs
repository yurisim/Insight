using System;
using NUnit.Framework;
using Insight.ViewModels;


namespace Insight.Tests.nUnit
{
	[TestFixture]
	// TODO WTS: Add appropriate tests
	public class Tests
	{

		// TODO WTS: Add tests for functionality you add to OrganizationsViewModel.
		[Test]
		public void TestOrganizationsViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			OrganizationsViewModel vm = new OrganizationsViewModel();
			Assert.NotNull(vm);
		}

		// TODO WTS: Add tests for functionality you add to OverviewViewModel.
		[Test]
		public void TestOverviewViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			OverviewViewModel vm = new OverviewViewModel();
			Assert.NotNull(vm);
		}

		// TODO WTS: Add tests for functionality you add to SettingsViewModel.
		[Test]
		public void TestSettingsViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			SettingsViewModel vm = new SettingsViewModel();
			Assert.NotNull(vm);
		}

		// TODO WTS: Add tests for functionality you add to StatusViewModel.
		[Test]
		public void TestStatusViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			StatusViewModel vm = new StatusViewModel();
			Assert.NotNull(vm);
		}

		//// TODO WTS: Add tests for functionality you add to WebViewViewModel.
		//[Test]
		//public void TestWebViewViewModelCreation()
		//{
		//	// This test is trivial. Add your own tests for the logic you add to the ViewModel.
		//	WebViewViewModel vm = new WebViewViewModel();
		//	Assert.NotNull(vm);
		//}
	}
}
