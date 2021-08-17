using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Insight.Core.Helpers;
using Insight.Core.Models;
using Insight.Core.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.File
{
	public class DigestAEF : AbstractDigest, IDigest
	{
		private int _nameIndex = -1;
		private int _payGradeIndex = -1;
		private int _unitIndex = -1;
		private int _afscIndex = -1;
		private int _personnelOverallStatusIndex = -1;
		private int _medicalOverallStatusIndex = -1;
		private int _trainingOverallStatusIndex = -1;

		int IDigest.Priority { get => 2; }

		public DigestAEF(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions) : base(FileContents, dbContextOptions)
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
					if(!lineToUpper.Contains("EXPORT DESCRIPTION: "))
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
		private void SetColumnIndexes(string[] columnHeaders)
		{
			//Converts everything to upper case for comparison
			columnHeaders = columnHeaders.Select(d => d.ToUpper().Trim()).ToArray();

			_nameIndex = Array.IndexOf(columnHeaders, "NAME");
			_payGradeIndex = Array.IndexOf(columnHeaders, "PAYGRADE");
			_unitIndex = Array.IndexOf(columnHeaders, "UNIT");
			_afscIndex = Array.IndexOf(columnHeaders, "AFSC");
			_personnelOverallStatusIndex = Array.IndexOf(columnHeaders, "PERSONNEL");
			_medicalOverallStatusIndex = Array.IndexOf(columnHeaders, "MEDICAL");
			_trainingOverallStatusIndex = Array.IndexOf(columnHeaders, "TRAINING");
		}

		/// <summary>
		/// loops AEF generated string list of lines and processes them
		/// </summary>
		/// <param name="File"></param>
		public void DigestLines()
		{
			for (int i = 0; i < FileContents.Count; i++)
			{
				string[] splitLine = FileContents[i].Split(',');

				//TODO refact to better handle format changes
				//Check variables
				string[] names = splitLine[_nameIndex].Split(' ').Select(x => x.ToUpperInvariant().Trim()).ToArray();
				string firstName = names[1];
				string lastName = names[0];
				string unit = splitLine[_unitIndex];
				string AFSC = splitLine[_afscIndex];
				Status personnelStatus = StringManipulation.StatusReader(splitLine[_personnelOverallStatusIndex]);
				Status medicalStatus = StringManipulation.StatusReader(splitLine[_medicalOverallStatusIndex]);
				Status trainingStatus = StringManipulation.StatusReader(splitLine[_trainingOverallStatusIndex]);

				Person person = insightController.GetPersonByName(firstName: firstName, lastName: lastName).Result;

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
