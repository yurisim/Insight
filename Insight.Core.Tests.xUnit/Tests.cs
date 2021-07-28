using Insight.Core.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Insight.Core.Services.File;
using Insight.Core.Models;
using System;

namespace Insight.Core.Tests.XUnit
{
	public class Helper
	{
		//TODO this is a stop gap to allow me to continue writing tasks. In the future read file needs to be moved out of the front end
		public static IList<string> ReadFile(string filePath)
		{
			IList<string> result = new List<string>();
			using (var sr = new StreamReader(filePath))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					result.Add(line);
				}
			}
			return result;
			//return File.ReadLines(filePath).ToList();
		}
	}

	public class TestFiles
	{
		[Fact]
		public async Task ReadGoodFile()
		{
			IList<string> FileContents = Helper.ReadFile(@"Test Mock Data\short_file_test.csv");


			IList<string> Result = new List<string>();
			Result.Add("\"Churchill, Olivia\",ABM,E-3G(II),SMSgt,D,CMR,E,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
			Result.Add("\"Clark, Joshua\",MSO,E-3G,AB,B,CMR,E,,,,,,,,,,,,,,,,,,,X,,X,,,,,,PERS: WDA OGRA,");
			Result.Add("\"Clark, Joshua\",MSO,E-3G(II),AB,B,CMR,E,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: WDA OGRA,");
			Result.Add("\"Hart, Anthony\",ABM,E-3G,TSgt,E,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: LTDNIF ,");
			Result.Add("\"Hart, Anthony\",ABM,E-3G(II),TSgt,E,CMR,I,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: LTDNIF ,");
			Result.Add("\"Hart, Grace\",FE,E-3G,MSgt,A,CMR,E,,,,,,,X,,,,,X,X,,,,,,X,,,,,,,,,");
			Result.Add("\"Jones, Stewart\",MCC,E-3G,SrA,E,CMR,I,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: ABM TP, DNIF");
			Result.Add("\"Jones, Stewart\",MSO,E-3G(II),SrA,E,IQT,I,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
			FileContents.Should().BeEquivalentTo(Result);
		}

		[Fact]
		public async Task ReadFileThatDoesNotExist()
		{
			IList<string> FileContents = Helper.ReadFile(@"Test Mock Data\this_file_does_not_exist.csv");

			FileContents.Should().BeNull();
		}

		public async Task ReadEmptyFile()
		{
			IList<string> FileContents = Helper.ReadFile(@"Test Mock Data\empty_file.csv");

			FileContents.Should().BeNull();
		}

		public async Task ReadWrongFileExtension()
		{
			IList<string> FileContents = Helper.ReadFile(@"Test Mock Data\random.idk");

			FileContents.Should().BeNull();
		}

		[Fact]
		public async Task ReadGoodLoX()
		{
			IList<string> FileContents = Helper.ReadFile(@"Test Mock Data\LoX_good_input.csv");
		}
	}

	public class DetectorTests
	{
		[Fact]
		public void DetectLoX()
		{
			IList<string> FileContents = Helper.ReadFile(@"Test Mock Data\LoX_good_input.csv");

			FileContents.Should().NotBeNullOrEmpty();

			FileType detectedFiletype = Detector.DetectFileType(FileContents);

			detectedFiletype.Should().Be(FileType.LOX);
		}

		[Fact]
		public void DetectAEF()
		{
			IList<string> FileContents = Helper.ReadFile(@"Test Mock Data\AEF_good_input.csv");

			FileContents.Should().NotBeNullOrEmpty();

			FileType detectedFiletype = Detector.DetectFileType(FileContents);

			detectedFiletype.Should().Be(FileType.AEF);
		}

		[Fact]
		public void DetectAlpha()
		{
			IList<string> FileContents = Helper.ReadFile(@"Test Mock Data\Alpha_good_input.csv");

			FileContents.Should().NotBeNullOrEmpty();

			FileType detectedFiletype = Detector.DetectFileType(FileContents);

			detectedFiletype.Should().Be(FileType.AlphaRoster);
		}

		[Fact]
		public void DetectETMS()
		{
			IList<string> FileContents = Helper.ReadFile(@"Test Mock Data\ETMS_good_input.csv");

			FileContents.Should().NotBeNullOrEmpty();

			FileType detectedFiletype = Detector.DetectFileType(FileContents);

			detectedFiletype.Should().Be(FileType.ETMS);
		}

		[Fact]
		public void DetectPEX()
		{
			IList<string> FileContents = Helper.ReadFile(@"Test Mock Data\PEX_good_input.csv");

			FileContents.Should().NotBeNullOrEmpty();

			FileType detectedFiletype = Detector.DetectFileType(FileContents);

			detectedFiletype.Should().Be(FileType.PEX);
		}

		[Fact]
		public void DetectUnknown()
		{
			IList<string> FileContents = Helper.ReadFile(@"Test Mock Data\empty_file.csv");

			FileContents.Should().NotBeNullOrEmpty();

			FileType detectedFiletype = Detector.DetectFileType(FileContents);

			detectedFiletype.Should().Be(FileType.Unknown);
		}
	}

}
