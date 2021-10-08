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

		/// <summary>
		/// allows the Frontend to recieve the style
		/// </summary>
		/// <param name="item"></param>
		/// <param name="container"></param>
		/// <returns></returns>
		protected override Style SelectStyleCore(object item, DependencyObject container)
		{
			double cellValue;
			DataGridCellInfo cellInfo = item as DataGridCellInfo;
			double.TryParse(cellInfo.Value.ToString().TrimEnd('%'), out cellValue);
			//Separated functional code for  testabliity 
			return GetStyle(cellValue);
		}


		/// <summary>
		/// Returns the style based on a given percent
		/// </summary>
		/// <param name="cellValue">Percentage in the given cell</param>
		/// <returns>Style for the given cell</returns>
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
