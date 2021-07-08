﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insight.Core.Models
{
	public class Person
	{
		[Key, Required]
		public int PersonId { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Name => $"{LastName}, {FirstName}";

		public string Phone { get; set; }

		public long DoDID { get; set; }

		public string SSN { get; set; }

		public string DateOnStation { get; set; }

		public string Comments { get; set; }

		public string Flight { get; set; }

		public Medical Medical { get; set; }

		public Training Training { get; set; }

		public Personnel Personnel { get; set; }

		public AFSC AFSC { get; set; }

		public Org Organization { get; set; }

		public PEX PEX { get; set; }

		public List<CourseInstance> CourseInstances { get; set; }

	}
}