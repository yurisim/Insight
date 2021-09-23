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
using System.ComponentModel.DataAnnotations;

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
			var person = insightController.GetPersonByName(firstName: expectedFirstName, lastName: expectedLastName).Result;
			Course course;
			if (expectedFileType == FileType.ARIS_Handgun)
			{
				course = insightController.GetCourseByName(WeaponCourseTypes.Handgun).Result;
			}
			else if (expectedFileType == FileType.ARIS_Rifle_Carbine)
			{
				course = insightController.GetCourseByName(WeaponCourseTypes.Rifle_Carbine).Result;
			}
			else
			{
				//Should never be run, test will fail if it does
				course = new Course();
			}

			

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(expectedFileType);
				digest.Should().BeOfType<DigestARIS>();

				

				allPersons.Count.Should().Be(1);

				person.Should().NotBeNull();

				person.FirstName.Should().Be(expectedFirstName.Trim().ToUpperInvariant());
				person.LastName.Should().Be(expectedLastName.Trim().ToUpperInvariant());

				course.CourseInstances.Count.Should().NotBe(0);
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

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(expectedFileType);
				allPersons.Count.Should().Be(0);

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
					expectedFileType: FileType.ARIS_Handgun
				),

				//test case - base case Rifle/Carbine
				new TestCaseObject(
					input: new List<string>
					{
						"People Assigned,,,,,,,,,,,,",
						",,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"RIFLE/CARBINE (Group B),,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Name,Organizations,Control AFSC,Duty AFSC,Rank,Scheduled Date,Last Completion Date,Status,Qual Expiration Date,Qual Status,User Assigned,Do Not Schedule,Licensed",
						"\"Alsop, Sophie\",960 AIRBORNE AIR CTRL SQ FFDFP0,,,AMN,,26 Apr 2021,CURRENT,30 Apr 2022,QUALIFIED,Y,N,N"
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
						"RIFLE/CARBINE (Group B),,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Name,Organizations,Control AFSC,Duty AFSC,Rank,Scheduled Date,Last Completion Date,Status,Qual Expiration Date,Qual Status,User Assigned,Do Not Schedule,Licensed",
						"\"lastName, \",960 AIRBORNE AIR CTRL SQ FFDFP0,,,AMN,,26 Apr 2021,CURRENT,30 Apr 2022,QUALIFIED,Y,N,N"
					},
					expectedFileType: FileType.ARIS_Rifle_Carbine
				),

				//test case - no last name
				new TestCaseObject(
					input: new List<string>
					{
						"People Assigned,,,,,,,,,,,,",
						",,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"RIFLE/CARBINE (Group B),,,,,,,,,,,,",
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
						"RIFLE/CARBINE (Group B),,,,,,,,,,,,",
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
