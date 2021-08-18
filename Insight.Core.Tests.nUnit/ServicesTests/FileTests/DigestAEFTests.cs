using System.Collections.Generic;
using FluentAssertions;
using Insight.Core.Services;
using Insight.Core.Services.File;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Insight.Core.Tests.nUnit.ServicesTests.FileTests
{
	[TestFixture]
	public class DigestAEFTests : DigestAEF
	{
		[TearDown]
		public void TearDown()
		{
			insightController.EnsureDatabaseDeleted();
		}

		private static readonly DbContextOptions<InsightContext> dbContextOptions =
			new DbContextOptionsBuilder<InsightContext>()
				.UseInMemoryDatabase("InsightTestDB")
				.Options;

		public DigestAEFTests() : base(null, dbContextOptions)
		{
		}

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.AEFCleanInputTestCases))]
		public void AEFCleanInputTest(IList<string> input, IList<string> expected)
		{
			FileContents = input;

			CleanInput();

			FileContents.Should().BeEquivalentTo(expected);
		}

		//[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.AEFSetColumnIndexesCases))]
		public void AEFSetColumnIndexesTest(string[] input, string[] expected)
		{
			SetColumnIndexes(input);

			FileContents.Should().BeEquivalentTo(expected);
		}
	}

	public partial class TestCasesObjects
	{
		public static object[] AEFCleanInputTestCases =
		{
			//test case
			new object[]
			{
				//input - FOUO header/footer
				new List<string>
				{
					"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",
					"Export Description:  UnitRoster,,,,,,,,,,,,,,,,",
					"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip",
					"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,G,G,R,Y,Y,Member Started,",
					"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,"
				},
				//expected
				new List<string>
				{
					"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,G,G,R,Y,Y,Member Started,"
				}
			},
			//end test case

			//test case
			new object[]
			{
				//input - CUI header/footer
				new List<string>
				{
					"CONTROLLED UNCLASSIFIED INFORMATION,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
					"Export Description:  UnitRoster,,,,,,,,,,,,,,,,",
					"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip",
					"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,G,G,R,Y,Y,Member Started,",
					"CONTROLLED UNCLASSIFIED INFORMATION,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,"
				},
				//expected
				new List<string>
				{
					"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,G,G,R,Y,Y,Member Started,"
				}
			},
			//end test case

			//test case
			new object[]
			{
				//input - CUI footer
				new List<string>
				{
					"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip",
					"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,G,G,R,Y,Y,Member Started,",
					"CONTROLLED UNCLASSIFIED INFORMATION,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,"
				},
				//expected
				new List<string>
				{
					"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,G,G,R,Y,Y,Member Started,"
				}
			},
			//end test case

			//test case
			new object[]
			{
				//input - only column header 
				new List<string>
				{
					"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip",
					"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,G,G,R,Y,Y,Member Started,"
				},
				//expected
				new List<string>
				{
					"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,G,G,R,Y,Y,Member Started,"
				}
			},
			//end test case

			//test case
			new object[]
			{
				//input - reversed column headers
				new List<string>
				{
					"ModeTip,Checklist Status,Visited AEF Online,Has AEF Account,Training,Medical,Personnel,Duty Status,Gender,AFSC,PASCode,Unit,AEFI,PayGrade,DRC Status for Email,Course Count,Name",
					",Member Started,Y,Y,R,G,G,PRES FOR DUTY,M,3D054,TE1CFKG8,552 AIR CON/NETWORKS SQ (FFKG80),YR,E3,,0,SMITH JOHN"
				},
				//expected
				new List<string>
				{
					",Member Started,Y,Y,R,G,G,PRES FOR DUTY,M,3D054,TE1CFKG8,552 AIR CON/NETWORKS SQ (FFKG80),YR,E3,,0,SMITH JOHN"
				}
			},
			//end test case

			//test case
			new object[]
			{
				//input - only column headers, no data
				new List<string>
				{
					"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip"
				},
				//expected
				new List<string>()
			},
			//end test case

			//test case
			new object[]
			{
				//input - empty list input
				new List<string>(),
				//expected
				new List<string>()
			}
			//end test case
		};

		//public static object[] AEFSetColumnIndexesCases =
		//{

		//}
	}
}
