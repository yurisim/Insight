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
using FluentAssertions.Execution;
using System.Linq;

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
			/// Tests that a ARIS file is detected properly
			/// </summary>
			[Test]
			public void DetectARIS()
			{
				IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\ARIS_good_input.csv");

				FileType detectedFiletype = Detector.DetectFileType(fileContents);

				detectedFiletype.Should().Be(FileType.ARIS);
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
		/// <summary>
		/// Digest Factory Tests
		/// </summary>
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
			/// Tests factory creating IDgest for ARIS FileType
			/// </summary>
			[Test]
			public void FactoryARIS()
			{
				FileType fileType = FileType.ARIS;

				IDigest digest = DigestFactory.GetDigestor(fileType, fileContents: new List<string>(), dbContextOptions);

				digest.Should().BeOfType<DigestARIS>();
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
			[TestCase(@"Test Mock Data\LoX_good_input.csv")]
			[TestCase(@"Test Mock Data\LoX_switched_columns.csv")]
			[TestCase(@"Test Mock Data\LoX_switched_columns_2.csv")]
			//These test cases will run, but since some data will be blank so need to write it's own test for each. But then that's a lot of duplicated (or very similar) code
			//[TestCase(@"Test Mock Data\LoX_missing_column_cp.csv")]
			//[TestCase(@"Test Mock Data\LoX_missing_column_flight.csv")]
			//[TestCase(@"Test Mock Data\LoX_missing_column_mds.csv")]
			//[TestCase(@"Test Mock Data\LoX_missing_column_name.csv")]
			//[TestCase(@"Test Mock Data\LoX_missing_column_rank.csv")]
			public void DigestLOXTest(string filePath)
			{
				//TODO test Flight and org once org is implemented

				//Set up for test
				IList<string> fileContents = Helper.ReadFile(filePath);

				IDigest digest = new DigestLOX(fileContents, dbContextOptions);
				digest.CleanInput();
				digest.DigestLines();

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Sophie", "Alsop", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.E2);
					//person.Flight
					//person.org
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Neil", "Chapman", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.Unknown);
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Olivia", "Churchill", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.E8);
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Joshua", "Clark", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.E1);
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Anthony", "Hart", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.E6);
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Grace", "McCloud", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.E3);
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Dean", "St. Onge", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.E7);
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Jean", "St. Onge", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.O9);
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Julian", "Underwood", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.O8);
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("First", "Last Name", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.O2);
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Sim", "Yura-Sim", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.O1);
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("LeKeith", "McLean", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.O3);
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Charles", "Nichols", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.O10);
				}

				using (new AssertionScope())
				{

					Person person = controller.GetPersonByName("No", "Data", false).Result;
					person.Should().NotBeNull();
					person.Rank.Should().Be(Rank.Unknown);
				}
			}

			[TestCase(@"Test Mock Data\ARIS_good_input.csv")]
			public void DigestARISTest(string filePath)
			{
				IList<string> fileContents = Helper.ReadFile(filePath);

				IDigest digest = new DigestARIS(fileContents, dbContextOptions);
				digest.CleanInput();
				digest.DigestLines();

				Course cbrnCourse = controller.GetCourseByName("CBRN");
				Course catmCourse = controller.GetCourseByName("CATM");

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Sophie", "Alsop", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Neil", "Chapman", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Olivia", "Churchill", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Joshua", "Clark", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Anthony", "Hart", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Grace", "McCloud", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Grace", "McCloud", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Jean", "St. Onge", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Julian", "Underwood", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("First", "Last Name", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Sim", "Yura-Sim", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("LeKeith", "McLean", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("Charles", "Nichols", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}

				using (new AssertionScope())
				{
					Person person = controller.GetPersonByName("No", "Data", true).Result;
					person.CourseInstances.Count.Should().NotBe(0);

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == cbrnCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}

					using (new AssertionScope())
					{
						List<CourseInstance> courseInstances = person.CourseInstances.Where(c => c.Course.Id == catmCourse.Id).ToList();
						courseInstances.Count.Should().Be(1);
						courseInstances.First().Completion.Should().Be(new DateTime());
					}
				}
			}
		}
	}
}
