using System.ComponentModel.DataAnnotations;

namespace Insight.Core.Models
{
	public class TBA
	{
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Name of AFSC
		/// </summary>
		public string Name { get; set; }

		public int TBATasksNeeded { get; set; }
	}
}
