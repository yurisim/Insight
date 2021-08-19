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
	public class DigestLOXTests 
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

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestLOX_ExpectOnePersonsTestCases))]
		public void DigestLOXTest_ExpectOnePerson(TestCaseObject testCaseParameters)
		{
			var (input, expectedFirstName, expectedLastName, expectedFlight, expectedOrg, expectedRank, expectedCrewPosition) = testCaseParameters;

			//arrange
			FileType detectedFileType = Detector.DetectFileType(input);

			IDigest digest = DigestFactory.GetDigestor(detectedFileType, input, dbContextOptions);

			//act
			digest.CleanInput();
			digest.DigestLines();

			//arrange 2.0
			var allPersons = insightController.GetAllPersons().Result;
			var person = insightController.GetPersonByName(firstName: expectedFirstName, lastName: expectedLastName).Result;
			var org = insightController.GetOrgByAlias(expectedOrg);

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(FileType.LOX);
				digest.Should().BeOfType<DigestLOX>();

				allPersons.Count.Should().Be(1);

				person.Should().NotBeNull();

				person.FirstName.Should().Be(expectedFirstName.Trim().ToUpperInvariant());
				person.LastName.Should().Be(expectedLastName.Trim().ToUpperInvariant());
				person.Organization.Name.Should().Be(org.Name);
				person.Flight.Equals(expectedFlight);
				person.CrewPosition.Should().Be(expectedCrewPosition);
				//person.Rank.Should().Be(expectedRank);
			}
		}

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.DigestLOX_ExpectZeroPersonsTestCases))]
		public void DigestLOXTest_ExpectZeroPerson(TestCaseObject testCaseParameters)
		{
			var (input, expectedOrg) = testCaseParameters;

			//arrange
			FileType detectedFileType = Detector.DetectFileType(input);

			IDigest digest = DigestFactory.GetDigestor(detectedFileType, input, dbContextOptions);

			//act
			digest.CleanInput();
			digest.DigestLines();

			//arrange 2.0
			var allPersons = insightController.GetAllPersons().Result;
			var org = insightController.GetOrgByAlias(expectedOrg);

			//assert
			using (new AssertionScope())
			{
				detectedFileType.Should().Be(FileType.LOX);
				digest.Should().BeOfType<DigestLOX>();

				allPersons.Count.Should().Be(0);
				org.Should().NotBeNull();
			}
		}

		private class TestCasesObjects
		{
			public static object[] DigestLOX_ExpectOnePersonsTestCases =
			{
				//test case - base case
				new TestCaseObject(
					input: new List<string>
					{
						"CONTROLLED UNCLASSIFIED INFORMATION,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"(Controlled with Standard Dissemination),,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Letter of Certifications,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Squadron: 960 AACS,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,Flight Quals,,,,Dual Qual,,Certifications,,,,,,,,,,,,,,,,,,,Duty,,",
						"Name,CP,MDS,Rank,Flight,Msn Rdy Status Assigned,Exp Ind,Instructor,Evaluator,E-3G Dragon,E-3B/C/G Legacy Flt Deck,Primary Qual,Secondary Qual,FPS/E-Told,Bounce Recovery,SOF,ISOF,Ops Sup,Jeppesen Approach Plate Certified,SDP,WX Tier 1,WX Tier 2,KC-46 AAR Cert,DRAGON Sim Operator,E-3B Certified,E-3G Certified,Active Sensor Operations,Data Link Operations,SL Certified,Msn Commander,DRAGON Msn Crew,IPEC,Attached,Remarks,",
						"\"Alsop, Sophie\",ABM,E-3G,AMN,E,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: DNIF,",
						"\"Alsop, Sophie\",ABM,E-3G(II),AMN,E,CMR,I,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: DNIF,",
						",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"R,Certified w/ Restrictions,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"X,Certified,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"X,Certified/Qualified,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Remarks:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Signature:, ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						", ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",\"MICHAEL C. HOGAN, Lt Col, USAF\",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",Commander,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
					},
					expectedFirstName: "Sophie",
					expectedLastName: "Alsop",
					expectedFlight: "E",
					expectedOrg: "960 AACS",
					expectedRank: Rank.E2,
					expectedCrewPosition: "ABM"
				),

				//test case - removes anyhing with E-3G(II), different squadron
				new TestCaseObject(
					input: new List<string>
					{
						"Letter of Certifications,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Squadron: 522 ACNS,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,Flight Quals,,,,Dual Qual,,Certifications,,,,,,,,,,,,,,,,,,,Duty,,",
						"Name,CP,MDS,Rank,Flight,Msn Rdy Status Assigned,Exp Ind,Instructor,Evaluator,E-3G Dragon,E-3B/C/G Legacy Flt Deck,Primary Qual,Secondary Qual,FPS/E-Told,Bounce Recovery,SOF,ISOF,Ops Sup,Jeppesen Approach Plate Certified,SDP,WX Tier 1,WX Tier 2,KC-46 AAR Cert,DRAGON Sim Operator,E-3B Certified,E-3G Certified,Active Sensor Operations,Data Link Operations,SL Certified,Msn Commander,DRAGON Msn Crew,IPEC,Attached,Remarks,",
						"\"Alsop, Sophie\",ABM,E-3G,AMN,E,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: DNIF,",
						"\"This, should not be added\",ABM,E-3G(II),AMN,E,CMR,I,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: DNIF,",
					},
					expectedFirstName: "Sophie",
					expectedLastName: "Alsop",
					expectedFlight: "E",
					expectedOrg: "522 ACNS",
					expectedRank: Rank.E2,
					expectedCrewPosition: "ABM"
				),

				//test case - extra spaces in name, 2nd LT rank
				new TestCaseObject(
					input: new List<string>
					{
						"Letter of Certifications,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Squadron: 960 AACS,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,Flight Quals,,,,Dual Qual,,Certifications,,,,,,,,,,,,,,,,,,,Duty,,",
						"Name,CP,MDS,Rank,Flight,Msn Rdy Status Assigned,Exp Ind,Instructor,Evaluator,E-3G Dragon,E-3B/C/G Legacy Flt Deck,Primary Qual,Secondary Qual,FPS/E-Told,Bounce Recovery,SOF,ISOF,Ops Sup,Jeppesen Approach Plate Certified,SDP,WX Tier 1,WX Tier 2,KC-46 AAR Cert,DRAGON Sim Operator,E-3B Certified,E-3G Certified,Active Sensor Operations,Data Link Operations,SL Certified,Msn Commander,DRAGON Msn Crew,IPEC,Attached,Remarks,",
						"\" Alsop ,  Sophie \",ABM,E-3G,2nd Lt,E,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: DNIF,",
					},
					expectedFirstName: "Sophie",
					expectedLastName: "Alsop",
					expectedFlight: "E",
					expectedOrg: "960 AACS",
					expectedRank: Rank.O2,
					expectedCrewPosition: "ABM"
				),

				//test case - hyphenated last name
				new TestCaseObject(
					input: new List<string>
					{
						"Letter of Certifications,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Squadron: 960 AACS,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,Flight Quals,,,,Dual Qual,,Certifications,,,,,,,,,,,,,,,,,,,Duty,,",
						"Name,CP,MDS,Rank,Flight,Msn Rdy Status Assigned,Exp Ind,Instructor,Evaluator,E-3G Dragon,E-3B/C/G Legacy Flt Deck,Primary Qual,Secondary Qual,FPS/E-Told,Bounce Recovery,SOF,ISOF,Ops Sup,Jeppesen Approach Plate Certified,SDP,WX Tier 1,WX Tier 2,KC-46 AAR Cert,DRAGON Sim Operator,E-3B Certified,E-3G Certified,Active Sensor Operations,Data Link Operations,SL Certified,Msn Commander,DRAGON Msn Crew,IPEC,Attached,Remarks,",
						"\"Hyphe-nated, Sophie \",FE,E-3G,AMN,A,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: DNIF,",
					},
					expectedFirstName: "Sophie",
					expectedLastName: "Hyphe-nated",
					expectedFlight: "A",
					expectedOrg: "960th AACS",
					expectedRank: Rank.E2,
					expectedCrewPosition: "FE"
				),

				//test case - unknown rank, St. name, long squadron name
				new TestCaseObject(
					input: new List<string>
					{
						"Letter of Certifications,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Squadron: 960 AACS,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,Flight Quals,,,,Dual Qual,,Certifications,,,,,,,,,,,,,,,,,,,Duty,,",
						"Name,CP,MDS,Rank,Flight,Msn Rdy Status Assigned,Exp Ind,Instructor,Evaluator,E-3G Dragon,E-3B/C/G Legacy Flt Deck,Primary Qual,Secondary Qual,FPS/E-Told,Bounce Recovery,SOF,ISOF,Ops Sup,Jeppesen Approach Plate Certified,SDP,WX Tier 1,WX Tier 2,KC-46 AAR Cert,DRAGON Sim Operator,E-3B Certified,E-3G Certified,Active Sensor Operations,Data Link Operations,SL Certified,Msn Commander,DRAGON Msn Crew,IPEC,Attached,Remarks,",
						"\"St. Onge, Dean\",FE,E-3G,bad_rank,A,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: DNIF,",
					},
					expectedFirstName: "Dean",
					expectedLastName: "St. Onge",
					expectedFlight: "A",
					expectedOrg: "552 Air Control Network Squadron",
					expectedRank: Rank.Unknown,
					expectedCrewPosition: "FE"
				),

				//test case - no data other than name provided (and squadron
				new TestCaseObject(
					input: new List<string>
					{
						"Letter of Certifications,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Squadron: 960 AACS,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,Flight Quals,,,,Dual Qual,,Certifications,,,,,,,,,,,,,,,,,,,Duty,,",
						"Name,CP,MDS,Rank,Flight,Msn Rdy Status Assigned,Exp Ind,Instructor,Evaluator,E-3G Dragon,E-3B/C/G Legacy Flt Deck,Primary Qual,Secondary Qual,FPS/E-Told,Bounce Recovery,SOF,ISOF,Ops Sup,Jeppesen Approach Plate Certified,SDP,WX Tier 1,WX Tier 2,KC-46 AAR Cert,DRAGON Sim Operator,E-3B Certified,E-3G Certified,Active Sensor Operations,Data Link Operations,SL Certified,Msn Commander,DRAGON Msn Crew,IPEC,Attached,Remarks,",
						"\"Data, NO\",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
					},
					expectedFirstName: "No",
					expectedLastName: "Data",
					expectedFlight: "",
					expectedOrg: "960 AACS",
					expectedRank: Rank.Unknown,
					expectedCrewPosition: ""
				),
			};

			/// <summary>
			/// Test cases where it is expected that zero valid persons will be created in database
			/// </summary>
			public static object[] DigestLOX_ExpectZeroPersonsTestCases =
			{
				//test case - no first name
				new TestCaseObject(
					input: new List<string>
					{
						"Letter of Certifications,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Squadron: 960 AACS,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,Flight Quals,,,,Dual Qual,,Certifications,,,,,,,,,,,,,,,,,,,Duty,,",
						"Name,CP,MDS,Rank,Flight,Msn Rdy Status Assigned,Exp Ind,Instructor,Evaluator,E-3G Dragon,E-3B/C/G Legacy Flt Deck,Primary Qual,Secondary Qual,FPS/E-Told,Bounce Recovery,SOF,ISOF,Ops Sup,Jeppesen Approach Plate Certified,SDP,WX Tier 1,WX Tier 2,KC-46 AAR Cert,DRAGON Sim Operator,E-3B Certified,E-3G Certified,Active Sensor Operations,Data Link Operations,SL Certified,Msn Commander,DRAGON Msn Crew,IPEC,Attached,Remarks,",
						"\"last name, \",FE,E-3G,AMN,A,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: DNIF,",
					},
					expectedOrg: "960 AACS"
				),

				//test case - no last name
				new TestCaseObject(
					input: new List<string>
					{
						"Letter of Certifications,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Squadron: 960 AACS,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,Flight Quals,,,,Dual Qual,,Certifications,,,,,,,,,,,,,,,,,,,Duty,,",
						"Name,CP,MDS,Rank,Flight,Msn Rdy Status Assigned,Exp Ind,Instructor,Evaluator,E-3G Dragon,E-3B/C/G Legacy Flt Deck,Primary Qual,Secondary Qual,FPS/E-Told,Bounce Recovery,SOF,ISOF,Ops Sup,Jeppesen Approach Plate Certified,SDP,WX Tier 1,WX Tier 2,KC-46 AAR Cert,DRAGON Sim Operator,E-3B Certified,E-3G Certified,Active Sensor Operations,Data Link Operations,SL Certified,Msn Commander,DRAGON Msn Crew,IPEC,Attached,Remarks,",
						"\", first name\",FE,E-3G,AMN,A,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: DNIF,",
					},
					expectedOrg: "960 AACS"
				),

				//test case - no squadron
				new TestCaseObject(
					input: new List<string>
					{
						"Letter of Certifications,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,Flight Quals,,,,Dual Qual,,Certifications,,,,,,,,,,,,,,,,,,,Duty,,",
						"Name,CP,MDS,Rank,Flight,Msn Rdy Status Assigned,Exp Ind,Instructor,Evaluator,E-3G Dragon,E-3B/C/G Legacy Flt Deck,Primary Qual,Secondary Qual,FPS/E-Told,Bounce Recovery,SOF,ISOF,Ops Sup,Jeppesen Approach Plate Certified,SDP,WX Tier 1,WX Tier 2,KC-46 AAR Cert,DRAGON Sim Operator,E-3B Certified,E-3G Certified,Active Sensor Operations,Data Link Operations,SL Certified,Msn Commander,DRAGON Msn Crew,IPEC,Attached,Remarks,",
						"\"Alsop, Sophie\",ABM,E-3G,AMN,E,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: DNIF,",
						"\"Alsop, Sophie\",ABM,E-3G(II),AMN,E,CMR,I,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: DNIF,",
					},
					expectedOrg: "960 AACS"
				),


			};
		}

		public class TestCaseObject
		{
			IList<string> _input { get; set; }

			string _expectedFirstName { get; set; }

			string _expectedLastName { get; set; }

			string _expectedFlight { get; set; }

			string _expectedOrg { get; set; }
			
			Rank _expectedRank { get; set; }

			string _expectedCrewPosition { get; set; }

			public TestCaseObject(IList<string> input, string expectedFirstName, string expectedLastName, string expectedFlight, string expectedOrg, Rank expectedRank, string expectedCrewPosition)
			{
				_input = input;
				_expectedFirstName = expectedFirstName;
				_expectedLastName = expectedLastName;
				_expectedFlight = expectedFlight;
				_expectedOrg = expectedOrg;
				_expectedRank = expectedRank;
				_expectedCrewPosition = expectedCrewPosition;
			}

			public void Deconstruct(out IList<string> input, out string expectedFirstName, out string expectedLastName, out string expectedFlight, out string expectedOrgAlias, out Rank expectedRank, out string expectedCrewPosition)
			{
				input = _input;
				expectedFirstName = _expectedFirstName;
				expectedLastName = _expectedLastName;
				expectedFlight = _expectedFlight;
				expectedOrgAlias = _expectedOrg;
				expectedRank = _expectedRank;
				expectedCrewPosition = _expectedCrewPosition;
			}

			public TestCaseObject(IList<string> input, string expectedOrg)
			{
				_input = input;
				_expectedOrg = expectedOrg;
			}

			public void Deconstruct(out IList<string> input, out string expectedOrgAlias)
			{
				input = _input;
				expectedOrgAlias = _expectedOrg;
			}
		}
	}
}
