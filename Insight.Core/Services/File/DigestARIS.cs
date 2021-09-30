using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.File
{
	public class DigestARIS : AbstractDigest, IDigest
	{
		private string _weaponType = "";
		private int _nameIndex = -1;
		private int _organizationIndex;
		private int _completionDateIndex;
		private int _expirationDateIndex;

		private const int Offset = 1; //this offset is to account for the comma in the Name field

		int IDigest.Priority => 3;

		public DigestARIS(IList<string> fileContents, DbContextOptions<InsightContext> dbContextOptions) : base(fileContents, dbContextOptions)
		{

		}

		public void CleanInput()
		{
			//headers found and indexes set for the columns to be digested
			bool headersProcessed = false;

			for (int i = 0; i < FileContents.Count; i++)
			{
				var line = FileContents[i].ToUpper();
				if (!headersProcessed)
				{
					if (line.Contains("HANDGUN"))
					{
						_weaponType = WeaponCourseTypes.Handgun;
					}
					else if (line.Contains("RIFLE/CARBINE"))
					{
						_weaponType = WeaponCourseTypes.Rifle_Carbine;
					}
					else if (!new Regex("^,+$").IsMatch(line) && !line.Contains("PEOPLE ASSIGNED"))
					{
						SetColumnIndexes(FileContents[i].Split(','));
						headersProcessed = true;
					}

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

			//dont forget to add +1 to all other indexes
			_nameIndex = Array.IndexOf(columnHeaders, "NAME");
			_organizationIndex = Array.IndexOf(columnHeaders, "ORGANIZATIONS"); //needs offset
			_completionDateIndex = Array.IndexOf(columnHeaders, "LAST COMPLETION DATE"); //needs offset
			_expirationDateIndex = Array.IndexOf(columnHeaders, "QUAL EXPIRATION DATE"); //needs offset
		}

		public void DigestLines()
		{
			if (string.IsNullOrWhiteSpace(_weaponType)) return;
			if (_completionDateIndex <= -1) return;

			foreach (string line in FileContents)
			{
				var splitLine = line.Split(',').Select(d => d.Trim()).ToArray();

				string firstName = splitLine.ElementAtOrDefault(_nameIndex + 1).Replace("\"", "").Trim();
				string lastName = splitLine.ElementAtOrDefault(_nameIndex).Replace("\"", "").Trim();

				//TODO look for existing person and update if it exists
				var person = insightController.GetPersonByName(firstName, lastName).Result;

				// If you don't find the person (because we value LOXs, throw them out)
				if (person == null) continue;

				if (_completionDateIndex <= -1 || _completionDateIndex > splitLine.Length) return;

				var catmCompletionString = splitLine.ElementAtOrDefault(_completionDateIndex + Offset);
				var catmExperationString = splitLine.ElementAtOrDefault(_expirationDateIndex + Offset);

				if (!string.IsNullOrWhiteSpace(splitLine.ElementAtOrDefault(_completionDateIndex + Offset)) || !string.IsNullOrWhiteSpace(catmCompletionString))
				{
					Course catmCourse = base.GetOrCreateCourse(_weaponType);

					DateTime catmCompletionDate = DateTime.Parse(catmCompletionString);
					DateTime catmExperationDate = !string.IsNullOrWhiteSpace(catmExperationString) ? DateTime.Parse(catmExperationString) :
						catmCompletionDate.AddDays((catmCourse?.Interval ?? 1) * 365);

					CourseInstance courseInstance = new CourseInstance()
					{
						Course = catmCourse,
						Person = person,
						Completion = catmCompletionDate,
						Expiration = catmExperationDate

						// TODO: Make custom expiration by JSON object
						//Expiration = DateTime.Parse(completionDate).AddYears(1)
					};

					insightController.AddCourseInstance(courseInstance, catmCourse, person);
				}
			}
		}
	}
}
