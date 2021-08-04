using Insight.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insight.Core.Helpers
{
   public static class DataCalculation
   {
		/// <summary>
		/// Calculates percentage of people ready with medical
		/// </summary>
		/// <param name="data">the group you want to check</param>
		/// <returns></returns>
        public static string GetMedical(List<Person> data)
        {
            string medicalPercentageOutput = "Unknown";
            if (data.Count != 0)
            {
                decimal medicalPercentage = 0;
                foreach (var item in data)
                {
                    if (item.Medical?.OverallStatus == Status.Current || item.Medical?.OverallStatus == Status.Upcoming)
                    {
                        medicalPercentage++;
                    }
                }
                medicalPercentage /= data.Count;
                medicalPercentageOutput = string.Format("{0:P}", medicalPercentage);
            }
            return medicalPercentageOutput;
        }
		/// <summary>
		/// Calculates percentage of people ready with personnel items
		/// </summary>
		/// <param name="data">the group you want to check</param>
		/// <returns></returns>
		public static string GetPersonnel(List<Person> data)
        {
            string personnelPercentageOutput = "Unknown";
            if (data.Count != 0)
            {
                decimal personnelPercentage = 0;
                foreach (var item in data)
                {
                    if (item.Personnel?.OverallStatus == Status.Current || item.Personnel?.OverallStatus == Status.Upcoming)
                    {
                        personnelPercentage++;
                    }
                }
                personnelPercentage /= data.Count;
                personnelPercentageOutput = string.Format("{0:P}", personnelPercentage);
            }
            return personnelPercentageOutput;
        }
		/// <summary>
		/// Calculates percentage of people ready with training
		/// </summary>
		/// <param name="data">the group you want to check</param>
		/// <returns></returns>
		public static string GetTraining(List<Person> data)
        {
            string trainingPercentageOutput = "Unknown";
            if (data.Count != 0)
            {
                decimal trainingPercentage = 0;
                foreach (var item in data)
                {
                    if (item.Training?.OverallStatus == Status.Current || item.Training?.OverallStatus == Status.Upcoming)
                    {
                        trainingPercentage++;
                    }
                }
                trainingPercentage /= data.Count;
                trainingPercentageOutput = string.Format("{0:P}", trainingPercentage);
            }
            return trainingPercentageOutput;
        }
		/// <summary>
		/// Calculates percentage of people ready with training, personnel items, and medical
		/// </summary>
		/// <param name="data">the group you want to check</param>
		/// <returns></returns>
		public static string GetOverall(List<Person> data)
		{
			string overallPercentageOutput = "Unknown";
			if (data.Count != 0)
			{
				decimal overallPercentage = 0;
				foreach (var item in data)
				{
					if ((item.Training?.OverallStatus == Status.Current || item.Training?.OverallStatus == Status.Upcoming) && (item.Personnel?.OverallStatus == Status.Current || item.Personnel?.OverallStatus == Status.Upcoming) &&
						(item.Medical?.OverallStatus == Status.Current || item.Medical?.OverallStatus == Status.Upcoming))
					{
						overallPercentage++;
					}
				}
				overallPercentage /= data.Count;
				overallPercentageOutput = string.Format("{0:P}", overallPercentage);
			}
			return overallPercentageOutput;
		}
    }
}
