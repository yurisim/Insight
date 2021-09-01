using Insight.Core.Models;
using Insight.Core.Services.Database;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Insight.ViewModels
{
	public class StatusViewModel : ObservableObject
	{
		// This is a temporary solution for the custom binding. Would be better if we just accept a constructor of objects
		public ObservableCollection<StatusViewItems> Source { get; set; } = new ObservableCollection<StatusViewItems>();

		public StatusViewModel()
		{
		}

		/// <summary>
		/// Loads person objects into the status table whenever the page loads. This is called in the code behind of the object.
		/// See references.
		/// </summary>
		/// <returns></returns>
		public async Task LoadDataAsync()
		{
			Source.Clear();

			InsightController controller = new InsightController();

			// Get the person objects needed
			var peopleToProcess = await controller.GetAllPersons();

			var peopleToDisplay = peopleToProcess.Select(person => new StatusViewItems
			{
				Id = person.Id,
				Name = person.Name,
				SSN = person.SSN,
				CrewPosition = person.CrewPosition,
				DeploymentStatus = person.DeploymentStatus,
				DateOnStation = person.DateOnStation,
				// Need ?. null checks in case data is bad (it is)
				CyberAwarenessExpiration = person.CourseInstances.FirstOrDefault(coursePersonTook => coursePersonTook.Course.Name == "TFAT - Cyber Awareness Challenge")?.Expiration,
				ForceProtectionExpiration = person.CourseInstances.FirstOrDefault(coursePersonTook => coursePersonTook.Course.Name == "Force Protection")?.Expiration,
				LawOfWarExpiration = person.CourseInstances.FirstOrDefault(coursePersonTook => coursePersonTook.Course.Name == "Law of War (LoW) - Basic")?.Expiration,
				ReligiousFreedomExpiration = person.CourseInstances.FirstOrDefault(coursePersonTook => coursePersonTook.Course.Name == "Religious Freedom")?.Expiration,
				SABCHandsOnExpiration = person.CourseInstances.FirstOrDefault(coursePersonTook => coursePersonTook.Course.Name == "Self Aid & Buddy Care Hands On")?.Expiration,
				SABCExpiration = person.CourseInstances.FirstOrDefault(coursePersonTook => coursePersonTook.Course.Name == "Self Aid & Buddy Care (SABC)")?.Expiration,
			});

			foreach (var person in peopleToDisplay)
			{
				Source.Add(person);
			}
		}
	}

	/// <summary>
	/// Temporary stop gap. Would prefer to use a dynamic "value tuple" instead to display the items in the table. Because this is only used in this view, I stuck it here.
	/// The benefits of a value tuple means we don't have to make a new class for every different type of display.
	/// </summary>
	public class StatusViewItems : INotifyPropertyChanged
	{
		private DeploymentStatus _deploymentStatus;

		public int Id { get; set; }

		public string Name { get; set; }

		public string SSN { get; set; }

		public string CrewPosition { get; set; }

		public DeploymentStatus DeploymentStatus
		{
			get => _deploymentStatus;
			set
			{
				if (value != _deploymentStatus)
				{
					_deploymentStatus = value;
					this.OnPropertyChanged(nameof(DeploymentStatus));
				}
			}
		}

		// This datetime to be nullable to enable null operators for faulty/bad data. 
		public DateTime? DateOnStation { get; set; }
		public DateTime? CyberAwarenessExpiration { get; set; }
		public DateTime? ForceProtectionExpiration { get; set; }
		public DateTime? LawOfWarExpiration { get; set; }
		public DateTime? ReligiousFreedomExpiration { get; set; }
		public DateTime? SABCHandsOnExpiration { get; set; }
		public DateTime? SABCExpiration { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		// This method MUST BE called by the Set accessor of each property for TwoWay binding to work.
		// The CallerMemberName attribute that is applied to the optional propertyName
		// parameter causes the property name of the caller to be substituted as an argument.
		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				InsightController insightController = new InsightController();
				switch (propertyName)
				{
					//piggy back on this case statement for any Person property
					case nameof(DeploymentStatus):
						//get obj by id
						var person = insightController.GetByID<Person>(this.Id).Result;

						//get property of obj that was changed by user
						PropertyInfo destinationProperty = person.GetType().GetProperty(propertyName);

						//get property that needs to be updated in database
						PropertyInfo sourceProperty = this.GetType().GetProperty(propertyName);

						//update value and save to db
						destinationProperty.SetValue(person, sourceProperty.GetValue(this), null);
						insightController.Update(person);
						break;

					default:

						break;
				}
			}
		}
	}
}
