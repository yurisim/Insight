using Insight.Core.Models;
using Insight.Core.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Insight.ViewModels
{
    public class OverviewDetailViewModel : ObservableObject
    {
        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        public ObservableCollection<Person> Source { get; } = new ObservableCollection<Person>();

        public OverviewDetailViewModel()
        {
        }

        public async Task InitializeAsync(long orderID)
        {
            System.Collections.Generic.IEnumerable<SampleOrder> data = await SampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.DoDID == orderID);
        }
    }
}
