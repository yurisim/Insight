using FluentAssertions;
using Insight.Core.Models;
using Insight.Core.Services;
using Insight.Core.Services.Database;
using Insight.Core.Services.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Insight.Core.Tests.xUnit.ServicesTests
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

	public class ReadFilesTest
	{
		/// <summary>
		/// Tests that a file is read properly and that the returned List<string> as expected
		/// </summary>
		/// <returns></returns>
		[Fact]
		public void ReadGoodFile()
		{
			IList<string> fileContents = Helper.ReadFile(@"Test Moc\k Data\short_file_test.csv");


			IList<string> Result = new List<string>();
			Result.Add("\"Churchill, Olivia\",ABM,E-3G(II),SMSgt,D,CMR,E,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
			Result.Add("\"Clark, Joshua\",MSO,E-3G,AB,B,CMR,E,,,,,,,,,,,,,,,,,,,X,,X,,,,,,PERS: WDA OGRA,");
			Result.Add("\"Clark, Joshua\",MSO,E-3G(II),AB,B,CMR,E,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: WDA OGRA,");
			Result.Add("\"Hart, Anthony\",ABM,E-3G,TSgt,E,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: LTDNIF ,");
			Result.Add("\"Hart, Anthony\",ABM,E-3G(II),TSgt,E,CMR,I,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: LTDNIF ,");
			Result.Add("\"Hart, Grace\",FE,E-3G,MSgt,A,CMR,E,,,,,,,X,,,,,X,X,,,,,,X,,,,,,,,,");
			Result.Add("\"Jones, Stewart\",MCC,E-3G,SrA,E,CMR,I,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: ABM TP, DNIF");
			Result.Add("\"Jones, Stewart\",MSO,E-3G(II),SrA,E,IQT,I,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
			fileContents.Should().BeEquivalentTo(Result);
		}

		/// <summary>
		/// Tests reading a file that does not exist
		/// </summary>
		/// <returns></returns>
		[Fact]
		public void ReadFileThatDoesNotExist()
		{
			IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\this_file_does_not_exist.csv");

			fileContents.Should().BeNull();
		}

		/// <summary>
		/// Tests reading an empty file
		/// </summary>
		/// <returns></returns>
		[Fact]
		public void ReadEmptyFile()
		{
			IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\empty_file.csv");

			fileContents.Should().BeNull();
		}
	}

	public class FileTests
	{
		public class DetectorTests
		{
			/// <summary>
			/// Tests that a LoX file is detected properly
			/// </summary>
			[Fact]
			public void DetectLoX()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\LoX_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.LOX);
			}

			/// <summary>
			/// Tests that an AEF file is detected properly
			/// </summary>
			[Fact]
			public void DetectAEF()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\AEF_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.AEF);
			}

			/// <summary>
			/// Tests that a Alpha Roster file is detected properly
			/// </summary>
			[Fact]
			public void DetectAlphaRoster()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\Alpha_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.AlphaRoster);
			}

			/// <summary>
			/// Tests that a ETMS file is detected properly
			/// </summary>
			[Fact]
			public void DetectETMS()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\ETMS_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.ETMS);
			}

			/// <summary>
			/// Tests that a PEX file is detected properly
			/// </summary>
			[Fact]
			public void DetectPEX()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\PEX_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.PEX);
			}

			/// <summary>
			/// Tests that an unknown type file is detected properly
			/// </summary>
			[Fact]
			public void DetectUnknown()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\random_text.txt");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.Unknown);
			}

			/// <summary>
			/// Tests that an unknown type file is detected properly
			/// </summary>
			[Fact]
			public void DetectEmptyFile()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\empty_file.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.Unknown);
			}

			/// <summary>
			/// Tests that a file with null contents is handled properly
			/// </summary>
			[Fact]
			public void NullfileContents()
			{
				FileType detectedFiletype = Detector.DetectFileType(null);

				detectedFiletype.Should().Be(FileType.Unknown);
			}
		}

		public class DigestFactoryTests
		{
			/// <summary>
			/// Tests factory creating IDgest for AlphaRoster FileType
			/// </summary>
			[Fact]
			public void FactoryAlphaRoster()
			{
				FileType fileType = FileType.AlphaRoster;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>());

				digest.Should().BeOfType<DigestAlphaRoster>();
			}

			/// <summary>
			/// Tests factory creating IDgest for PEX FileType
			/// </summary>
			[Fact]
			public void FactoryPEX()
			{
				FileType fileType = FileType.PEX;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>());

				digest.Should().BeOfType<DigestPEX>();
			}

			/// <summary>
			/// Tests factory creating IDgest for AEF FileType
			/// </summary>
			[Fact]
			public void FactoryAEF()
			{
				FileType fileType = FileType.AEF;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>());

				digest.Should().BeOfType<DigestAEF>();
			}

			/// <summary>
			/// Tests factory creating IDgest for ETMS FileType
			/// </summary>
			[Fact]
			public void FactoryETMS()
			{
				FileType fileType = FileType.ETMS;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>());

				digest.Should().BeOfType<DigestETMS>();
			}

			/// <summary>
			/// Tests factory creating IDgest for LOX FileType
			/// </summary>
			[Fact]
			public void FactoryLOX()
			{
				FileType fileType = FileType.LOX;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>());

				digest.Should().BeOfType<DigestLOX>();
			}

			/// <summary>
			/// Tests factory creating IDgest for Unknownk FileType
			/// </summary>
			[Fact]
			public void FactoryUnkown()
			{
				FileType fileType = FileType.Unknown;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>());

				digest.Should().BeNull();
			}
		}

		public class DigestAlphaRosterTests : IDisposable
		{
			private InsightController controller;
			private bool disposedValue;

			DbContextOptions<InsightContext> dbContextOptions = new DbContextOptionsBuilder<InsightContext>()
					.UseInMemoryDatabase(databaseName: "InsightTestDB")
					.Options;

			public DigestAlphaRosterTests()
			{
				

				controller = new InsightController(dbContextOptions);
			}
			protected virtual void Dispose(bool disposing)
			{
				if (!disposedValue)
				{
					if (disposing)
					{
						controller.EnsureDatabaseDeleted();
					}

					// TODO: free unmanaged resources (unmanaged objects) and override finalizer
					// TODO: set large fields to null
					disposedValue = true;
				}
			}

			// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
			// ~DigestAlphaRosterTests()
			// {
			//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			//     Dispose(disposing: false);
			// }

			public void Dispose()
			{
				// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}

			[Fact]
			public void GoodAlphaRoster()
			{
				//Set up for test
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\Alpha_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				IDigest digest = DigestFactory.GetDigestor(fileType: detectedFiletype, fileContents: fileContents, dbContextOptions);
				digest.DigestLines();



			}

			
		}
	}
}
