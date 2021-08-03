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
		public static IList<string> ReadFile(string relativeFilePath)
		{
			string filePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, @"Test Mock Data\"+relativeFilePath);
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
			IList<string> fileContents = Helper.ReadFile(@"short_file_test.csv");


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
			IList<string> fileContents = Helper.ReadFile(@"this_file_does_not_exist.csv");

			fileContents.Should().BeNull();
		}

		/// <summary>
		/// Tests reading an empty file
		/// </summary>
		/// <returns></returns>
		[Test]
		public void ReadEmptyFile()
		{
			IList<string> fileContents = Helper.ReadFile(@"empty_file.csv");

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
				IList<string> fileContents = Helper.ReadFile(@"LoX_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.LOX);
			}

			/// <summary>
			/// Tests that an AEF file is detected properly
			/// </summary>
			[Test]
			public void DetectAEF()
			{
				IList<string> fileContents = Helper.ReadFile(@"AEF_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.AEF);
			}

			/// <summary>
			/// Tests that a Alpha Roster file is detected properly
			/// </summary>
			[Test]
			public void DetectAlphaRoster()
			{
				IList<string> fileContents = Helper.ReadFile(@"Alpha_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.AlphaRoster);
			}

			/// <summary>
			/// Tests that a ETMS file is detected properly
			/// </summary>
			[Test]
			public void DetectETMS()
			{
				IList<string> fileContents = Helper.ReadFile(@"ETMS_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.ETMS);
			}

			/// <summary>
			/// Tests that a PEX file is detected properly
			/// </summary>
			[Test]
			public void DetectPEX()
			{
				IList<string> fileContents = Helper.ReadFile(@"PEX_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.PEX);
			}

			/// <summary>
			/// Tests that an unknown type file is detected properly
			/// </summary>
			[Test]
			public void DetectUnknown()
			{
				IList<string> fileContents = Helper.ReadFile(@"random_text.txt");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.Unknown);
			}

			/// <summary>
			/// Tests that an unknown type file is detected properly
			/// </summary>
			[Test]
			public void DetectEmptyFile()
			{
				IList<string> fileContents = Helper.ReadFile(@"empty_file.csv");

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
			[TestCase("LoX_good_input.csv")]
			[TestCase("LoX_switched_columns.csv")]
			[TestCase("LoX_switched_columns_2.csv")]

			//These test cases will run, but since some data will be blank so need to write it's own test for each. But then that's a lot of duplicated (or very similar) code
			//[TestCase("LoX_missing_column_cp.csv")]
			//[TestCase("LoX_missing_column_flight.csv")]
			//[TestCase("LoX_missing_column_mds.csv")]
			//[TestCase("LoX_missing_column_name.csv")]
			//[TestCase("LoX_missing_column_rank.csv")]

			public void DigestLOXTest(string filePath)
			{
				//TODO test Flight and org once org is implemented

				//Set up for test
				IList<string> fileContents = Helper.ReadFile(filePath);

				IDigest digest = DigestFactory.GetDigestor(fileType: FileType.LOX, fileContents: fileContents, dbContextOptions);
				digest.DigestLines();

				//Check database is in expected state
				Person person1 = controller.GetPersonByName("Sophie", "Alsop").Result;
				person1.Should().NotBeNull();
				person1.Rank.Should().Be(Rank.E2);
				//person1.Flight
				//person1.org

				Person person2 = controller.GetPersonByName("Neil", "Chapman").Result;
				person2.Should().NotBeNull();
				person2.Rank.Should().Be(Rank.Unknown);

				Person person3 = controller.GetPersonByName("Olivia", "Churchill").Result;
				person3.Should().NotBeNull();
				person3.Rank.Should().Be(Rank.E8);

				Person person4 = controller.GetPersonByName("Joshua", "Clark").Result;
				person4.Should().NotBeNull();
				person4.Rank.Should().Be(Rank.E1);

				Person person5 = controller.GetPersonByName("Anthony", "Hart").Result;
				person5.Should().NotBeNull();
				person5.Rank.Should().Be(Rank.E6);

				Person person6 = controller.GetPersonByName("Grace", "McCloud").Result;
				person6.Should().NotBeNull();
				person6.Rank.Should().Be(Rank.E3);

				Person person7 = controller.GetPersonByName("Dean", "St. Onge").Result;
				person7.Should().NotBeNull();
				person7.Rank.Should().Be(Rank.E7);

				Person person8 = controller.GetPersonByName("Jean", "St. Onge").Result;
				person8.Should().NotBeNull();
				person8.Rank.Should().Be(Rank.O9);

				Person person9 = controller.GetPersonByName("Julian", "Underwood").Result;
				person9.Should().NotBeNull();
				person9.Rank.Should().Be(Rank.O8);

				Person person10= controller.GetPersonByName("First", "Last Name").Result;
				person10.Should().NotBeNull();
				person10.Rank.Should().Be(Rank.O2);

				Person person11 = controller.GetPersonByName("Sim", "Yura-Sim").Result;
				person11.Should().NotBeNull();
				person11.Rank.Should().Be(Rank.O1);

				Person person12 = controller.GetPersonByName("LeKeith", "McLean").Result;
				person12.Should().NotBeNull();
				person12.Rank.Should().Be(Rank.O3);

				Person person13 = controller.GetPersonByName("Charles", "Nichols").Result;
				person13.Should().NotBeNull();
				person13.Rank.Should().Be(Rank.O10);

				//TODO This person is a 'the third' in database. Need to determine how it's handled
				//Person person14 = controller.GetPersonByName("Katherine", "Thomson").Result;
				//person14.Should().NotBeNull();
				//person14.Rank.Should().Be(Rank.E7);

				Person person15= controller.GetPersonByName("No", "Data").Result;
				person15.Should().NotBeNull();
				person15.Rank.Should().Be(Rank.Unknown);

			}
		}
	}
}
