using System;
using System.Collections.Generic;
using System.Text;
using Insight.Core.Helpers;
using Insight.Core.Models;
using Insight.Core.Services.Database;

namespace Insight.Core.Services.FileProcessors
{
	public class DigestAEF : IDigest
	{
		private readonly IList<string> File = new List<string>();

		public DigestAEF(IList<string> input)
		{
			this.File = input;
		}

		/// <summary>
		/// loops AEF generated string list of lines and processes them
		/// </summary>
		/// <param name="File"></param>
		public void DigestLines()
		{
			for (int i = 3; i < File.Count - 1; i++)
			{
				string[] data = File[i].Split(',');

				//TODO refact to better handle format changes
				//Check variables
				var name = data[0].Split(' ');
				string unit = data[5];
				string AFSC = data[7];

				Person person = InsightController.GetPersonByName(firstName: name[1].Trim(), lastName: name[0].Trim());

				//TODO handle user existing in AEF but not in alpha roster
				if (person == null)
				{
					continue;
				}

				//PERSONNEL
				if (person.Personnel == null)
				{
					person.Personnel = new Personnel();
				}
				person.Personnel.OverallStatus = StringManipulation.StatusReader(data[10]);

				//MEDICAL
				if (person.Medical == null)
				{
					person.Medical = new Medical();
				}
				person.Medical.OverallStatus = StringManipulation.StatusReader(data[11]);

				//TRAINING
				if (person.Training == null)
				{
					person.Training = new Training();
				}
				person.Training.OverallStatus = StringManipulation.StatusReader(data[12]);

				InsightController.Update(person);
			}
		}
	}
}
