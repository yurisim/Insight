using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insight.Core.Models
{
	public class Medical
	{
		[Key, Required]
		public int Id { get; set; }

		public Status OverallStatus { get; set; }

		public Status Dental { get; set; }

		public Status Immunizations { get; set; }

		public Status Lab { get; set; }

		public Status PHA { get; set; }
	}
}
