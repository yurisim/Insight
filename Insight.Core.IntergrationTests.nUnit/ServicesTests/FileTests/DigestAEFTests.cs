using System.Collections.Generic;
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
	public class DigestAEFTests
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

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestAEF_ExpectOnePersonsTestCases))]
		public void DigestAEFTest_ExpectOnePerson(TestCaseObject testCaseParameters)
		{
			var (input, expectedFirstName, expectedLastName, expectedTrainingOverallStatus, expectedMedicalOverallStatus, expectedPersonnelOverallStatus) = testCaseParameters;

			//arrange
			FileType detectedFileType = Detector.DetectFileType(input);

			IDigest digest = DigestFactory.GetDigestor(detectedFileType, input, dbContextOptions);

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

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(FileType.AEF);
				digest.Should().BeOfType<DigestAEF>();

				allPersons.Count.Should().Be(1);

				person.Should().NotBeNull();

				person.Training.OverallStatus.Should().Be(expectedTrainingOverallStatus);
				person.Medical.OverallStatus.Should().Be(expectedMedicalOverallStatus);
				person.Personnel.OverallStatus.Should().Be(expectedPersonnelOverallStatus);

			}
		}

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestAEF_ExpectZeroPersonsTestCases))]
		public void DigestAEFTest_ExpectZeroPerson(TestCaseObject testCaseParameters)
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

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(FileType.AEF);
				digest.Should().BeOfType<DigestAEF>();

				allPersons.Count.Should().Be(0);
			}
		}

		/// <summary>
		/// Objects containing the data for the test cases
		/// </summary>
		private class TestCasesObjects
		{
			public static object[] DigestAEF_ExpectOnePersonsTestCases =
			{
				//test case - base case
				new TestCaseObject(
					input: new List<string>
					{
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",
						"Export Description:  UnitRoster,,,,,,,,,,,,,,,,",
						"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip",
						"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,G,G,R,Y,Y,Member Started,",
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",

					},
					expectedFirstName: "John",
					expectedLastName: "Smith",
					expectedPersonnelOverallStatus : Status.Current,
					expectedMedicalOverallStatus : Status.Current,
					expectedTrainingOverallStatus : Status.Overdue
				),

				//test case - no header
				new TestCaseObject(
					input: new List<string>
					{
						"Export Description:  UnitRoster,,,,,,,,,,,,,,,,",
						"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip",
						"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,G,G,R,Y,Y,Member Started,",

					},
					expectedFirstName: "John",
					expectedLastName: "Smith",
					expectedPersonnelOverallStatus : Status.Current,
					expectedMedicalOverallStatus : Status.Current,
					expectedTrainingOverallStatus : Status.Overdue
				),

				//test case - different statuses
				new TestCaseObject(
					input: new List<string>
					{
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",
						"Export Description:  UnitRoster,,,,,,,,,,,,,,,,",
						"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip",
						"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,R,Y,G,Y,Y,Member Started,",
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",

					},
					expectedFirstName: "John",
					expectedLastName: "Smith",
					expectedPersonnelOverallStatus : Status.Overdue,
					expectedMedicalOverallStatus : Status.Upcoming,
					expectedTrainingOverallStatus : Status.Current
				),

				//test case - base case
				new TestCaseObject(
					input: new List<string>
					{
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",
						"Export Description:  UnitRoster,,,,,,,,,,,,,,,,",
						"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip",
						"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,,,,Y,Y,Member Started,",
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",

					},
					expectedFirstName: "John",
					expectedLastName: "Smith",
					expectedPersonnelOverallStatus : Status.Unknown,
					expectedMedicalOverallStatus : Status.Unknown,
					expectedTrainingOverallStatus : Status.Unknown
				),

			};

			public static object[] DigestAEF_ExpectZeroPersonsTestCases =
			{
				//test case - no data rows
				new TestCaseObject(
					input: new List<string>
					{
						"\"1The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",
						"Export Description:  UnitRoster,,,,,,,,,,,,,,,,",
						"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip",
						"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,,,,Y,Y,Member Started,",
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",
					},
					"" //throwaway parameter since you can't deconstruct to a single parameter
				),

				//test case - no person that exists in database
				new TestCaseObject(
					input: new List<string>
					{
						"\"2The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",
						"Export Description:  UnitRoster,,,,,,,,,,,,,,,,",
						"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip",
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",
					},
					"" //throwaway parameter since you can't deconstruct to a single parameter
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

			Status _expectedTrainingOverallStatus { get; set; }

			Status _expectedMedicalOverallStatus { get; set; }

			Status _expectedPersonnelOverallStatus { get; set; }

			public TestCaseObject(IList<string> input, string expectedFirstName, string expectedLastName, Status expectedTrainingOverallStatus, Status expectedMedicalOverallStatus, Status expectedPersonnelOverallStatus)
			{
				_input = input;
				_expectedFirstName = expectedFirstName;
				_expectedLastName = expectedLastName;
				_expectedTrainingOverallStatus = expectedTrainingOverallStatus;
				_expectedMedicalOverallStatus = expectedMedicalOverallStatus;
				_expectedPersonnelOverallStatus = expectedPersonnelOverallStatus;
			}

			public void Deconstruct(out IList<string> input, out string expectedFirstName, out string expectedLastName, out Status expectedTrainingOverallStatus, out Status expectedMedicalOverallStatus, out Status expectedPersonnelOverallStatus)
			{
				input = _input;
				expectedFirstName = _expectedFirstName;
				expectedLastName = _expectedLastName;
				expectedTrainingOverallStatus = _expectedTrainingOverallStatus;
				expectedMedicalOverallStatus = _expectedMedicalOverallStatus;
				expectedPersonnelOverallStatus = _expectedPersonnelOverallStatus;
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
