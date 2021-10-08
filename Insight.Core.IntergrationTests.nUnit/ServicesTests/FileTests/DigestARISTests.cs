using FluentAssertions;
using FluentAssertions.Execution;
using Insight.Core.Models;
using Insight.Core.Services;
using Insight.Core.Services.Database;
using Insight.Core.Services.File;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Insight.Core.IntegrationTests.nUnit.ServicesTests.FileTests
{
	[TestFixture]
	public class DigestARISTests
	{

		public InsightController insightController;

		private static readonly DbContextOptions<InsightContext> dbContextOptions =
			new DbContextOptionsBuilder<InsightContext>()
				.UseInMemoryDatabase("InsightTestDB")
				.Options;

		[SetUp]
		public void SetUp()
		{
			insightController = new InsightController(dbContextOptions);
		}

		[TearDown]
		public void TearDown()
		{
			insightController.EnsureDatabaseDeleted();
		}

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestARIS_ExpectOnePersonsTestCases))]
		public void DigestARISTest_ExpectOnePerson(TestCaseObject testCaseParameters)
		{
			var (input, expectedFirstName, expectedLastName, expectedFileType, courseCompletionExpected, courseExpirationExpected) = testCaseParameters;

			//arrange
			FileType detectedFileType = Detector.DetectFileType(input);

			IDigest digest = DigestFactory.GetDigestor(detectedFileType, input, dbContextOptions);

			//creates person entity in DB so there's someone to look up
			Person personToCreateInDB = new Person()
			{
				FirstName = expectedFirstName,
				LastName = expectedLastName,
			};
			insightController.Add(personToCreateInDB);

			//act
			digest.CleanInput();
			digest.DigestLines();

			//arrange 2.0
			var allPersons = insightController.GetAllPersons().Result;
			var person = insightController.GetPersonsByName(firstName: expectedFirstName, lastName: expectedLastName).Result.FirstOrDefault();

			Course course = null;
			if (expectedFileType == FileType.ARIS_Handgun)
			{
				course = insightController.GetCoursesByName(WeaponCourseTypes.Handgun).Result.FirstOrDefault();
			}
			else if (expectedFileType == FileType.ARIS_Rifle_Carbine)
			{
				course = insightController.GetCoursesByName(WeaponCourseTypes.Rifle_Carbine).Result.FirstOrDefault();
			}

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(expectedFileType);
				digest.Should().BeOfType<DigestARIS>();

				allPersons.Count.Should().Be(1);

				person.Should().NotBeNull();

				CourseInstance courseInstanceToCheck = new CourseInstance()
				{
					Person = person,
					Course = course,
					Completion = courseCompletionExpected,
					Expiration = courseExpirationExpected
				};
				CourseInstance courseInstanceFromDB = insightController.GetCourseInstances(courseInstanceToCheck).Result.FirstOrDefault();

				courseInstanceFromDB.Should().NotBeNull();

				course.CourseInstances.Should().HaveCount(1);
				person.CourseInstances.Should().HaveCount(1);
			}
		}

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestAris_ExpectOnePeron_ZeroCouresTestCases))]
		public void DigestARISTest_ExpectOnePerson_ZeroCourses(TestCaseObject testCaseParameters)
		{
			var (input, expectedFirstName, expectedLastName, expectedFileType) = testCaseParameters;

			//arrange
			FileType detectedFileType = Detector.DetectFileType(input);

			IDigest digest = DigestFactory.GetDigestor(detectedFileType, input, dbContextOptions);

			//creates person entity in DB so there's someone to look up
			Person personToCreateInDB = new Person()
			{
				FirstName = expectedFirstName,
				LastName = expectedLastName,
			};
			insightController.Add(personToCreateInDB);

			//act
			digest.CleanInput();
			digest.DigestLines();

			//arrange 2.0
			var allPersons = insightController.GetAllPersons().Result;
			var allCourses = insightController.GetAll<Course>().Result;
			var person = insightController.GetPersonByName(firstName: expectedFirstName, lastName: expectedLastName).Result;

			Course course = null;
			if (expectedFileType == FileType.ARIS_Handgun)
			{
				course = insightController.GetCourseByName(WeaponCourseTypes.Handgun).Result;
			}
			else if (expectedFileType == FileType.ARIS_Rifle_Carbine)
			{
				course = insightController.GetCourseByName(WeaponCourseTypes.Rifle_Carbine).Result;
			}

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(expectedFileType);
				digest.Should().BeOfType<DigestARIS>();

				allPersons.Count.Should().Be(1);
				allCourses.Should().HaveCount(0);

				person.Should().NotBeNull();

				person.CourseInstances.Should().BeNullOrEmpty();
			}
		}

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestARIS_ExpectZeroPersonsTestCases))]
		public void DigestARISTest_ExpectZeroPerson(TestCaseObject testCaseParameters)
		{
			var (input, expectedFileType) = testCaseParameters;

			//arrange
			FileType detectedFileType = Detector.DetectFileType(input);

			IDigest digest = DigestFactory.GetDigestor(detectedFileType, input, dbContextOptions);

			//act
			digest.CleanInput();
			digest.DigestLines();

			//arrange 2.0
			var allPersons = insightController.GetAll<Person>().Result;
			var courses = insightController.GetAll<Course>().Result;

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(expectedFileType);
				allPersons.Count.Should().Be(0);
				courses.Count.Should().Be(0);
			}
		}

		/// <summary>
		/// Objects containing the data for the test cases
		/// </summary>
		private class TestCasesObjects
		{
			public static object[] DigestARIS_ExpectOnePersonsTestCases =
			{
				//test case - base case Handgun
				new TestCaseObject(
					input: new List<string>
					{
						"People Assigned,,,,,,,,,,,,",
						",,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"HANDGUN (GROUP B),,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Name,Organizations,Control AFSC,Duty AFSC,Rank,Scheduled Date,Last Completion Date,Status,Qual Expiration Date,Qual Status,User Assigned,Do Not Schedule,Licensed",
						"\"Alsop, Sophie\",960 AIRBORNE AIR CTRL SQ FFDFP0,,,AMN,,26 Apr 2021,CURRENT,30 Apr 2022,QUALIFIED,Y,N,N"
					},
					expectedFirstName: "Sophie",
					expectedLastName: "Alsop",
					expectedFileType: FileType.ARIS_Handgun,
					courseCompletionExpected: DateTime.Parse("26 Apr 2021"),
					courseExpirationExpected: DateTime.Parse("30 Apr 2022")
				),

				//test case - base case Rifle/Carbine
				new TestCaseObject(
					input: new List<string>
					{
						"People Assigned,,,,,,,,,,,,",
						",,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Rifle/Carbine (Group B)),,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Name,Organizations,Control AFSC,Duty AFSC,Rank,Scheduled Date,Last Completion Date,Status,Qual Expiration Date,Qual Status,User Assigned,Do Not Schedule,Licensed",
						"\"Alsop, Sophie\",960 AIRBORNE AIR CTRL SQ FFDFP0,,,AMN,,26 May 2021,CURRENT,30 May 2022,QUALIFIED,Y,N,N"
					},
					expectedFirstName: "Sophie",
					expectedLastName: "Alsop",
					expectedFileType: FileType.ARIS_Rifle_Carbine,
					courseCompletionExpected: DateTime.Parse("26 May 2021"),
					courseExpirationExpected: DateTime.Parse("30 May 2022")
				),

				//test case - extra empty lines
				new TestCaseObject(
					input: new List<string>
					{
						"People Assigned,,,,,,,,,,,,",
						",,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Rifle/Carbine (Group B)),,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Name,Organizations,Control AFSC,Duty AFSC,Rank,Scheduled Date,Last Completion Date,Status,Qual Expiration Date,Qual Status,User Assigned,Do Not Schedule,Licensed",
						"\"Alsop, Sophie\",960 AIRBORNE AIR CTRL SQ FFDFP0,,,AMN,,26 May 2021,CURRENT,30 May 2022,QUALIFIED,Y,N,N",
						",,,,,,,,,,,,"
					},
					expectedFirstName: "Sophie",
					expectedLastName: "Alsop",
					expectedFileType: FileType.ARIS_Rifle_Carbine,
					courseCompletionExpected: DateTime.Parse("26 May 2021"),
					courseExpirationExpected: DateTime.Parse("30 May 2022")
				),
			};

			public static object[] DigestAris_ExpectOnePeron_ZeroCouresTestCases =
			{
				//test case - extra empty lines
				new TestCaseObject(
					input: new List<string>
					{
						"People Assigned,,,,,,,,,,,,",
						",,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Rifle/Carbine (Group B)),,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Name,these,are,random,columns",
						"\"Alsop, Sophie\",more,random,stuff,yaknow",
						",,,,,,,,,,,,"
					},
					expectedFirstName: "Sophie",
					expectedLastName: "Alsop",
					expectedFileType: FileType.ARIS_Rifle_Carbine
				),
			};

			public static object[] DigestARIS_ExpectZeroPersonsTestCases =
			{
				//test case - no first name
				new TestCaseObject(
					input: new List<string>
					{
						"People Assigned,,,,,,,,,,,,",
						",,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"HANDGUN (GROUP B),,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Name,Organizations,Control AFSC,Duty AFSC,Rank,Scheduled Date,Last Completion Date,Status,Qual Expiration Date,Qual Status,User Assigned,Do Not Schedule,Licensed",
						"\"lastName, \",960 AIRBORNE AIR CTRL SQ FFDFP0,,,AMN,,5 Nov 2021,CURRENT,30 Nov 2022,QUALIFIED,Y,N,N"
					},
					expectedFileType: FileType.ARIS_Handgun
				),

				//test case - no last name
				new TestCaseObject(
					input: new List<string>
					{
						"People Assigned,,,,,,,,,,,,",
						",,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Rifle/Carbine (Group B)),,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Name,Organizations,Control AFSC,Duty AFSC,Rank,Scheduled Date,Last Completion Date,Status,Qual Expiration Date,Qual Status,User Assigned,Do Not Schedule,Licensed",
						"\", FirstName\",960 AIRBORNE AIR CTRL SQ FFDFP0,,,AMN,,26 Apr 2021,CURRENT,30 Apr 2022,QUALIFIED,Y,N,N"
					},
					expectedFileType: FileType.ARIS_Rifle_Carbine
				),

				//test case - no name
				new TestCaseObject(
					input: new List<string>
					{
						"People Assigned,,,,,,,,,,,,",
						",,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Rifle/Carbine (Group B)),,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Name,Organizations,Control AFSC,Duty AFSC,Rank,Scheduled Date,Last Completion Date,Status,Qual Expiration Date,Qual Status,User Assigned,Do Not Schedule,Licensed",
						"\", \",960 AIRBORNE AIR CTRL SQ FFDFP0,,,AMN,,26 Apr 2021,CURRENT,30 Apr 2022,QUALIFIED,Y,N,N"
					},
					expectedFileType: FileType.ARIS_Rifle_Carbine
				),
			};
		}

		/// <summary>
		/// Container outline for all of the required input and expected outputs for the tests
		/// </summary>
		public class TestCaseObject
		{
			private readonly IList<string> _input;
			private readonly string _expectedFirstName;
			private readonly string _expectedLastName;
			private readonly FileType _expectedFileType;
			private readonly DateTime _courseCompletionExpected;
			private readonly DateTime _courseExpirationExpected;

			public TestCaseObject(IList<string> input, string expectedFirstName, string expectedLastName, FileType expectedFileType, DateTime courseCompletionExpected, DateTime courseExpirationExpected)
			{
				_input = input;
				_expectedFirstName = expectedFirstName;
				_expectedLastName = expectedLastName;
				_expectedFileType = expectedFileType;
				_courseCompletionExpected = courseCompletionExpected;
				_courseExpirationExpected = courseExpirationExpected;
			}

			public void Deconstruct(out IList<string> input, out string expectedFirstName, out string expectedLastName, out FileType expectedFileType, out DateTime courseCompletionExpected, out DateTime courseExpirationExpected)
			{
				input = _input;
				expectedFirstName = _expectedFirstName;
				expectedLastName = _expectedLastName;
				expectedFileType = _expectedFileType;
				courseCompletionExpected = _courseCompletionExpected;
				courseExpirationExpected = _courseExpirationExpected;
			}

			public TestCaseObject(IList<string> input, string expectedFirstName, string expectedLastName, FileType expectedFileType)
			{
				_input = input;
				_expectedFirstName = expectedFirstName;
				_expectedLastName = expectedLastName;
				_expectedFileType = expectedFileType;
			}

			public void Deconstruct(out IList<string> input, out string expectedFirstName, out string expectedLastName, out FileType expectedFileType)
			{
				input = _input;
				expectedFirstName = _expectedFirstName;
				expectedLastName = _expectedLastName;
				expectedFileType = _expectedFileType;
			}


			public TestCaseObject(IList<string> input, FileType expectedFileType)
			{
				_input = input;
				_expectedFileType = expectedFileType;
			}

			public void Deconstruct(out IList<string> input, out FileType expectedFileType)
			{
				input = _input;
				expectedFileType = _expectedFileType;
			}
		}
	}
}
