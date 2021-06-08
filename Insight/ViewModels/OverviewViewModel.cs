using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using Insight.Core.Models;
using Insight.Core.Services;
using Insight.Services;
using Insight.Views;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace Insight.ViewModels
{
    public class OverviewViewModel : ObservableObject
    {
        private ICommand _itemClickCommand;

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<SampleOrder>(OnItemClick));

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public OverviewViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await SampleDataService.GetContentGridDataAsync();
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        private void OnItemClick(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate<OverviewDetailPage>(clickedItem.DoDID);
            }
        }
    }
}
