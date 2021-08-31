using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Insight.Core.Models
{
	public class Org
	{
		private string _name;
		//private string _pascode;

		[Key]
		public int Id { get; set; }

		public string Name
		{
			get => _name;
			set => _name = value.ToUpper();
		}

		public List<Org> SubOrgs { get; set; } = new List<Org>();

		// IS there such a thing as a org PAS code?
		//public string PASCode
		//{
		//	get => _pascode;
		//	set => _pascode = value?.ToUpper();
		//}

		public ICollection<OrgAlias> Aliases { get; set; } = new List<OrgAlias>();
	}
}
