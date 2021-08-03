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
using NUnit.Framework;

namespace Insight.Core.Tests.nUnit.ServicesTests
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

	[TestFixture]
	public class ReadFilesTest
	{
		/// <summary>
		/// Tests that a file is read properly and that the returned List<string> as expected
		/// </summary>
		/// <returns></returns>
		[Test]
		public void ReadGoodFile()
		{
			IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\short_file_test.csv");


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
		[Test]
		public void ReadFileThatDoesNotExist()
		{
			IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\this_file_does_not_exist.csv");

			fileContents.Should().BeNull();
		}

		/// <summary>
		/// Tests reading an empty file
		/// </summary>
		/// <returns></returns>
		[Test]
		public void ReadEmptyFile()
		{
			IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\empty_file.csv");

			fileContents.Should().BeNullOrEmpty();
		}
	}

	public class FileTests
	{
		[TestFixture]
		public class DetectorTests
		{
			/// <summary>
			/// Tests that a LoX file is detected properly
			/// </summary>
			[Test]
			public void DetectLoX()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\LoX_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.LOX);
			}

			/// <summary>
			/// Tests that an AEF file is detected properly
			/// </summary>
			[Test]
			public void DetectAEF()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\AEF_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.AEF);
			}

			/// <summary>
			/// Tests that a Alpha Roster file is detected properly
			/// </summary>
			[Test]
			public void DetectAlphaRoster()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\Alpha_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.AlphaRoster);
			}

			/// <summary>
			/// Tests that a ETMS file is detected properly
			/// </summary>
			[Test]
			public void DetectETMS()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\ETMS_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.ETMS);
			}

			/// <summary>
			/// Tests that a PEX file is detected properly
			/// </summary>
			[Test]
			public void DetectPEX()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\PEX_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.PEX);
			}

			/// <summary>
			/// Tests that an unknown type file is detected properly
			/// </summary>
			[Test]
			public void DetectUnknown()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\random_text.txt");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.Unknown);
			}

			/// <summary>
			/// Tests that an unknown type file is detected properly
			/// </summary>
			[Test]
			public void DetectEmptyFile()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\empty_file.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.Unknown);
			}

			/// <summary>
			/// Tests that a file with null contents is handled properly
			/// </summary>
			[Test]
			public void DetectNullfileContents()
			{
				FileType detectedFiletype = Detector.DetectFileType(null);

				detectedFiletype.Should().Be(FileType.Unknown);
			}
		}

		[TestFixture]
		public class DigestFactoryTests
		{
			DbContextOptions<InsightContext> dbContextOptions = new DbContextOptionsBuilder<InsightContext>()
				.UseInMemoryDatabase(databaseName: "InsightTestDB")
				.Options;

			/// <summary>
			/// Tests factory creating IDgest for AlphaRoster FileType
			/// </summary>
			[Test]
			public void FactoryAlphaRoster()
			{
				FileType fileType = FileType.AlphaRoster;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>(), dbContextOptions);

				digest.Should().BeOfType<DigestAlphaRoster>();
			}

			/// <summary>
			/// Tests factory creating IDgest for PEX FileType
			/// </summary>
			[Test]
			public void FactoryPEX()
			{
				FileType fileType = FileType.PEX;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>(), dbContextOptions);

				digest.Should().BeOfType<DigestPEX>();
			}

			/// <summary>
			/// Tests factory creating IDgest for AEF FileType
			/// </summary>
			[Test]
			public void FactoryAEF()
			{
				FileType fileType = FileType.AEF;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>(), dbContextOptions);

				digest.Should().BeOfType<DigestAEF>();
			}

			/// <summary>
			/// Tests factory creating IDgest for ETMS FileType
			/// </summary>
			[Test]
			public void FactoryETMS()
			{
				FileType fileType = FileType.ETMS;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>(), dbContextOptions);

				digest.Should().BeOfType<DigestETMS>();
			}

			/// <summary>
			/// Tests factory creating IDgest for LOX FileType
			/// </summary>
			[Test]
			public void FactoryLOX()
			{
				FileType fileType = FileType.LOX;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>(), dbContextOptions);

				digest.Should().BeOfType<DigestLOX>();
			}

			/// <summary>
			/// Tests factory creating IDgest for Unknownk FileType
			/// </summary>
			[Test]
			public void FactoryUnkown()
			{
				FileType fileType = FileType.Unknown;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>(), dbContextOptions);

				digest.Should().BeNull();
			}
		}

		[TestFixture]
		public class DigestTests
		{
			private InsightController controller;
			private DbContextOptions<InsightContext> dbContextOptions = new DbContextOptionsBuilder<InsightContext>()
					.UseInMemoryDatabase(databaseName: "InsightTestDB")
					.Options;

			/// <summary>
			/// Setup run before every test
			/// </summary>
			[SetUp]
			public void SetUp()
			{
				controller = new InsightController(dbContextOptions);
			}

			/// <summary>
			/// Tear down run after every test
			/// </summary>
			[TearDown]
			public void Dispose()
			{
				controller.EnsureDatabaseDeleted();
			}

			/// <summary>
			/// Tests that the data from a LoX file is added to the database properly
			/// </summary>
			[Test]
			public void DigestLOXTest()
			{
				//TODO test Flight and org once org is implemented

				//Set up for test
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\LoX_good_input.csv");

				IDigest digest = DigestFactory.GetDigestor(fileType: FileType.LOX, fileContents: fileContents, dbContextOptions);
				digest.DigestLines();

				//Check database is in expected state
				Person person1 = controller.GetPersonByName("Sophie", "Alsop");
				person1.Should().NotBeNull();
				person1.Rank.Should().Be(Rank.E2);
				//person1.Flight
				//person1.org

				Person person2 = controller.GetPersonByName("Neil", "Chapman");
				person2.Should().NotBeNull();
				person2.Rank.Should().Be(Rank.Unknown);

				Person person3 = controller.GetPersonByName("Olivia", "Churchill");
				person3.Should().NotBeNull();
				person3.Rank.Should().Be(Rank.E8);

				Person person4 = controller.GetPersonByName("Joshua", "Clark");
				person4.Should().NotBeNull();
				person4.Rank.Should().Be(Rank.E1);

				Person person5 = controller.GetPersonByName("Anthony", "Hart");
				person5.Should().NotBeNull();
				person5.Rank.Should().Be(Rank.E6);

				Person person6 = controller.GetPersonByName("Grace", "McCloud");
				person6.Should().NotBeNull();
				person6.Rank.Should().Be(Rank.E3);

				Person person7 = controller.GetPersonByName("Dean", "St. Onge");
				person7.Should().NotBeNull();
				person7.Rank.Should().Be(Rank.E7);

				Person person8 = controller.GetPersonByName("Jean", "St. Onge");
				person8.Should().NotBeNull();
				person8.Rank.Should().Be(Rank.O9);

				Person person9 = controller.GetPersonByName("Julian", "Underwood");
				person9.Should().NotBeNull();
				person9.Rank.Should().Be(Rank.O8);

				Person person10= controller.GetPersonByName("First", "Last Name");
				person10.Should().NotBeNull();
				person10.Rank.Should().Be(Rank.O2);

				Person person11 = controller.GetPersonByName("Sim", "Yura-Sim");
				person11.Should().NotBeNull();
				person11.Rank.Should().Be(Rank.O1);

				Person person12 = controller.GetPersonByName("LeKeith", "McLean");
				person12.Should().NotBeNull();
				person12.Rank.Should().Be(Rank.O3);

				Person person13 = controller.GetPersonByName("Charles", "Nichols");
				person13.Should().NotBeNull();
				person13.Rank.Should().Be(Rank.O10);

				Person person14 = controller.GetPersonByName("Katherine", "Thomson");
				person14.Should().NotBeNull();
				person14.Rank.Should().Be(Rank.E7);
			
			}
		}
	}
}
