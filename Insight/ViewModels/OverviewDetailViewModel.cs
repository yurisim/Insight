using Insight.Core.Models;
using Insight.Core.Services;
using Insight.Core.Services.Database;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
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



        public ObservableCollection<ReadyPercentages> Source { get; } = new ObservableCollection<ReadyPercentages>();

        public OverviewDetailViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            var data = await Interact.GetAllPersons();

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

            ReadyPercentages OverallPercentages = new ReadyPercentages("960", GetMedical(data), GetPersonnel(data), GetTraining(data));
            Source.Add(OverallPercentages);

            foreach (var item in allFlightNames)
            {
                Source.Add(FlightPercentageBuilder(item, allFlights[allFlightNames.IndexOf(item)]));
            }

        }


        private ReadyPercentages FlightPercentageBuilder(string flight, List<Person> data)
        {
            ReadyPercentages flightPercentages = new ReadyPercentages(flight, GetMedical(data), GetPersonnel(data), GetTraining(data));
            return flightPercentages;
        }

        private string GetMedical(List<Person> data)
        {
            string medicalPercentageOutput = "Unknown";
            if (data.Count != 0)
            {
                decimal medicalPercentage = 0;
                foreach (var item in data)
                {
                    if (item.Medical.OverallStatus == Status.Current || item.Medical.OverallStatus == Status.Upcoming)
                    {
                        medicalPercentage++;
                    }
                }
                medicalPercentage /= data.Count;
                medicalPercentageOutput = string.Format("{0:P}", medicalPercentage);
            }
            return medicalPercentageOutput;
        }

        private string GetPersonnel(List<Person> data)
        {
            string personnelPercentageOutput = "Unknown";
            if (data.Count != 0)
            {
                decimal personnelPercentage = 0;
                foreach (var item in data)
                {
                    if (item.Personnel.OverallStatus == Status.Current || item.Personnel.OverallStatus == Status.Upcoming)
                    {
                        personnelPercentage++;
                    }
                }
                personnelPercentage /= data.Count;
                personnelPercentageOutput = string.Format("{0:P}", personnelPercentage);
            }
            return personnelPercentageOutput;
        }

        private string GetTraining(List<Person> data)
        {
            string trainingPercentageOutput = "Unknown";
            if (data.Count != 0)
            {
                decimal trainingPercentage = 0;
                foreach (var item in data)
                {
                    if (item.Training.OverallStatus == Status.Current || item.Training.OverallStatus == Status.Upcoming)
                    {
                        trainingPercentage++;
                    }
                }
                trainingPercentage /= data.Count;
                trainingPercentageOutput = string.Format("{0:P}", trainingPercentage);
            }
            return trainingPercentageOutput;
        }

        // Fix naming of this
        public struct ReadyPercentages
        {
            public string RowName { get; }
            public string ValueMed { get; }
            public string ValuePersonnel { get; }
            public string ValueTraining { get; }


            public ReadyPercentages(string rowName, string valueMed, string valuePersonnel, string valueTraining)
            {
                RowName = rowName;
                ValueMed = valueMed;
                ValuePersonnel = valuePersonnel;
                ValueTraining = valueTraining;
            }
        }
    }
}
