using FluentAssertions;
using Insight.Core.Models;
using Insight.Core.Services.File;
using NUnit.Framework;
using System.Collections.Generic;

namespace Insight.Core.UnitTests.nUnit.ServicesTests.FileTests
{
	[TestFixture]
	public class DetectorTests
	{
		/// <summary>
		/// Tests based on test cases defined below in DetectorTestCasesObject.DetectorTestCases
		/// </summary>
		/// <param name="input"></param>
		/// <param name="expected"></param>
		[TestCaseSource(typeof(DetectorTestCasesObject), nameof(DetectorTestCasesObject.DetectorTestCases))]
		public void DetectorTest(IList<string> input, FileType expected)
		{
			//arrange

			//act
			var detectedFiletype = Detector.DetectFileType(input);

			//assert
			detectedFiletype.Should().Be(expected);
		}

		private class DetectorTestCasesObject
		{
			public static object[] DetectorTestCases =
			{
				//test case - detect LOX
				new object[] {
					//input
					new List<string>
					{
						"CONTROLLED UNCLASSIFIED INFORMATION,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"(Controlled with Standard Dissemination),,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Letter of Certifications,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						"Squadron: 960 AACS,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",
						",,,,,,,Flight Quals,,,,Dual Qual,,Certifications,,,,,,,,,,,,,,,,,,,Duty,,",
						"Name,CP,MDS,Rank,Flight,Msn Rdy Status Assigned,Exp Ind,Instructor,Evaluator,E-3G Dragon,E-3B/C/G Legacy Flt Deck,Primary Qual,Secondary Qual,FPS/E-Told,Bounce Recovery,SOF,ISOF,Ops Sup,Jeppesen Approach \"Plate Certified,SDP,WX Tier 1,WX Tier 2,KC-46 AAR Cert,DRAGON Sim Operator,E-3B Certified,E-3G Certified,Active Sensor Operations,Data Link Operations,SL Certified,Msn Commander,DRAGON Msn Crew,IPEC,Attached,Remarks,",
						"\"Alsop, Sophie\",ABM,E-3G,AMN,E,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: DNIF,",
						"\"Alsop, Sophie\",ABM,E-3G(II),AMN,E,CMR,I,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: DNIF,",
					},
					//expected
					FileType.LOX
				},
				//end test case

				//test case - detect ETMS
				new object[] {
					//input
					new List<string>
					{
						"PAS Description,Course Title,Last Name,First Name,Completion Date",
						"960 AACS,Self Aid & Buddy Care (SABC),ALSOP,SOPHIE,4/23/2021",
						"960 AACS,Self Aid & Buddy Care (SABC),ALSOP,SOPHIE,"
					},
					//expected
					FileType.ETMS
				},
				//end test case

				//test case - detect AEF
				new object[] {
					//input
					new List<string>
					{
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,",
						"Export Description:  UnitRoster,,,,,,,,,,,,,,,,",
						"Name,Course Count,DRC Status for Email,PayGrade,AEFI,Unit,PASCode,AFSC,Gender,Duty Status,Personnel,Medical,Training,Has AEF Account,Visited AEF Online,Checklist Status,ModeTip",
						"SMITH JOHN,0,,E3,YR,552 AIR CON/NETWORKS SQ (FFKG80),TE1CFKG8,3D054,M,PRES FOR DUTY,G,G,R,Y,Y,Member Started,",
					},
					//expected
					FileType.AEF
				},
				//end test case

				//test case - detect PEX
				new object[] {
					//input
					new List<string>
					{
						"ShortName,PEX Designation",
						"SmithJ,A",
						"ClarkJ,B",
					},
					//expected
					FileType.PEX
				},
				//end test case

