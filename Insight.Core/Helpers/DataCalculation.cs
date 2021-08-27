using Insight.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Insight.Core.Helpers
{
	public static class DataCalculation
	{
		/// <summary>
		/// Returns a percentage of people full ready in all catagories. If no persons in passed list, returns all 100%
		/// </summary>
		/// <param name="persons"></param>
		/// <returns></returns>
		public static decimal GetReadinessPercentage(List<Person> persons)
		{
			if (persons.Count == 0) return 1;

			var validStatus = new List<Status>() { Status.Current, Status.Upcoming };

			decimal numValid = persons.Count(p =>
				validStatus.Contains(p.Medical.OverallStatus) &&
				validStatus.Contains(p.Personnel.OverallStatus) &&
				validStatus.Contains(p.Training.OverallStatus));
			return numValid / persons.Count;
		}

		/// <summary>
		/// Returns a typle containing the overall status of Medical, Personnel, Training. If no persons in passed list, returns all 100%
		/// </summary>
		/// <param name="persons"></param>
		/// <returns></returns>
		public static (decimal, decimal, decimal) GetReadinessPerctageByCategory(List<Person> persons)
		{
			if (persons.Count == 0) return (1m, 1m, 1m);

			var validStatus = new List<Status>() { Status.Current, Status.Upcoming };

			decimal numValidMedical = persons.Count(p => validStatus.Contains(p.Medical.OverallStatus));
			decimal numValidPersonnel = persons.Count(p => validStatus.Contains(p.Personnel.OverallStatus));
			decimal numValidTraining = persons.Count(p => validStatus.Contains(p.Training.OverallStatus));

			//return in tuple
			return (numValidMedical / persons.Count, numValidPersonnel / persons.Count, numValidTraining / persons.Count);
		}
	}
}
