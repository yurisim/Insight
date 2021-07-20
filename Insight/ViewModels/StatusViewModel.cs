using Insight.Core.Models;
using Insight.Core.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Insight.Core.Services.Database;

namespace Insight.ViewModels
{
    public class StatusViewModel : ObservableObject
    {
        public ObservableCollection<Person> Source { get; } = new ObservableCollection<Person>();



        public StatusViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

			// Replace this with your actual data
			//var data = await SampleDataService.GetGridDataAsync();

			InsightController controller = new InsightController();

			var data = await controller.GetAllPersons();


            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
