using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Insight.Core.Models;
using Insight.Core.Services;
using Insight.Core.Services.Database;
using Insight.Core.Services.File;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Insight.Core.IntegrationTests.nUnit.ServicesTests.FileTests
{
	[TestFixture]
	public class DigestSFMISTests
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

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestSFMIS_ExpectOnePersons_TestCases))]
		public void DigestSFMISTest_ExpectOnePerson(TestCaseObject testCaseParameters)
		{
			var (input, expectedFirstName, expectedLastName, strm4CourseCompletionExpected, strm9CourseCompletionExpected, expectedEmail) = testCaseParameters;

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

			bool m4CourseExpected = !string.IsNullOrEmpty(strm4CourseCompletionExpected);
			bool m9CourseExpected = !string.IsNullOrEmpty(strm9CourseCompletionExpected);

			//act
			digest.CleanInput();
			digest.DigestLines();

			//arrange 2.0
			var allPersons = insightController.GetAllPersons().Result;
			var person = insightController.GetPersonsByName(firstName: expectedFirstName, lastName: expectedLastName).Result.FirstOrDefault();
			var m4Couse = insightController.GetCoursesByName(WeaponCourseTypes.Rifle_Carbine).Result.FirstOrDefault();
			var m9Couse = insightController.GetCoursesByName(WeaponCourseTypes.Handgun).Result.FirstOrDefault();

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(FileType.SFMIS);
				digest.Should().BeOfType<DigestSFMIS>();

				allPersons.Count.Should().Be(1);

				person.Should().NotBeNull();
				person.Email.Should().Be(expectedEmail);

				//hanldes testing if a m4 course is expected
				if (m4CourseExpected)
				{
					DateTime m4CourseCompletionExpected = DateTime.Parse(strm4CourseCompletionExpected);
					CourseInstance courseInstanceToCheck = new CourseInstance()
					{
						Person = person,
						Course = m4Couse,
						Completion = m4CourseCompletionExpected,
						Expiration = m4CourseCompletionExpected.AddDays(m4Couse.Interval * 365)
					};
					CourseInstance courseInstanceFromDB = insightController.GetCourseInstances(courseInstanceToCheck).Result.FirstOrDefault();

					courseInstanceFromDB.Should().NotBeNull();
					m4Couse.Should().NotBeNull();
					m4Couse.CourseInstances.Count.Should().Be(1);
					person.CourseInstances.Count.Should().BeInRange(1, 2);

				}

				//hanldes testing if a m4 course is expected
				if (m9CourseExpected)
				{
					DateTime m9CourseCompletionExpected = DateTime.Parse(strm9CourseCompletionExpected);
					CourseInstance courseInstanceToCheck = new CourseInstance()
					{
						Person = person,
						Course = m9Couse,
						Completion = m9CourseCompletionExpected,
						Expiration = m9CourseCompletionExpected.AddDays(m9Couse.Interval * 365)
					};
					CourseInstance courseInstanceFromDB = insightController.GetCourseInstances(courseInstanceToCheck).Result.FirstOrDefault();

					courseInstanceFromDB.Should().NotBeNull();
					m9Couse.Should().NotBeNull();
					m9Couse.CourseInstances.Count.Should().Be(1);
					person.CourseInstances.Count.Should().BeInRange(1, 2);
				}

				//if exactly one course is expected, course count should be exactly one (exclusive or)
				if (m4CourseExpected ^ m9CourseExpected)
				{
					person.CourseInstances.Count.Should().Be(1);
				}

				//if exactly two courses are expected, course count should be exactly two
				if (m4CourseExpected && m9CourseExpected)
				{
					person.CourseInstances.Count.Should().Be(2);
				}
			}
		}


		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestSFMIS_ExpectOnePerson_ZeroCourseInstances_TestCases))]
		public void DigestSFMIS_ExpectOnePerson_ZeroCourseInstances(TestCaseObject testCaseParameters)
		{
			var (input, expectedFirstName, expectedLastName, expectedEmail) = testCaseParameters;

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
			var allCourses = insightController.GetAll<Course>().Result;

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(FileType.SFMIS);
				digest.Should().BeOfType<DigestSFMIS>();

				allPersons.Count.Should().Be(1);

				person.Should().NotBeNull();
				person.Email.Should().Be(expectedEmail);

				allCourses.Should().HaveCount(0);
			}
		}


		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestSFMIS_ExpectZeroPersons_TestCases))]
		public void DigestSFMISTest_ExpectZeroPerson(TestCaseObject testCaseParameters)
		{
			var (input, _) = testCaseParameters;

			//arrange
			FileType detectedFileType = Detector.DetectFileType(input);

			IDigest digest = DigestFactory.GetDigestor(detectedFileType, input, dbContextOptions);

			//act
			digest.CleanInput();
			digest.DigestLines();

			//arrange 2.0
			var allPersons = insightController.GetAll<Person>().Result;
			var allCourses = insightController.GetAll<Course>().Result;
			var allCourseInstances = insightController.GetAll<CourseInstance>().Result;

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(FileType.SFMIS);
				digest.Should().BeOfType<DigestSFMIS>();

				allPersons.Count.Should().Be(0);
				allCourses.Count.Should().Be(0);
				allCourseInstances.Count.Should().Be(0);
			}
		}

		/// <summary>
		/// Objects containing the data for the test cases
		/// </summary>
		private class TestCasesObjects
		{
			public static object[] DigestSFMIS_ExpectOnePersons_TestCases =
			{
				//test case - base case - m9, header
				new TestCaseObject(
					input: new List<string>
					{
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,,,,,",
						"Export Description:  SFMISRoster,,,,,,,,,,,,,,,,,,,,",
						"Name,Unit,PasCode,AEFI,AFSC,PayGrade,Email,Email4Career,Gender,DutyStatus,SecurityClearance,Security_Status,Security_Expire,TrainingNeeds,Course,Weapon_Model,Course_ID,Completion_Date,Expire_Date,Qualification,Arming_Group",
						"ALSOP SOPHIE JANE,960 AIRBORNE AIR CTRL SQ (FFDFP0),TE1CFDFP,YR,013B3B,E2,sophie.alsop@us.af.mil,sophie.alsop@us.af.mil,F,TDY CONTGENCY,SCI(DCID 1/14 ELIGIBLE) - expires 24Jan24,expires,24-JAN-24,,M9 HG AFQC (INITIAL/RECURRING),M9,219882,2021-04-26,2022-04-30,QUALIFIED,",
					},
					expectedFirstName : "Sophie",
					expectedLastName: "Alsop",
					m4CourseCompletionExpected: "",
					m9CourseCompletionExpected: "2021-04-26",
					expectedEmail: "sophie.alsop@us.af.mil"
				),

				//test case - m9, no header
				new TestCaseObject(
					input: new List<string>
					{
						"Export Description:  SFMISRoster,,,,,,,,,,,,,,,,,,,,",
						"Name,Unit,PasCode,AEFI,AFSC,PayGrade,Email,Email4Career,Gender,DutyStatus,SecurityClearance,Security_Status,Security_Expire,TrainingNeeds,Course,Weapon_Model,Course_ID,Completion_Date,Expire_Date,Qualification,Arming_Group",
						"ALSOP SOPHIE JANE,960 AIRBORNE AIR CTRL SQ (FFDFP0),TE1CFDFP,YR,013B3B,E2,sophie.alsop@us.af.mil,sophie.alsop@us.af.mil,F,TDY CONTGENCY,SCI(DCID 1/14 ELIGIBLE) - expires 24Jan24,expires,24-JAN-24,,M4 RIFLE/CARBINE GROUP C AFQC,M4,219882,2021-04-26,2022-04-30,QUALIFIED,",

					},
					expectedFirstName : "Sophie",
					expectedLastName: "Alsop",
					m4CourseCompletionExpected: "2021-04-26",
					m9CourseCompletionExpected: "",
					expectedEmail : "sophie.alsop@us.af.mil"
				),

				//test case - m4
				new TestCaseObject(
					input: new List<string>
					{
						"Export Description:  SFMISRoster,,,,,,,,,,,,,,,,,,,,",
						"Name,Unit,PasCode,AEFI,AFSC,PayGrade,Email,Email4Career,Gender,DutyStatus,SecurityClearance,Security_Status,Security_Expire,TrainingNeeds,Course,Weapon_Model,Course_ID,Completion_Date,Expire_Date,Qualification,Arming_Group",
						"ALSOP SOPHIE JANE,960 AIRBORNE AIR CTRL SQ (FFDFP0),TE1CFDFP,YR,013B3B,E2,sophie.alsop@us.af.mil,sophie.alsop@us.af.mil,F,TDY CONTGENCY,SCI(DCID 1/14 ELIGIBLE) - expires 24Jan24,expires,24-JAN-24,,M4 RIFLE/CARBINE GROUP C AFQC,M4,219882,2021-04-26,2022-04-30,QUALIFIED,",

					},
					expectedFirstName : "Sophie",
					expectedLastName: "Alsop",
					m4CourseCompletionExpected: "2021-04-26",
					m9CourseCompletionExpected: "",
					expectedEmail : "sophie.alsop@us.af.mil"
				),

				//test case - both weapons
				new TestCaseObject(
					input: new List<string>
					{
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,,,,,",
						"Export Description:  SFMISRoster,,,,,,,,,,,,,,,,,,,,",
						"Name,Unit,PasCode,AEFI,AFSC,PayGrade,Email,Email4Career,Gender,DutyStatus,SecurityClearance,Security_Status,Security_Expire,TrainingNeeds,Course,Weapon_Model,Course_ID,Completion_Date,Expire_Date,Qualification,Arming_Group",
						"CHAPMAN NEIL SMITH,960 AIRBORNE AIR CTRL SQ (FFDFP0),TE1CFDFP,YR,1A331I,E2,neil.chapman.2@us.af.mil,neil.chapman.2@us.af.mil,M,TDY CONTGENCY,SCI(DCID 1/14 ELIGIBLE) - expires 09Mar25,expires,09-MAR-25,,M4 RIFLE/CARBINE GROUP C AFQC,M4,207876,2019-09-09,2020-09-30,EXPERT,",
						"CHAPMAN NEIL SMITH,960 AIRBORNE AIR CTRL SQ (FFDFP0),TE1CFDFP,YR,1A331I,E2,neil.chapman.2@us.af.mil,neil.chapman.2@us.af.mil,M,TDY CONTGENCY,SCI(DCID 1/14 ELIGIBLE) - expires 09Mar25,expires,09-MAR-25,,M9 HG AFQC (INITIAL/RECURRING),M9,341798,2021-04-01,2022-04-30,EXPERT,",

					},
					expectedFirstName : "NEIL",
					expectedLastName: "CHAPMAN",
					m4CourseCompletionExpected: "2019-09-09",
					m9CourseCompletionExpected: "2021-04-01",
					expectedEmail : "neil.chapman.2@us.af.mil"
				),
			};

			public static object[] DigestSFMIS_ExpectOnePerson_ZeroCourseInstances_TestCases =
			{
				//test case - only email
				new TestCaseObject(
					input: new List<string>
					{
						"Export Description:  SFMISRoster",
						"Email4Career",
						"sophie.alsop@us.af.mil",
					},
					expectedFirstName : "Sophie",
					expectedLastName: "Alsop",
					expectedEmail: "sophie.alsop@us.af.mil"
				),
			};

			public static object[] DigestSFMIS_ExpectZeroPersons_TestCases =
			{
				//test case - no email
				new TestCaseObject(
					input: new List<string>
					{
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,,,,,",
						"Export Description:  SFMISRoster,,,,,,,,,,,,,,,,,,,,",
						"Name,Unit,PasCode,AEFI,AFSC,PayGrade,Email,Email4Career,Gender,DutyStatus,SecurityClearance,Security_Status,Security_Expire,TrainingNeeds,Course,Weapon_Model,Course_ID,Completion_Date,Expire_Date,Qualification,Arming_Group",
						"ALSOP SOPHIE JANE,960 AIRBORNE AIR CTRL SQ (FFDFP0),TE1CFDFP,YR,013B3B,E2,,,F,TDY CONTGENCY,SCI(DCID 1/14 ELIGIBLE) - expires 24Jan24,expires,24-JAN-24,,M9 HG AFQC (INITIAL/RECURRING),M9,219882,2021-04-26,2022-04-30,QUALIFIED,",
					},
					""
				),

				//test case - no email
				new TestCaseObject(
					input: new List<string>
					{
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,,,,,",
						"Export Description:  SFMISRoster,,,,,,,,,,,,,,,,,,,,",
						"Name,Unit,PasCode,AEFI,AFSC,PayGrade,Email,Email4Career,Gender,DutyStatus,SecurityClearance,Security_Status,Security_Expire,TrainingNeeds,Course,Weapon_Model,Course_ID,Completion_Date,Expire_Date,Qualification,Arming_Group",
						"ALSOP SOPHIE JANE,960 AIRBORNE AIR CTRL SQ (FFDFP0),TE1CFDFP,YR,013B3B,E2,email@dne.com,email@dne.com,F,TDY CONTGENCY,SCI(DCID 1/14 ELIGIBLE) - expires 24Jan24,expires,24-JAN-24,,M9 HG AFQC (INITIAL/RECURRING),M9,219882,2021-04-26,2022-04-30,QUALIFIED,",
					},
					""
				),

				//test case - no email
				new TestCaseObject(
					input: new List<string>
					{
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,,,,,",
						"Export Description:  SFMISRoster,,,,,,,,,,,,,,,,,,,,",
						"Name,Unit,PasCode,AEFI,AFSC,PayGrade,Email,Email4Career,Gender,DutyStatus,SecurityClearance,Security_Status,Security_Expire,TrainingNeeds,Course,Weapon_Model,Course_ID,Completion_Date,Expire_Date,Qualification,Arming_Group",
						"ALSOP SOPHIE JANE,960 AIRBORNE AIR CTRL SQ (FFDFP0),TE1CFDFP,YR,013B3B,E2,garbage,garbage,F,TDY CONTGENCY,SCI(DCID 1/14 ELIGIBLE) - expires 24Jan24,expires,24-JAN-24,,M9 HG AFQC (INITIAL/RECURRING),M9,219882,2021-04-26,2022-04-30,QUALIFIED,",
					},
					""
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
			private readonly string _m4CourseCompletionExpected;
			private readonly string _m9CourseCompletionExpected;
			private readonly string _expectedEmail;

			public TestCaseObject(IList<string> input, string expectedFirstName, string expectedLastName, string m4CourseCompletionExpected, string m9CourseCompletionExpected, string expectedEmail)
			{
				_input = input;
				_expectedFirstName = expectedFirstName;
				_expectedLastName = expectedLastName;
				_m4CourseCompletionExpected = m4CourseCompletionExpected;
				_m9CourseCompletionExpected = m9CourseCompletionExpected;
				_expectedEmail = expectedEmail;
			}

			public void Deconstruct(out IList<string> input, out string expectedFirstName, out string expectedLastName, out string m4CourseCompletionExpected, out string m9CourseCompletionExpected, out string email)
			{
				input = _input;
				expectedFirstName = _expectedFirstName;
				expectedLastName = _expectedLastName;
				m4CourseCompletionExpected = _m4CourseCompletionExpected;
				m9CourseCompletionExpected = _m9CourseCompletionExpected;
				email = _expectedEmail;
			}


			public TestCaseObject(IList<string> input, string expectedFirstName, string expectedLastName, string expectedEmail)
			{
				_input = input;
				_expectedFirstName = expectedFirstName;
				_expectedLastName = expectedLastName;
				_expectedEmail = expectedEmail;
			}

			public void Deconstruct(out IList<string> input, out string expectedFirstName, out string expectedLastName, out string email)
			{
				input = _input;
				expectedFirstName = _expectedFirstName;
				expectedLastName = _expectedLastName;
				email = _expectedEmail;
			}

			public TestCaseObject(IList<string> input, string expectedEmail)
			{
				_input = input;
				_expectedEmail = expectedEmail;
			}

			public void Deconstruct(out IList<string> input, out string email)
			{
				input = _input;
				email = _expectedEmail;
			}
		}
	}
}
