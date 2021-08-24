namespace Insight.Core.Models
{
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
		///<summary>Unknown deployment status</summary>
		Unknown,
		///<summary>Returning from deployment or on R&R</summary>
		Resetting,
		///<summary>Training to prepare for deployment</summary>
		Training,
		///<summary>Completed all defined training & readiness requirements</summary>
		Ready,
		///<summary>Ready and scheduled to deply</summary>
		Scheduled,
		///<summary>Currently Deployed</summary>
		Deployed,
	}

	public enum Rank
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
	}
}
