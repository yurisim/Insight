using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insight.Core.Models
{
	public class Personnel
	{
		[Key, Required]
		public int Id { get; set; }

		public Status OverallStatus { get; set; }
	}
}
