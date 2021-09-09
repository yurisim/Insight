using System;
using Telerik.UI.Xaml.Controls.Grid;
using Telerik.UI.Xaml.Controls.Grid.Commands;

namespace Insight.Helpers
{

	public class StatusPageGenerateColumnCommand : DataGridCommand
	{
		public StatusPageGenerateColumnCommand()
		{
			this.Id = CommandId.GenerateColumn;

			//this might work, but it looks at all cells
			//so probably wrong approach
			//this.Id = CommandId.DataBindingComplete;
		}

		/// <summary>
		/// This is a delegate command that checks if the column should be rendered. By default it'll render every column.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public override bool CanExecute(object parameter)
		{
			// var context = parameter as GenerateColumnContext;
			// put your custom logic here

			//
			return true;
		}

		/// <summary>
		/// Executes the following code on each column where CanExecute is true.
		/// </summary>
		/// <param name="parameter"></param>
		public override void Execute(object parameter)
		{
			var context = parameter as GenerateColumnContext;
			//If column name has "Expiration" or "Date" in it, set date format. Otherwise sets it to a text column
			if (context.PropertyName.Contains("Expiration", StringComparison.CurrentCultureIgnoreCase) || context.PropertyName.Contains("Date", StringComparison.CurrentCultureIgnoreCase))
			{
				DataGridDateColumn column = new DataGridDateColumn();
				column.CellContentFormat = "{0: MM/dd/yyyy}";
				context.Result = column;
			}
			else
			{
				context.Result = new DataGridTextColumn();
			}
		}
	}
}
