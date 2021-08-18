using System;
using System.ComponentModel.DataAnnotations;

namespace Insight.Core.Models
{
	public class CourseInstance : IGenericable
	{
		[Key]
		public int Id { get; set; }

		public Course Course { get; set; }

		public Person Person { get; set; }

		public DateTime Completion { get; set; }

		public DateTime Expiration { get; set; }
	}
}
