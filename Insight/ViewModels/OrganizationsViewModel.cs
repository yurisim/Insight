using Insight.Core.Models;
using Insight.Core.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Insight.ViewModels
{
	public class OrganizationsViewModel : ObservableObject
	{
		private SampleOrder _selected;

		public SampleOrder Selected
		{
			get { return _selected; }
			set { SetProperty(ref _selected, value); }
		}

		public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

		public OrganizationsViewModel()
		{
		}

		public async Task LoadDataAsync(ListDetailsViewState viewState)
		{
			SampleItems.Clear();

			System.Collections.Generic.IEnumerable<SampleOrder> data = await SampleDataService.GetListDetailDataAsync();

			foreach (SampleOrder item in data)
			{
				SampleItems.Add(item);
			}

			if (viewState == ListDetailsViewState.Both)
			{
				Selected = SampleItems.First();
			}
		}
	}
}
