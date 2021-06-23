using Insight.Core.Models;
using Insight.Core.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
            System.Collections.Generic.IEnumerable<SampleOrder> data = await SampleDataService.GetGridDataAsync();

            foreach (SampleOrder item in data)
            {
                Source.Add(item);
            }
        }
    }
}
