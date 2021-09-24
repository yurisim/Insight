namespace Insight.Core.Models
{
	public class WeaponCourseTypes
	{
		public const string Handgun = "M9 Handgun Course";
		public const string Rifle_Carbine = "M4 Rifle/Carbine Course";
	}

	public enum Status
	{
		Unknown = 0,
		Current = 1,
		Upcoming = 2,
		Overdue = 4
	}

	/// <summary>
	/// Status of an individual's deployment readiness/preparedness 
	/// </summary>
	public enum DeploymentStatus
	{
		//Regular is first so that it is the default value
		///<summary>Normal operating procedures and is not scheduled for an upcoming deployment</summary>
		Regular,
		///<summary>Returning from deployment or on R&R</summary>
		Resetting,
		///<summary>Completed all defined training & readiness requirements</summary>
		Ready,
		///<summary>Ready and scheduled to deply</summary>
		Scheduled,
		///<summary>Currently Deployed</summary>
		Deployed,
	}

	public enum Grade
	{
		Unknown,
		E1,
		E2,
		E3,
		E4,
		E5,
		E6,
		E7,
		E8,
		E9,
		O1,
		O2,
		O3,
		O4,
		O5,
		O6,
		O7,
		O8,
		O9,
		O10,
	}

	public enum FileType
	{
		Unknown,
		AlphaRoster,
		///<summary>Patriot Excalibur</summary>
		PEX,
		///<summary>AEF Online</summary>
		AEF,
		///<summary>Patriot Excalibur</summary>
		ETMS,
		///<summary>Letter of Certifications</summary>
		LOX,
		///<summary>Security Forces Management Information System</summary>
		SFMIS,
		///<summary>Automated Readiness Information System; Handgun</summary>
		ARIS_Handgun,
		///<summary>Automated Readiness Information System; Rifle/Carbine</summary>
		ARIS_Rifle_Carbine,
	}
}
