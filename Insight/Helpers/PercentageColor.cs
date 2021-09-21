using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.Grid;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Insight.Helpers
{
	public class PercentageColor : StyleSelector
	{
		public Style VeryHighPercent
		{
			get;
			set;
		}
		public Style HighPercent
		{
			get;
			set;
		}
		public Style MediumPercent
		{
			get;
			set;
		}
		public Style VeryLowPercent
		{
			get;
			set;
		}
		public Style LowPercent
		{
			get;
			set;
		}

		protected override Style SelectStyleCore(object item, DependencyObject container)
		{
			double cellValue;
			DataGridCellInfo cellInfo = item as DataGridCellInfo;
			double.TryParse(cellInfo.Value.ToString().TrimEnd('%'), out cellValue);
			return GetStyle(cellValue);
		}

		public Style GetStyle(double cellValue)
		{
			if (cellValue >= 90)
			{
				return VeryHighPercent;
			}
			else if (cellValue >= 80)
			{
				return HighPercent;
			}
			else if (cellValue >= 70)
			{
				return MediumPercent;
			}
			else if (cellValue >= 60)
			{
				return LowPercent;
			}
			else
			{
				return VeryLowPercent;
			}
		}
	}
}
