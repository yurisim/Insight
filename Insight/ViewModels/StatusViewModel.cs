using Insight.Core.Models;
using Insight.Core.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Insight.Core.Services.Database;
using Windows.Networking;
using System.Linq;
using System;

namespace Insight.ViewModels
{
    public class StatusViewModel : ObservableObject
    {
		// This is a temporary solution for the custom binding. Would be better if we just accept a constructor of objects
        public ObservableCollection<StatusViewItems> Source { get; } = new ObservableCollection<StatusViewItems>();

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
	public class StatusViewItems
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string SSN { get; set; }

		public string CrewPosition { get; set; }

		// This datetime to be nullable to enable null operators for faulty/bad data. 
		public DateTime? DateOnStation { get; set; }
		public DateTime? CyberAwarenessExpiration { get; set; }
		public DateTime? ForceProtectionExpiration { get; set; }
		public DateTime? LawOfWarExpiration { get; set; }
		public DateTime? ReligiousFreedomExpiration { get; set; }
		public DateTime? SABCHandsOnExpiration { get; set; }
		public DateTime? SABCExpiration { get; set; }


	}
}
