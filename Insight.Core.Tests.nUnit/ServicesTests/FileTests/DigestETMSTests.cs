﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Insight.Core.Models;
using Insight.Core.Services;
using Insight.Core.Services.Database;
using Insight.Core.Services.File;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Insight.Core.Tests.nUnit.ServicesTests.FileTests
{
	[TestFixture]
	public class DigestETMSTests
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

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestETMS_ExpectOnePersonsTestCases))]
		public void DigestETMSTest_ExpectOnePerson(TestCaseObject testCaseParameters)
		{
			var (input, expectedFirstName, expectedLastName, expectedCourseName, expectedCourseCompletion) = testCaseParameters;

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
			var allPersons = insightController.GetAll<Person>().Result;
			var person = insightController.GetPersonByName(firstName: expectedFirstName, lastName: expectedLastName).Result;
			var course = insightController.GetCourseByName(courseName: expectedCourseName);
			CourseInstance courseInstanceToCheck = new CourseInstance()
			{
				Person = person,
				Course = course,
				Completion = expectedCourseCompletion,
				Expiration = expectedCourseCompletion.AddDays(course.Interval * 365)
			};
			CourseInstance courseInstanceFromDB = insightController.GetCourseInstance(courseInstanceToCheck).Result;


			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(FileType.ETMS);
				digest.Should().BeOfType<DigestETMS>();

				allPersons.Count.Should().Be(1);

				person.Should().NotBeNull();
				course.Should().NotBeNull();
				courseInstanceFromDB.Should().NotBeNull();
			}
		}

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestETMS_ExpectZeroCourseInstancesTestCases))]
		public void DigestETMSTest_ExpectZeroCourseInstances(TestCaseObject testCaseParameters)
		{
			var (input, expectedFirstName, expectedLastName, expectedCourseName) = testCaseParameters;

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
			var allPersons = insightController.GetAll<Person>().Result;
			var person = insightController.GetPersonByName(firstName: expectedFirstName, lastName: expectedLastName).Result;
			var courses = insightController.GetAll<Course>().Result;
			var course = insightController.GetCourseByName(expectedCourseName);


			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(FileType.ETMS);
				digest.Should().BeOfType<DigestETMS>();

				allPersons.Count.Should().Be(1);

				//if a course was created, there should only be one and no releated courseInstances
				if(course != null)
				{
					courses.Count.Should().Be(1);
					person.CourseInstances.Count.Should().Be(0);
					course.CourseInstances.Count.Should().Be(0);
				}
				//else, no couse was created
				else
				{
					courses.Count.Should().Be(0);
				}
			}
		}

		/// <summary>
		/// Objects containing the data for the test cases
		/// </summary>
		private class TestCasesObjects
		{
			public static object[] DigestETMS_ExpectOnePersonsTestCases =
			{
				//test case - base case
				new TestCaseObject(
					input: new List<string>
					{
						"PAS Description,Course Title,Last Name,First Name,Completion Date",
						"960 AACS,TFAT - Cyber Awareness Challenge,ALSOP,SOPHIE,4/23/2021"
					},
					expectedFirstName: "SOPHIE",
					expectedLastName: "ALSOP",
					expectedCourseName: "TFAT - Cyber Awareness Challenge",
					expectedCourseCompletion: DateTime.Parse("4/23/2021")
				),

				//test case - base case
				new TestCaseObject(
					input: new List<string>
					{
						"PAS Description,Course Title,Last Name,First Name,Completion Date",
						"960 AACS,TFAT - Cyber Awareness Challenge,ALSOP,SOPHIE,4/23/2021",
						"960 AACS,TFAT - Cyber Awareness Challenge,ALSOP,SOPHIE,"
					},
					expectedFirstName: "SOPHIE",
					expectedLastName: "ALSOP",
					expectedCourseName: "TFAT - Cyber Awareness Challenge",
					expectedCourseCompletion: DateTime.Parse("4/23/2021")
				),

				//test case - base case
				new TestCaseObject(
					input: new List<string>
					{
						"PAS Description,Course Title,Last Name,First Name,Completion Date",
						"960 AACS,Law of War (LoW) - Basic,ALSOP,SOPHIE,",
						"960 AACS,Law of War (LoW) - Basic,ALSOP,SOPHIE,3/5/2020"
					},
					expectedFirstName: "SOPHIE",
					expectedLastName: "ALSOP",
					expectedCourseName: "Law of War (LoW) - Basic",
					expectedCourseCompletion: DateTime.Parse("3/5/2020")
				),
			};

			public static object[] DigestETMS_ExpectZeroCourseInstancesTestCases =
			{
				//test case - base case
				new TestCaseObject(
					input: new List<string>
					{
						"PAS Description,Course Title,Last Name,First Name,Completion Date",
						"960 AACS,Law of War (LoW) - Basic,ALSOP,SOPHIE,",
					},
					"SOPHIE",
					"ALSOP",
					"Law of War (LoW) - Basic"
				),

				////test case - base case
				//new TestCaseObject(
				//	input: new List<string>
				//	{
				//		"PAS Description,Course Title,Last Name,First Name,Completion Date",
				//		"960 AACS,Law of War (LoW) - Basic,thisperson,doesnotexist,",
				//	},
				//	"" //throw away parameter
				//),

			};

			public static object[] DigestETMS_InvalidCourse =
			{
				//test case - base case
				new TestCaseObject(
					input: new List<string>
					{
						"PAS Description,Course Title,Last Name,First Name,Completion Date",
						"960 AACS,,ALSOP,SOPHIE,3/5/2020",
					},
					"" //throw away parameter
				),
			};
		}

		/// <summary>
		/// Container outline for all of the required input and expected outputs for the tests
		/// </summary>
		public class TestCaseObject
		{
			IList<string> _input { get; set; }

			string _expectedFirstName { get; set; }

			string _expectedLastName { get; set; }

			string _expectedCourseName { get; set; }

			DateTime _expectedCourseCompletion { get; set; }

			public TestCaseObject(IList<string> input, string expectedFirstName, string expectedLastName, string expectedCourseName, DateTime expectedCourseCompletion)
			{
				_input = input;
				_expectedFirstName = expectedFirstName;
				_expectedLastName = expectedLastName;
				_expectedCourseName = expectedCourseName;
				_expectedCourseCompletion = expectedCourseCompletion;
			}

			public void Deconstruct(out IList<string> input, out string expectedFirstName, out string expectedLastName, out string expectedCourseName, out DateTime expectedCourseCompletion)
			{
				input = _input;
				expectedFirstName = _expectedFirstName;
				expectedLastName = _expectedLastName;
				expectedCourseName = _expectedCourseName;
				expectedCourseCompletion = _expectedCourseCompletion;
			}

			public TestCaseObject(IList<string> input, string expectedFirstName, string expectedLastName, string expectedCourseName)
			{
				_input = input;
				_expectedFirstName = expectedFirstName;
				_expectedLastName = expectedLastName;
				_expectedCourseName = expectedCourseName;
			}

			public void Deconstruct(out IList<string> input, out string expectedFirstName, out string expectedLastName, out string expectedCourseName)
			{
				input = _input;
				expectedFirstName = _expectedFirstName;
				expectedLastName = _expectedLastName;
				expectedCourseName = _expectedCourseName;
			}

			public TestCaseObject(IList<string> input, string throwaway)
			{
				_input = input;
			}

			public void Deconstruct(out IList<string> input, out string throwaway)
			{
				input = _input;
				throwaway = "";
			}
		}
	}
}
