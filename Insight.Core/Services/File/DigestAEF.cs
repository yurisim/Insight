using System;
using System.Collections.Generic;
using System.Linq;
using Insight.Core.Helpers;
using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.File
{
	public class DigestAEF : AbstractDigest, IDigest
	{
		private int NameIndex = -1;
		private int UnitIndex = -1;
		private int PersonnelOverallStatusIndex = -1;
		private int MedicalOverallStatusIndex = -1;
		private int TrainingOverallStatusIndex = -1;

		int IDigest.Priority => 2;

		public DigestAEF(IList<string> fileContents, DbContextOptions<InsightContext> dbContextOptions) : base(fileContents, dbContextOptions)
		{

		}

		public void CleanInput()
		{
			//headers found and indexes set for the columns to be digested
			bool headersProcessed = false;

			for (int i = 0; i < FileContents.Count; i++)
			{
				string[] splitLine = FileContents[i].Split(',');
				string lineToUpper = FileContents[i].ToUpper();
				//finds and removes any FOUO/CUI warning
				if (lineToUpper.Contains("FOR OFFICIAL USE ONLY") || lineToUpper.Contains("CONTROLLED UNCLASSIFIED INFORMATION"))
				{
					FileContents.RemoveAt(i);
					i--;
				}
				//headersProcessed is false as long as we're still in the top portion of the file, before the person data
				else if (!headersProcessed)
				{
					//FOUO/CUI warnings have been removed, checks that the current line is not the export description.
					//If all of those things have been eliminated, it must be the column header's line
					if (!lineToUpper.Contains("EXPORT DESCRIPTION: "))
					{
						SetColumnIndexes(splitLine);
						headersProcessed = true;
					}
					//removes everything up to and including column headers
					FileContents.RemoveAt(i);
					i--;
				}
			}
		}

		/// <summary>
		/// Sets the indexes for columns of data that needs to be digested
		/// </summary>
		/// <param name="columnHeaders">Represents the row of headers for data columns</param>
		private
			void SetColumnIndexes(string[] columnHeaders)
		{
			//Converts everything to upper case for comparison
			columnHeaders = columnHeaders.Select(d => d.ToUpper().Trim()).ToArray();

			NameIndex = Array.IndexOf(columnHeaders, "NAME");
			UnitIndex = Array.IndexOf(columnHeaders, "UNIT");
			PersonnelOverallStatusIndex = Array.IndexOf(columnHeaders, "PERSONNEL");
			MedicalOverallStatusIndex = Array.IndexOf(columnHeaders, "MEDICAL");
			TrainingOverallStatusIndex = Array.IndexOf(columnHeaders, "TRAINING");
		}

		/// <summary>
		/// loops AEF generated string list of lines and processes them
		/// </summary>
		/// <param name="File"></param>
		public void DigestLines()
		{
			foreach (string line in FileContents)
			{
				var splitLine = line.Split(',').Select(d => d.Trim()).ToArray();

				//TODO refact to better handle format changes
				//Check variables
				string[] names = splitLine[NameIndex].Split(' ').Select(x => x.Trim()).ToArray();
				string firstName = names[1];
				string lastName = names[0];
				//string unit = splitLine[UnitIndex];
				//string AFSC = splitLine[AFSCIndex];
				Status personnelStatus = StringManipulation.StatusReader(splitLine[PersonnelOverallStatusIndex]);
				Status medicalStatus = StringManipulation.StatusReader(splitLine[MedicalOverallStatusIndex]);
				Status trainingStatus = StringManipulation.StatusReader(splitLine[TrainingOverallStatusIndex]);

				//TODO handle picking which person in front end
				Person person = insightController.GetPersonsByName(firstName: firstName, lastName: lastName).Result.FirstOrDefault();

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
				person.Personnel.OverallStatus = personnelStatus;

				//MEDICAL
				if (person.Medical == null)
				{
					person.Medical = new Medical();
				}
				person.Medical.OverallStatus = medicalStatus;

				//TRAINING
				if (person.Training == null)
				{
					person.Training = new Training();
				}
				person.Training.OverallStatus = trainingStatus;

				insightController.Update(person);
			}
		}
	}
}
