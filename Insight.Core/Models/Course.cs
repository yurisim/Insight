using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insight.Core.Models
{
	/// <summary>
	/// Includes CBRN, CATM, SABC, Force Protection, Cyber Awareness, Law of War (Basic), Expeditionary
	/// </summary>
	public class Course : IGenericable
	{
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Name of the Course
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The Code is what the various courses are labeled as on the excel sheets
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Interval (in years) in which the course needs to be completed)
		/// </summary>
		public double Interval { get; set; }

		public List<CourseInstance> CourseInstances { get; set; }

		public Course()
		{
			CourseInstances = new List<CourseInstance>();
		}
	}
}
