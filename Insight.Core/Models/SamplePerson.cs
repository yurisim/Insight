using System.Collections.Generic;

namespace Insight.Core.Models
{
	// Remove this class once your pages/features are using your data.
	// This is used by the SampleDataService.
	// It is the model class we use to display data on pages like Grid, Chart, and List Detail.
	public class SamplePerson
	{
		public string DoDID { get; set; }

		public string PersonName { get; set; }

		public string ContactName { get; set; }

		public string ContactTitle { get; set; }

		public string Address { get; set; }

		public string City { get; set; }

		public string PostalCode { get; set; }

		public string Country { get; set; }

		public string Phone { get; set; }

		public string Fax { get; set; }

		public ICollection<SampleOrder> Orders { get; set; }
	}
}