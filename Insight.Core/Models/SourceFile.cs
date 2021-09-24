using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insight.Core.Models
{
	public class SourceFile
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime DateUploaded { get; set; }
	}
}
