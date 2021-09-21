using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insight.Core.Models
{
	public class AFSC
	{
		private string _pafsc;
		private string _cafsc;
		private string _dafsc;

		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Primary AFSC
		/// </summary>
		public string PAFSC
		{
			get => _pafsc;
			set => _pafsc = value.ToUpper();
		}

		/// <summary>
		/// Duty AFSC
		/// </summary>
		public string DAFSC
		{
			get => _dafsc;
			set => _dafsc = value.ToUpper();
		}

		/// <summary>
		/// Control AFSC
		/// </summary>
		public string CAFSC
		{
			get => _cafsc;
			set => _cafsc = value.ToUpper();
		}

		public int TBATasksNeeded { get; set; }

		/// <summary>
		/// These are the people that are assigned to this AFSC
		/// </summary>
		public List<Person> Persons { get; set; } = new List<Person>();
	}
}
