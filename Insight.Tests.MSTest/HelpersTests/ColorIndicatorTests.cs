using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Insight.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace Insight.Tests.MSTest.HelpersTests
{
	class ColorIndicatorTests
	{
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
