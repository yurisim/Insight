using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insight.Core.Models
{
	public class AFSC
	{
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Primary AFSC
		/// </summary>
		public string PAFSC { get; set; }

		/// <summary>
		/// Duty AFSC
		/// </summary>
		public string DAFSC { get; set; }

		/// <summary>
		/// Control AFSC
		/// </summary>
		public string CAFSC { get; set; }

		public int TBATasksNeeded { get; set; }

		/// <summary>
		/// These are the people that are assigned to this AFSC
		/// </summary>
		public List<Person> Persons { get; set; }
	}
}
