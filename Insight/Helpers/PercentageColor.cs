using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Toolkit.Uwp.UI.Controls.ColorPickerConverters;
using System;
using System.Collections.Generic;
using System.Drawing;
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
		public Style Color
		{
			get;
			set;
		}

		protected override Style SelectStyleCore(object item, DependencyObject container)
		{
			var cellInfo = item as DataGridCellInfo;
			PercentageToHexColor(cellInfo);
			return Color;
		}

		public void PercentageToHexColor(DataGridCellInfo cellInfo)
		{
			double cellValue;
			double.TryParse(cellInfo.Value.ToString().TrimEnd('%'), out cellValue);
			Setter setter = new Setter();
			setter.Property = TextBox.ForegroundProperty;
			byte r = 0;
			byte g = 250;
			byte b = 0;
			setter.Value = string.Format("{0:X2}{1:X2}{2:X2}", r, g, b);
			Color.Setters.Add(setter);
		}
	}
}
