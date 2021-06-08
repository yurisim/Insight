using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Insight.Core.Models;
using Insight.Core.Services;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Insight.ViewModels
{
    public class StatusViewModel : ObservableObject
    {
        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public StatusViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await SampleDataService.GetGridDataAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
