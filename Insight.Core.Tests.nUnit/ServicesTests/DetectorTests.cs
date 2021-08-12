﻿using FluentAssertions;
using Insight.Core.Models;
using Insight.Core.Services.File;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;

namespace Insight.Core.Tests.nUnit.ServicesTests
{
	[TestFixture]
	public class DetectorTests
	{
		[TestCase(@"Test Mock Data\LoX_good_input.csv", FileType.LOX)]
		[TestCase(@"Test Mock Data\AEF_good_input.csv", FileType.AEF)]
		[TestCase(@"Test Mock Data\Alpha_good_input.csv", FileType.AlphaRoster)]
		[TestCase(@"Test Mock Data\ETMS_good_input.csv", FileType.ETMS)]
		[TestCase(@"Test Mock Data\PEX_good_input.csv", FileType.PEX)]
		[TestCase(@"Test Mock Data\SFMIS_good_input.csv", FileType.SFMIS)]
		[TestCase(@"Test Mock Data\random_text.txt", FileType.Unknown)]
		[TestCase(@"Test Mock Data\empty_file.csv", FileType.Unknown)]
		public void DetectorTest(string input, FileType expected)
		{
			IList<string> fileContents = Helper.ReadFile(input);

			FileType detectedFiletype = Detector.DetectFileType(fileContents);

			detectedFiletype.Should().Be(expected);
		}

		/// <summary>
		/// Tests that a file with null contents is handled properly
		/// </summary>
		[Test]
		public void DetectNullFileContents()
		{
			FileType detectedFiletype = Detector.DetectFileType(null);

			detectedFiletype.Should().Be(FileType.Unknown);
		}
	}
}