				//test case - detect AlphaRoster
				new object[] {
					//input - FOUO header/footer
					new List<string>
					{
						"FULL_NAME,SSAN,GRADE,RECORD_STATUS,ASSIGNED_PAS,ASSIGNED_PAS_CLEARTEXT,OFFICE_SYMBOL,DUTY_TITLE,DUTY_START_DATE,DOR,DAFSC,CAFSC,PAFSC,2AFSC,3AFSC,4AFSC,DATE_ARRIVED_STATION,DUTY_PHONE,TAFMSD,DOE,DOS,ETS,DATE_OF_BIRTH,HOME_ADDRESS,HOME_CITY,HOME_STATE,HOME_ZIP_CODE,SUPV_NAME,GRADE_PERM_PROJ,UIF_CODE,UIF_DISPOSITION_DATE,PROJ_EVAL_CLOSE_DATE,MARITAL_STATUS,RNLTD,GAINING_PAS,GAINING_PAS_CLEARTEXT,LAST_EVAL_RATING,LAST_EVAL_CLOSE_DATE,PERF_INDICATOR,SPOUSE_SSAN,SUPV_BEGIN_DATE,REENL_ELIG_STATUS,HOME_PHONE_NUMBER,AGE,DEROS,DEPLOY_ADMIN_STATUS,DEPLOY_ADMIN_STATUS_CLEARTEXT,DEPLOY_ADMIN_STOP_DATE,DEPLOY_LEGAL_STATUS,DEPLOY_LEGAL_STATUS_CLEARTEXT,DEPLOY_LEGAL_STOP_DATE,DEPLOY_PHYS_STATUS,DEPLOY_PHYS_STATUS_CLEARTEXT,DEPLOY_PHYS_STOP_DATE,DEPLOY_TIME_STATUS,DEPLOY_TIME_STATUS_CLEARTEXT,DEPLOY_TIME_STOP_DATE,AVAILABILITY_CODE,AVAILABILITY_CODE_CLEARTEXT,AVAILABILITY_DATE,AVAILABILITY_STATUS,AVAILABILITY_STATUS_CLEARTEXT,LIMITATION_CODE,LIMITATION_CODE_CLEARTEXT,LIMITATION_END_DATE,SEC_CLR,TYPE_SEC_INV,DT_SCTY_INVES_COMPL,SEC_ELIG_DT,TECH_ID,ACDU_STATUS,ANG_ROLL_INDICATOR,AFR_SECTION_ID,CIVILIAN_ART_ID,ATTACHED_PAS,FUNCTIONAL_CATEGORY",
						"\"SMITH, JOHN\",123-45-6789,A1C,10,TE1CFKG8,552 AIR CON/NETWORKS SQ FFKG80,DOUP,SOFTWARE DEVELOPER,10-JUN-2020,10-DEC-2019,-3D034,-3D034,3D054,,,,10-JUN-2020,4057345462,10-DEC-2019,10-DEC-2019,09-DEC-2023,09-DEC-2023,07-NOV-1991,8094 SE DELINE STREET,HILLSBORO,OR,97123,WILSON THOMAS,,,,31-MAR-2023,S,,,,,,A,,10-JUN-2020,3C,5039155972,29,,,,,,,,,,,,,,,,,,,,,,V,70,08-NOV-2019,14-NOV-2019,,,,,,,A",
						"\"CLARK, JOSHUA\",261-98-8289,AB,10,TE1CFKG8,552 AIR CON/NETWORKS SQ FFKG80,DOUP,PILOT,10-JUN-2021,10-DEC-2020,-3D035,-3D035,3D055,,,,10-JUN-2021,4057345463,10-DEC-2020,10-DEC-2020,09-DEC-2024,09-DEC-2024,07-NOV-1992,8095 SE DELINE STREET,NEW YORK,OK,97124,WILSON THOMAS,,,,31-MAR-2024,S,,,,,,A,,,,3128306373,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,",

					},
					//expected
					FileType.AlphaRoster
				},
				//end test case

				//test case - detect SFMIS
				new object[] {
					//input - FOUO header/footer
					new List<string>
					{
						"\"The information herein is For Official Use Only (FOUO) which must be protected under the FOIA and Privacy Act, as amended.  Unauthorized disclosure or misuse of this PERSONAL INFORMATION may result in criminal and/or civil penalties.\",,,,,,,,,,,,,,,,,,,,",
						"Export Description:  SFMISRoster,,,,,,,,,,,,,,,,,,,,",
						"Name,Unit,PasCode,AEFI,AFSC,PayGrade,Email,Email4Career,Gender,DutyStatus,SecurityClearance,Security_Status,Security_Expire,TrainingNeeds,Course,Weapon_Model,Course_ID,Completion_Date,Expire_Date,Qualification,Arming_Group",
						"ALSOP SOPHIE JANE,960 AIRBORNE AIR CTRL SQ (FFDFP0),TE1CFDFP,YR,013B3B,E2,sophie.alsop@us.af.mil,sophie.alsop@us.af.mil,F,TDY CONTGENCY,SCI(DCID 1/14 ELIGIBLE) - expires 24Jan24,expires,24-JAN-24,,M9 HG AFQC (INITIAL/RECURRING),M9,219882,2021-04-26,2022-04-30,QUALIFIED,",

					},
					//expected
					FileType.SFMIS
				},
				//end test case

				//test case - detect ARIS_Handgun
				new object[] {
					//input - FOUO header/footer
					new List<string>
					{
						"People Assigned,,,,,,,,,,,,",
						",,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"HANDGUN (GROUP B),,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Name,Organizations,Control AFSC,Duty AFSC,Rank,Scheduled Date,Last Completion Date,Status,Qual Expiration Date,Qual Status,User Assigned,Do Not Schedule,Licensed",
						"\"Alsop, Sophie\",960 AIRBORNE AIR CTRL SQ FFDFP0,,,AMN,,26 Apr 2021,CURRENT,30 Apr 2022,QUALIFIED,Y,N,N",

					},
					//expected
					FileType.ARIS_Handgun
				},
				//end test case

				//test case - detect ARIS_Rifle/Carbine
				new object[] {
					//input - FOUO header/footer
					new List<string>
					{
						"People Assigned,,,,,,,,,,,,",
						",,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"RIFLE/CARBINE (Group B),,,,,,,,,,,,",
						",,,,,,,,,,,,",
						"Name,Organizations,Control AFSC,Duty AFSC,Rank,Scheduled Date,Last Completion Date,Status,Qual Expiration Date,Qual Status,User Assigned,Do Not Schedule,Licensed",
						"\"Alsop, Sophie\",960 AIRBORNE AIR CTRL SQ FFDFP0,,,AMN,,26 Apr 2021,CURRENT,30 Apr 2022,QUALIFIED,Y,N,N",

					},
					//expected
					FileType.ARIS_Rifle_Carbine
				},
				//end test case

				//test case - empty list
				new object[] {
					//input - FOUO header/footer
					new List<string>
					{

					},
					//expected
					FileType.Unknown
				},
				//end test case

				//test case - null
				new object[] {
					//input - FOUO header/footer
					null,
					//expected
					FileType.Unknown,
				},
				//end test case

				//test case - random data
				new object[] {
					//input - FOUO header/footer
					new List<string>
					{
						"this is some random data that should come back FileType.Unknown",
						"here is some more stuff",
						"And guess what. this is another line of stuff",
					},
					//expected
					FileType.Unknown,
				},
				//end test case

			};
		}
	}
}
