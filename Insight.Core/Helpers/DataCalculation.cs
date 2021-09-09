using Insight.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Insight.Core.Helpers
{
	public static class DataCalculation
	{
		/// <summary>
		/// Returns a percentage of people full ready in all catagories. If no persons in passed list, returns all 100%
		/// </summary>
		/// <param name="persons"></param>
		/// <returns></returns>
		public static decimal GetReadinessPercentage(IList<Person> persons)
		{
			//return 1 if list is null or empty
			if (persons?.Any() != true) return 1;

			var validStatus = new List<Status>() { Status.Current, Status.Upcoming };

			decimal numValid = persons.Count(p =>
				validStatus.Contains((p.Medical?.OverallStatus).GetValueOrDefault()) &&
				validStatus.Contains((p.Personnel?.OverallStatus).GetValueOrDefault()) &&
				validStatus.Contains((p.Training?.OverallStatus).GetValueOrDefault()));
			return numValid / persons.Count;
		}

		/// <summary>
		/// Returns a typle containing the overall status of Medical, Personnel, Training. If no persons in passed list, returns all 100%
		/// </summary>
		/// <param name="persons"></param>
		/// <returns></returns>
		public static (decimal, decimal, decimal) GetReadinessPerctageByCategory(IList<Person> persons)
		{
			//return if list is null or empty
			if (persons?.Any() != true) return (1m, 1m, 1m);

			var validStatus = new List<Status>() { Status.Current, Status.Upcoming };

			decimal numValidMedical = persons.Count(p => validStatus.Contains(p.Medical.OverallStatus));
			decimal numValidPersonnel = persons.Count(p => validStatus.Contains(p.Personnel.OverallStatus));
			decimal numValidTraining = persons.Count(p => validStatus.Contains(p.Training.OverallStatus));

			//return in tuple
			return (numValidMedical / persons.Count, numValidPersonnel / persons.Count, numValidTraining / persons.Count);
		}
	}
}
