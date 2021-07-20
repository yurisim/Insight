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

			var insightController = new InsightController();
			var data = await insightController.GetAllPersons();

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

            ReadyPercentages medPercentages = new ReadyPercentages("Medical Overall", string.Format("{0:P}", GetMedical(data)), string.Format("{0:P}",
                GetMedical(aFlight)), string.Format("{0:P}", GetMedical(bFlight)), string.Format("{0:P}", GetMedical(cFlight)), string.Format("{0:P}",
                GetMedical(dFlight)), string.Format("{0:P}", GetMedical(eFlight)), string.Format("{0:P}", GetMedical(fFlight)));
            Source.Add(medPercentages);

            ReadyPercentages personnelPercentages = new ReadyPercentages("Personnel Overall", string.Format("{0:P}", GetPersonnel(data)), string.Format("{0:P}",
                GetPersonnel(aFlight)), string.Format("{0:P}", GetPersonnel(bFlight)), string.Format("{0:P}", GetPersonnel(cFlight)), string.Format("{0:P}",
                GetPersonnel(dFlight)), string.Format("{0:P}", GetPersonnel(eFlight)), string.Format("{0:P}", GetPersonnel(fFlight)));
            Source.Add(personnelPercentages);

            ReadyPercentages trainingPercentages = new ReadyPercentages("Training Overall", string.Format("{0:P}", GetTraining(data)), string.Format("{0:P}",
                GetTraining(aFlight)), string.Format("{0:P}", GetTraining(bFlight)), string.Format("{0:P}", GetTraining(cFlight)), string.Format("{0:P}",
                GetTraining(dFlight)), string.Format("{0:P}", GetTraining(eFlight)), string.Format("{0:P}", GetTraining(fFlight)));
            Source.Add(trainingPercentages);
        }

        private decimal GetMedical(List<Person> data)
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
            return medicalPercentage;
        }

        private decimal GetPersonnel(List<Person> data)
        {
            decimal personnelPercentage = 0;
            foreach (var item in data)
            {
                if (item.Personnel.OverallStatus == Status.Current || item.Personnel.OverallStatus == Status.Upcoming)
                {
                    personnelPercentage++;
                }
            }
            personnelPercentage = personnelPercentage / data.Count;
            return personnelPercentage;
        }

        private decimal GetTraining(List<Person> data)
        {
            decimal trainingPercentage = 0;
            foreach (var item in data)
            {
                if (item.Training.OverallStatus == Status.Current || item.Training.OverallStatus == Status.Upcoming)
                {
                    trainingPercentage++;
                }
            }
            trainingPercentage = trainingPercentage / data.Count;
            return trainingPercentage;
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
