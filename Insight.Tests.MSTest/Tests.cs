using System;
using Insight.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Insight.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace Insight.Tests.MSTest
{
	// TODO WTS: Add appropriate tests
	[TestClass]
	public class Tests
	{
		// TODO WTS: Add tests for functionality you add to OrganizationsViewModel.
		[TestMethod]
		public void TestOrganizationsViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			OrganizationsViewModel vm = new OrganizationsViewModel();
			Assert.IsNotNull(vm);
		}

		// TODO WTS: Add tests for functionality you add to OverviewViewModel.
		[TestMethod]
		public void TestOverviewViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			OverviewViewModel vm = new OverviewViewModel();
			Assert.IsNotNull(vm);
		}

		// TODO WTS: Add tests for functionality you add to SettingsViewModel.
		[TestMethod]
		public void TestSettingsViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			SettingsViewModel vm = new SettingsViewModel();
			Assert.IsNotNull(vm);
		}

		// TODO WTS: Add tests for functionality you add to StatusViewModel.
		[TestMethod]
		public void TestStatusViewModelCreation()
		{
			// This test is trivial. Add your own tests for the logic you add to the ViewModel.
			StatusViewModel vm = new StatusViewModel();
			Assert.IsNotNull(vm);
		}

		[UITestMethod]
		public void TestPercentageVeryHighStyleSelector()
		{
			PercentageColor percentageColor = new PercentageColor();
			Assert.AreEqual(percentageColor.VeryHighPercent, percentageColor.GetStyle(95));
		}

		[UITestMethod]
		public void TestPercentageHighStyleSelector()
		{
			PercentageColor percentageColor = new PercentageColor();
			Assert.AreEqual(percentageColor.HighPercent, percentageColor.GetStyle(85));
		}

		[UITestMethod]
		public void TestPercentageMidStyleSelector()
		{
			PercentageColor percentageColor = new PercentageColor();
			Assert.AreEqual(percentageColor.MediumPercent, percentageColor.GetStyle(73));
		}

		[UITestMethod]
		public void TestPercentageLowStyleSelector()
		{
			PercentageColor percentageColor = new PercentageColor();
			Assert.AreEqual(percentageColor.LowPercent, percentageColor.GetStyle(64));
		}

		[UITestMethod]
		public void TestPercentageVeryLowStyleSelector()
		{
			PercentageColor percentageColor = new PercentageColor();
			Assert.AreEqual(percentageColor.VeryLowPercent, percentageColor.GetStyle(32));
		}
	}
}
