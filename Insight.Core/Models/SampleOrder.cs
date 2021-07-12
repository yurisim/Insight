using System;
using System.Collections.Generic;

namespace Insight.Core.Models
{
	// Remove this class once your pages/features are using your data.
	// This is used by the SampleDataService.
	// It is the model class we use to display data on pages like Grid, Chart, and List Detail.
	public class SampleOrder
	{
		public long DoDID { get; set; }

		public DateTime DateOfLastCompletion { get; set; }

		public DateTime RequiredDate { get; set; }

		public DateTime ShippedDate { get; set; }

		public string Name { get; set; }

		public string ShipperPhone { get; set; }

		public double Freight { get; set; }

		public string Organization { get; set; }

		public string ShipTo { get; set; }

		public double OrderTotal { get; set; }

		public string Status { get; set; }

		public char Symbol => (char)SymbolCode;

		public int SymbolCode { get; set; }

		public ICollection<SampleOrderDetail> Details { get; set; }

		public override string ToString()
		{
			return $"{Organization} {Status}";
		}

		public string ShortDescription => $"Order ID: {DoDID}";
	}
}