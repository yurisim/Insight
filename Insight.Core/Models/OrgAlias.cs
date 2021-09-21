using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insight.Core.Models
{
	public class OrgAlias
	{
		private string _name;

		[Key, Required]
		public int Id { get; set; }

		[Required]
		public Org Org { get; set; }

		[Required]
		public string Name
		{
			get => _name;
			set => _name = value.ToUpper();
		}
	}
}
