using Insight.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insight.Core.Helpers
{
   public static class DataCalculation
   {
        public static string GetMedical(List<Person> data)
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

        public static string GetPersonnel(List<Person> data)
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

        public static string GetTraining(List<Person> data)
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
    }
}