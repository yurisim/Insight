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
	public class DigestPEXTests
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

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestPEX_ExpectOnePersonsTestCases))]
		//public void DigestPEXTest_ExpectOnePerson(TestCaseObject testCaseParameters)
		//{
		//	var (input, expectedFirstName, expectedLastName, expectedFlight) = testCaseParameters;

		//	//arrange
		//	FileType detectedFileType = Detector.DetectFileType(input);

		//	IDigest digest = DigestFactory.GetDigestor(detectedFileType, input, dbContextOptions);

		//	//creates person entity in DB so there's someone to look up
		//	Person personToCreateInDB = new Person()
		//	{
		//		FirstName = expectedFirstName,
		//		LastName = expectedLastName,
		//	};
		//	insightController.Add(personToCreateInDB);

		//	//act
		//	digest.CleanInput();
		//	digest.DigestLines();

		//	//arrange 2.0
		//	var allPersons = insightController.GetAllPersons().Result;
		//	var person = insightController.GetPersonByName(firstName: expectedFirstName, lastName: expectedLastName).Result;

		//	//assert
		//	using (new AssertionScope())
		//	{
		//		detectedFileType.Should().Be(FileType.PEX);
		//		digest.Should().BeOfType<DigestPEX>();

		//		allPersons.Count.Should().Be(1);
		//		person.Should().NotBeNull();

		//		person.FirstName.Should().Be(expectedFirstName.Trim().ToUpperInvariant());
		//		person.LastName.Should().Be(expectedLastName.Trim().ToUpperInvariant());
		//		person.Flight.Equals(expectedFlight);
		//	}
		//}

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestPEX_ExpectZeroPersonsTestCases))]
		public void DigestPEXTest_ExpectZeroPerson(TestCaseObject testCaseParameters)
		{
			var (input, _) = testCaseParameters;

			//arrange
			FileType detectedFileType = Detector.DetectFileType(input);

			IDigest digest = DigestFactory.GetDigestor(detectedFileType, input, dbContextOptions);

			//act
			digest.CleanInput();
			digest.DigestLines();

			//arrange 2.0
			var allPersons = insightController.GetAllPersons().Result;

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(FileType.PEX);
				digest.Should().BeOfType<DigestPEX>();

				allPersons.Count.Should().Be(0);
			}
		}

		/// <summary>
		/// Objects containing the data for the test cases
		/// </summary>
		private class TestCasesObjects
		{
			public static object[] DigestPEX_ExpectOnePersonsTestCases =
			{
				//test case - base case
				new TestCaseObject(
					input: new List<string>
					{
						"ShortName,PEX Designation",
						"SmithJ,A",
					},
					expectedFirstName: "John",
					expectedLastName: "Smith",
					expectedFlight: "A"
				),

				//test case - no flight provided
				new TestCaseObject(
					input: new List<string>
					{
						"ShortName,PEX Designation",
						"SmithJ,",
					},
					expectedFirstName: "John",
					expectedLastName: "Smith",
					expectedFlight: ""
				),

				//test case - no flight provided
				new TestCaseObject(
					input: new List<string>
					{
						"ShortName,PEX Designation",
						"ChurchillO,B",
					},
					expectedFirstName: "Olivia",
					expectedLastName: "Churchill",
					expectedFlight: "B"
				),
			};

			public static object[] DigestPEX_ExpectZeroPersonsTestCases =
			{
				//test case - no flight provided
				new TestCaseObject(
					input: new List<string>
					{
						"ShortName,PEX Designation",
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
			IList<string> _input { get; set; }

			string _expectedFirstName { get; set; }

			string _expectedLastName { get; set; }

			string _expectedFlight { get; set; }


			public TestCaseObject(IList<string> input, string expectedFirstName, string expectedLastName, string expectedFlight)
			{
				_input = input;
				_expectedFirstName = expectedFirstName;
				_expectedLastName = expectedLastName;
				_expectedFlight = expectedFlight;
			}

			public void Deconstruct(out IList<string> input, out string expectedFirstName, out string expectedLastName, out string expectedFlight)
			{
				input = _input;
				expectedFirstName = _expectedFirstName;
				expectedLastName = _expectedLastName;
				expectedFlight = _expectedFlight;
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
