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
            //System.Collections.Generic.IEnumerable<SampleOrder> data = await SampleDataService.GetContentGridDataAsync();
            //Item = data.First(i => i.DoDID == orderID);

            Source.Clear();

            var data = await Interact.GetAllPersons();

            List<Person> aFlight = new List<Person>();
            List<Person> bFlight = new List<Person>();
            List<Person> cFlight = new List<Person>();
            List<Person> dFlight = new List<Person>();
            List<Person> eFlight = new List<Person>();
            List<Person> fFlight = new List<Person>();

            //TODO: MAKE SWITCH

            foreach (var item in data)
            {
                if (item.Flight?.ToUpper() == "A")
                {
                    aFlight.Add(item);
                }
                else if (item.Flight?.ToUpper() == "B")
                {
                    bFlight.Add(item);
                }
                else if (item.Flight?.ToUpper() == "C")
                {
                    cFlight.Add(item);
                }
                else if (item.Flight?.ToUpper() == "D")
                {
                    dFlight.Add(item);
                }
                else if (item.Flight?.ToUpper() == "E")
                {
                    eFlight.Add(item);
                }
                else if (item.Flight?.ToUpper() == "F")
                {
                    fFlight.Add(item);
                }
            }

            ReadyPercentages medPercentages = new ReadyPercentages("Medical Overall", GetMedical(data), GetMedical(aFlight), GetMedical(bFlight), GetMedical(cFlight), GetMedical(dFlight),
                GetMedical(eFlight), GetMedical(fFlight));
            Source.Add(medPercentages);

            ReadyPercentages personnelPercentages = new ReadyPercentages("Personnel Overall", GetPersonnel(data), GetPersonnel(aFlight), GetPersonnel(bFlight), GetPersonnel(cFlight),
                GetPersonnel(dFlight), GetPersonnel(eFlight), GetPersonnel(fFlight));
            Source.Add(personnelPercentages);

            ReadyPercentages trainingPercentages = new ReadyPercentages("Training Overall", GetTraining(data), GetTraining(aFlight), GetTraining(bFlight), GetTraining(cFlight),
                GetTraining(dFlight), GetTraining(eFlight), GetTraining(fFlight));
            Source.Add(trainingPercentages);
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
                    if (item.Medical.OverallStatus == Status.Current || item.Medical.OverallStatus == Status.Upcoming)
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
                    if (item.Medical.OverallStatus == Status.Current || item.Medical.OverallStatus == Status.Upcoming)
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
            public string ValueAll { get; }
            public string FlightA { get; }
            public string FlightB { get; }
            public string FlightC { get; }
            public string FlightD { get; }
            public string FlightE { get; }
            public string FlightF { get; }


            public ReadyPercentages(string rowName, string value1, string valueA, string valueB, string valueC, string valueD, string valueE, string valueF)
            {
                RowName = rowName;
                ValueAll = value1;
                FlightA = valueA;
                FlightB = valueB;
                FlightC = valueC;
                FlightD = valueD;
                FlightE = valueE;
                FlightF = valueF;
            }
        }
    }
}
