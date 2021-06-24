using Insight.Core.Models;
using Insight.Core.Services;
using Insight.Services;
using Insight.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Insight.ViewModels
{
    public class OverviewViewModel : ObservableObject
    {
        private ICommand _itemClickCommand;
        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<SampleOrder>(OnItemClick));

        private ICommand _uploadPageCommand;
        public ICommand UploadPageCommand => _uploadPageCommand ?? (_uploadPageCommand = new RelayCommand(GoToUpload));

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public OverviewViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // Replace this with your actual data
            System.Collections.Generic.IEnumerable<SampleOrder> data = await SampleDataService.GetContentGridDataAsync();
            foreach (SampleOrder item in data)
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

                //NavigationService.Navigate<OverviewDetailPage>(clickedItem.DoDID);
            }
        }

        private void GoToUpload()
        {
            NavigationService.Navigate<UploadPage>();
        }
    }
}
