using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Insight.Core.Models
{
	public class GradeAlias
	{
		[Key, Required]
		public int Id { get; set; }

		public Grade Grade { get; set; }

		public string Name { get; set; }
	}
}
