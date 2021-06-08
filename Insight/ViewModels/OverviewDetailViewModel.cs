using System;
using System.Linq;
using System.Threading.Tasks;

using Insight.Core.Models;
using Insight.Core.Services;

using Microsoft.Toolkit.Mvvm.ComponentModel;

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

        public OverviewDetailViewModel()
        {
        }

        public async Task InitializeAsync(long orderID)
        {
            var data = await SampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.DoDID == orderID);
        }
    }
}
