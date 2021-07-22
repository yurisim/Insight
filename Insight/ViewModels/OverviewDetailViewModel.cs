using Insight.Core.Models;
using Insight.Core.Services;
using Insight.Core.Services.Database;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Insight.Core.Helpers;

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

        public ObservableCollection<ReadyPercentages> Source { get; } = new ObservableCollection<ReadyPercentages>();

        public OverviewDetailViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

			var insightController = new InsightController();
			var data = await insightController.GetAllPersons();

            List<string> allFlightNames = new List<string>();

            List<List<Person>> allFlights = new List<List<Person>>();

            foreach (var item in data)
            {
                if (allFlightNames.Contains(item.Flight.ToUpper()))
                {
                    allFlights[allFlightNames.IndexOf(item.Flight.ToUpper())].Add(item);
                }
                else
                {
                    allFlightNames.Add(item.Flight.ToUpper());
                    List<Person> newFlight = new List<Person>();
                    allFlights.Add(newFlight);
                }
            }

            ReadyPercentages OverallPercentages = new ReadyPercentages("960 Overall", DataCalculation.GetMedical(data), DataCalculation.GetPersonnel(data), DataCalculation.GetTraining(data));
            Source.Add(OverallPercentages);

            foreach (var item in allFlightNames)
            {
                Source.Add(FlightPercentageBuilder(item, allFlights[allFlightNames.IndexOf(item)]));
            }

        }


        private ReadyPercentages FlightPercentageBuilder(string flight, List<Person> data)
        {
            ReadyPercentages flightPercentages = new ReadyPercentages(string.Format("{0} Flight", flight), DataCalculation.GetMedical(data), DataCalculation.GetPersonnel(data), DataCalculation.GetTraining(data));
            return flightPercentages;
        }

        

        // Fix naming of this
        public struct ReadyPercentages
        {
            public string Organization { get; }
            public string Medical { get; }
            public string Personnel { get; }
            public string Training { get; }


            public ReadyPercentages(string rowName, string valueMed, string valuePersonnel, string valueTraining)
            {
				Organization = rowName;
				Medical = valueMed;
				Personnel = valuePersonnel;
				Training = valueTraining;
            }
        }
    }
}
