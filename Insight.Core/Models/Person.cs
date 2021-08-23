using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insight.Core.Models
{
	public class Person
	{
		private string _firstName;
		private string _lastName;

		[Key, Required]
		public int Id { get; set; }

		public string FirstName
		{
			get => _firstName;
			set => _firstName = value.ToUpperInvariant();
		}

		public string LastName
		{
			get => _lastName;
			set => _lastName = value.ToUpperInvariant();
		}

		public Rank Rank { get; set; }

		public string Name => $"{LastName}, {FirstName}";

		public string HomePhone { get; set; }

		public string Email {  get; set; }

		public long DoDID { get; set; }

		public string SSN { get; set; }

		public string CrewPosition { get; set; }

		public string DateOnStation { get; set; }

		public string Comments { get; set; }

		public string Flight { get; set; }

		public Medical Medical { get; set; }

		public Training Training { get; set; }

		public Personnel Personnel { get; set; }

		public AFSC AFSC { get; set; }

		public Org Organization { get; set; }

		public PEX PEX { get; set; }

		public List<CourseInstance> CourseInstances { get; set; } = new List<CourseInstance>();

		public Person()
		{
			
		}

	}
}
