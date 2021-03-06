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
	public class DigestAlphaRosterTests
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

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestAlphaRoster_ExpectOnePersonsTestCases))]
		public void DigestAlphaRosterTest_ExpectOnePerson(TestCaseObject testCaseParameters)
		{
			var (input, expectedFirstName, expectedLastName, expectedSSN, expectedRank, expectedDAFSC, expectedCAFSC, expectedPAFSC, expectedHomePhone, expectedDateOnStation) = testCaseParameters;

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
			var person = insightController.GetPersonsByName(firstName: expectedFirstName, lastName: expectedLastName).Result.FirstOrDefault();

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(FileType.AlphaRoster);
				digest.Should().BeOfType<DigestAlphaRoster>();

				allPersons.Count.Should().Be(1);
				person.Should().NotBeNull();

				person.DateOnStation.Should().Be(expectedDateOnStation);
				person.SSN.Should().Be(expectedSSN);
				person.HomePhone.Should().Be(expectedHomePhone);
				//TODO test rank once it's implemented
				//person.Rank.Should().Be(expectedRank);
				person.AFSC.DAFSC.Should().Be(expectedDAFSC);
				person.AFSC.CAFSC.Should().Be(expectedCAFSC);
				person.AFSC.PAFSC.Should().Be(expectedPAFSC);
			}
		}

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestAlphaRoster_ExpectZeroPersonsTestCases))]
		public void DigestAlphaRosterTest_ExpectZeroPerson(TestCaseObject testCaseParameters)
		{
			(IList<string> input, _) = testCaseParameters;

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
				detectedFileType.Should().Be(FileType.AlphaRoster);
				digest.Should().BeOfType<DigestAlphaRoster>();

				allPersons.Count.Should().Be(0);
			}
		}

		/// <summary>
		/// Objects containing the data for the test cases
		/// </summary>
		private class TestCasesObjects
		{
			public static object[] DigestAlphaRoster_ExpectOnePersonsTestCases =
			{
				//test case - base case
				new TestCaseObject(
					new List<string>
					{
						"FULL_NAME,SSAN,GRADE,RECORD_STATUS,ASSIGNED_PAS,ASSIGNED_PAS_CLEARTEXT,OFFICE_SYMBOL,DUTY_TITLE,DUTY_START_DATE,DOR,DAFSC,CAFSC,PAFSC,2AFSC,3AFSC,4AFSC,DATE_ARRIVED_STATION,DUTY_PHONE,TAFMSD,DOE,DOS,ETS,DATE_OF_BIRTH,HOME_ADDRESS,HOME_CITY,HOME_STATE,HOME_ZIP_CODE,SUPV_NAME,GRADE_PERM_PROJ,UIF_CODE,UIF_DISPOSITION_DATE,PROJ_EVAL_CLOSE_DATE,MARITAL_STATUS,RNLTD,GAINING_PAS,GAINING_PAS_CLEARTEXT,LAST_EVAL_RATING,LAST_EVAL_CLOSE_DATE,PERF_INDICATOR,SPOUSE_SSAN,SUPV_BEGIN_DATE,REENL_ELIG_STATUS,HOME_PHONE_NUMBER,AGE,DEROS,DEPLOY_ADMIN_STATUS,DEPLOY_ADMIN_STATUS_CLEARTEXT,DEPLOY_ADMIN_STOP_DATE,DEPLOY_LEGAL_STATUS,DEPLOY_LEGAL_STATUS_CLEARTEXT,DEPLOY_LEGAL_STOP_DATE,DEPLOY_PHYS_STATUS,DEPLOY_PHYS_STATUS_CLEARTEXT,DEPLOY_PHYS_STOP_DATE,DEPLOY_TIME_STATUS,DEPLOY_TIME_STATUS_CLEARTEXT,DEPLOY_TIME_STOP_DATE,AVAILABILITY_CODE,AVAILABILITY_CODE_CLEARTEXT,AVAILABILITY_DATE,AVAILABILITY_STATUS,AVAILABILITY_STATUS_CLEARTEXT,LIMITATION_CODE,LIMITATION_CODE_CLEARTEXT,LIMITATION_END_DATE,SEC_CLR,TYPE_SEC_INV,DT_SCTY_INVES_COMPL,SEC_ELIG_DT,TECH_ID,ACDU_STATUS,ANG_ROLL_INDICATOR,AFR_SECTION_ID,CIVILIAN_ART_ID,ATTACHED_PAS,FUNCTIONAL_CATEGORY",
						"\"SMITH, JOHN\",123-45-6789,A1C,10,TE1CFKG8,552 AIR CON/NETWORKS SQ FFKG80,DOUP,SOFTWARE DEVELOPER,10-JUN-2020,10-DEC-2019,-3D034,-3D034,3D054,,,,10-JUN-2020,4057345462,10-DEC-2019,10-DEC-2019,09-DEC-2023,09-DEC-2023,07-NOV-1991,8094 SE DELINE STREET,HILLSBORO,OR,97123,WILSON THOMAS,,,,31-MAR-2023,S,,,,,,A,,10-JUN-2020,3C,5039155972,29,,,,,,,,,,,,,,,,,,,,,,V,70,08-NOV-2019,14-NOV-2019,,,,,,,A",
					},
					"John",
					"Smith",
					"123456789",
					Grade.E3,
					"-3D034",
					"-3D034",
					"3D054",
					"5039155972",
					DateTime.Parse("10-JUN-2020")
				),

				//test case - lower case AFSC input, no SSN dashes
				new TestCaseObject(
					new List<string>
					{
						"FULL_NAME,SSAN,GRADE,RECORD_STATUS,ASSIGNED_PAS,ASSIGNED_PAS_CLEARTEXT,OFFICE_SYMBOL,DUTY_TITLE,DUTY_START_DATE,DOR,DAFSC,CAFSC,PAFSC,2AFSC,3AFSC,4AFSC,DATE_ARRIVED_STATION,DUTY_PHONE,TAFMSD,DOE,DOS,ETS,DATE_OF_BIRTH,HOME_ADDRESS,HOME_CITY,HOME_STATE,HOME_ZIP_CODE,SUPV_NAME,GRADE_PERM_PROJ,UIF_CODE,UIF_DISPOSITION_DATE,PROJ_EVAL_CLOSE_DATE,MARITAL_STATUS,RNLTD,GAINING_PAS,GAINING_PAS_CLEARTEXT,LAST_EVAL_RATING,LAST_EVAL_CLOSE_DATE,PERF_INDICATOR,SPOUSE_SSAN,SUPV_BEGIN_DATE,REENL_ELIG_STATUS,HOME_PHONE_NUMBER,AGE,DEROS,DEPLOY_ADMIN_STATUS,DEPLOY_ADMIN_STATUS_CLEARTEXT,DEPLOY_ADMIN_STOP_DATE,DEPLOY_LEGAL_STATUS,DEPLOY_LEGAL_STATUS_CLEARTEXT,DEPLOY_LEGAL_STOP_DATE,DEPLOY_PHYS_STATUS,DEPLOY_PHYS_STATUS_CLEARTEXT,DEPLOY_PHYS_STOP_DATE,DEPLOY_TIME_STATUS,DEPLOY_TIME_STATUS_CLEARTEXT,DEPLOY_TIME_STOP_DATE,AVAILABILITY_CODE,AVAILABILITY_CODE_CLEARTEXT,AVAILABILITY_DATE,AVAILABILITY_STATUS,AVAILABILITY_STATUS_CLEARTEXT,LIMITATION_CODE,LIMITATION_CODE_CLEARTEXT,LIMITATION_END_DATE,SEC_CLR,TYPE_SEC_INV,DT_SCTY_INVES_COMPL,SEC_ELIG_DT,TECH_ID,ACDU_STATUS,ANG_ROLL_INDICATOR,AFR_SECTION_ID,CIVILIAN_ART_ID,ATTACHED_PAS,FUNCTIONAL_CATEGORY",
						"\"SMITH, JOHN\",123456789,A1C,10,TE1CFKG8,552 AIR CON/NETWORKS SQ FFKG80,DOUP,SOFTWARE DEVELOPER,10-JUN-2020,10-DEC-2019,-3d034,-3d034,3d054,,,,10-JUN-2020,4057345462,10-DEC-2019,10-DEC-2019,09-DEC-2023,09-DEC-2023,07-NOV-1991,8094 SE DELINE STREET,HILLSBORO,OR,97123,WILSON THOMAS,,,,31-MAR-2023,S,,,,,,A,,10-JUN-2020,3C,5039155972,29,,,,,,,,,,,,,,,,,,,,,,V,70,08-NOV-2019,14-NOV-2019,,,,,,,A",
					},
					"John",
					"Smith",
					"123456789",
					Grade.E3,
					"-3D034",
					"-3D034",
					"3D054",
					"5039155972",
					DateTime.Parse("10-JUN-2020")
				),
			};

			public static object[] DigestAlphaRoster_ExpectZeroPersonsTestCases =
			{
				//test case - no person data provided
				new TestCaseObject(
					new List<string>
						{
							"FULL_NAME,SSAN,GRADE,RECORD_STATUS,ASSIGNED_PAS,ASSIGNED_PAS_CLEARTEXT,OFFICE_SYMBOL,DUTY_TITLE,DUTY_START_DATE,DOR,DAFSC,CAFSC,PAFSC,2AFSC,3AFSC,4AFSC,DATE_ARRIVED_STATION,DUTY_PHONE,TAFMSD,DOE,DOS,ETS,DATE_OF_BIRTH,HOME_ADDRESS,HOME_CITY,HOME_STATE,HOME_ZIP_CODE,SUPV_NAME,GRADE_PERM_PROJ,UIF_CODE,UIF_DISPOSITION_DATE,PROJ_EVAL_CLOSE_DATE,MARITAL_STATUS,RNLTD,GAINING_PAS,GAINING_PAS_CLEARTEXT,LAST_EVAL_RATING,LAST_EVAL_CLOSE_DATE,PERF_INDICATOR,SPOUSE_SSAN,SUPV_BEGIN_DATE,REENL_ELIG_STATUS,HOME_PHONE_NUMBER,AGE,DEROS,DEPLOY_ADMIN_STATUS,DEPLOY_ADMIN_STATUS_CLEARTEXT,DEPLOY_ADMIN_STOP_DATE,DEPLOY_LEGAL_STATUS,DEPLOY_LEGAL_STATUS_CLEARTEXT,DEPLOY_LEGAL_STOP_DATE,DEPLOY_PHYS_STATUS,DEPLOY_PHYS_STATUS_CLEARTEXT,DEPLOY_PHYS_STOP_DATE,DEPLOY_TIME_STATUS,DEPLOY_TIME_STATUS_CLEARTEXT,DEPLOY_TIME_STOP_DATE,AVAILABILITY_CODE,AVAILABILITY_CODE_CLEARTEXT,AVAILABILITY_DATE,AVAILABILITY_STATUS,AVAILABILITY_STATUS_CLEARTEXT,LIMITATION_CODE,LIMITATION_CODE_CLEARTEXT,LIMITATION_END_DATE,SEC_CLR,TYPE_SEC_INV,DT_SCTY_INVES_COMPL,SEC_ELIG_DT,TECH_ID,ACDU_STATUS,ANG_ROLL_INDICATOR,AFR_SECTION_ID,CIVILIAN_ART_ID,ATTACHED_PAS,FUNCTIONAL_CATEGORY",
						},
					"" // throwaway second paremeter since you can't deconstruct to a single object
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
			private readonly string _expectedSSN;
			private readonly Grade _expectedGrade;
			private readonly string _expectedDAFSC;
			private readonly string _expectedCAFSC;
			private readonly string _expectedPAFSC;
			private readonly string _expectedPhone;
			private readonly DateTime _expectedDateOnStation;

			public TestCaseObject(IList<string> input, string expectedFirstName, string expectedLastName, string expectedSSN, Grade expectedGrade, string expectedDAFSC, string expectedCAFSC, string expectedPAFSC, string expectedPhone, DateTime expectedDateOnStation)
			{
				_input = input;
				_expectedFirstName = expectedFirstName;
				_expectedLastName = expectedLastName;
				_expectedSSN = expectedSSN;
				_expectedGrade = expectedGrade;
				_expectedDAFSC = expectedDAFSC;
				_expectedCAFSC = expectedCAFSC;
				_expectedPAFSC = expectedPAFSC;
				_expectedPhone = expectedPhone;
				_expectedDateOnStation = expectedDateOnStation;
			}

			public void Deconstruct(out IList<string> input, out string expectedFirstName, out string expectedLastName, out string expectedSSN, out Grade expectedGrade, out string expectedDAFSC, out string expectedCAFSC, out string expectedPAFSC, out string expectedPhone, out DateTime expectedDateOnStation)
			{
				input = _input;
				expectedFirstName = _expectedFirstName;
				expectedLastName = _expectedLastName;
				expectedSSN = _expectedSSN;
				expectedGrade = _expectedGrade;
				expectedDAFSC = _expectedDAFSC;
				expectedCAFSC = _expectedCAFSC;
				expectedPAFSC = _expectedPAFSC;
				expectedPhone = _expectedPhone;
				expectedDateOnStation = _expectedDateOnStation;
			}

			public TestCaseObject(IList<string> input, string throwaway)
			{
				_input = input;
				_ = throwaway;
			}

			public void Deconstruct(out IList<string> input, out string throwaway)
			{
				input = _input;
				throwaway = "";
			}
		}
	}
}
