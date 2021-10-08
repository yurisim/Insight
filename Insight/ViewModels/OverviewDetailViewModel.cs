using Insight.Core.Helpers;
using Insight.Core.Models;
using Insight.Core.Services.Database;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace Insight.ViewModels
{
	public class OverviewDetailViewModel : ObservableObject
	{
		/// <summary>
		/// Holds the data for the front end table
		/// </summary>
		public ObservableCollection<ReadyPercentages> Source { get; } = new ObservableCollection<ReadyPercentages>();

		/// <summary>
		/// Number of people in the Squadron
		/// </summary>
		public string TotalPersons;

		/// <summary>
		/// Percentage of people fully ready
		/// </summary>
		public string OverallReadiness;

		/// <summary>
		/// Holds the organization name
		/// </summary>
		public string PageOrg { get; set; }

		public OverviewDetailViewModel(string org)
		{
			PageOrg = org;
		}

		/// <summary>
		/// puts the people into flights, calculates the unit's overall readiness based on medical, training, personnel data
		/// </summary>
		/// <returns>does not return anything, allows the program to keep track of the async method</returns>
		public async Task LoadDataAsync()
		{
			Source.Clear();

			InsightController insightController = new InsightController();
			List<Person> persons = await insightController.GetAllPersons(insightController.GetOrgsByAlias(PageOrg).Result.FirstOrDefault());

			List<string> allFlightNames = new List<string>();

			List<List<Person>> allFlights = new List<List<Person>>();
			TotalPersons = persons.Count.ToString();
			var num = DataCalculation.GetReadinessPercentage(persons);
			OverallReadiness = string.Format("{0:P}", DataCalculation.GetReadinessPercentage(persons));
			foreach (var person in persons)
			{
				if (!allFlightNames.Contains(person.Flight.ToUpper()))
				{
					allFlightNames.Add(person.Flight.ToUpper());
					List<Person> newFlight = new List<Person>();
					allFlights.Add(newFlight);
				}
				//The index of the flight it is trying to access
				int flightNameIndex = allFlightNames.IndexOf(person.Flight.ToUpper());
				allFlights[flightNameIndex].Add(person);
			}

			var (medicalPercent, PersonnelPercent, TrainingPercent) = DataCalculation.GetReadinessPerctageByCategory(persons);

			//TODO When orgs are implemented make more dynamic
			ReadyPercentages OverallPercentages = new ReadyPercentages(PageOrg + " Overall", medicalPercent, PersonnelPercent, TrainingPercent);

			Source.Add(OverallPercentages);

			foreach (var flightName in allFlightNames)
			{
				//The flight names are in a seperate List so this gets the people in each flight via a syncronized index
				Source.Add(FlightPercentageBuilder(flightName, allFlights[allFlightNames.IndexOf(flightName)]));
			}
		}

		/// <summary>
		/// puts the flight's Medical, Personnel, and Training data into a ReadyPercentages object
		/// </summary>
		/// <param name="flight">The name of the Flight</param>
		/// <param name="persons">The list of people in the flight</param>
		/// <returns>ReadyPercentages object which holds the input data</returns>
		private ReadyPercentages FlightPercentageBuilder(string flight, List<Person> persons)
		{
			var (medicalPercent, PersonnelPercent, TrainingPercent) = DataCalculation.GetReadinessPerctageByCategory(persons);
			return new ReadyPercentages($"{flight} Flight", medicalPercent, PersonnelPercent, TrainingPercent);
		}

		/// <summary>
		/// Organizes the information into rows by the organization
		/// </summary>
		public struct ReadyPercentages
		{
			public string Organization { get; }
			public string Medical { get; }
			public string Personnel { get; }
			public string Training { get; }

			public ReadyPercentages(string rowName, decimal valueMed, decimal valuePersonnel, decimal valueTraining)
			{
				Organization = rowName;
				Medical = string.Format("{0:P}", valueMed);
				Personnel = string.Format("{0:P}", valuePersonnel);
				Training = string.Format("{0:P}", valueTraining);
			}
		}
	}
}
