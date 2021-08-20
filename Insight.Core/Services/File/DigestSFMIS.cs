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
	public class DigestSFMIS : AbstractDigest, IDigest
	{
		private int _emailIndex;
		private int _catmCourseNameIndex;
		private int _catmCompletionDateIndex;
		private int _catmExperationDateIndex;

		int IDigest.Priority { get => 5; }

		public DigestSFMIS(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions) : base(FileContents, dbContextOptions)
		{

		}

		public void CleanInput()
		{
			//headers found and indexes set for the columns to be digested
			bool headersProcessed = false;

			for (int i = 0; i < FileContents.Count; i++)
			{
				string lineToUpper = FileContents[i].ToUpper();
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
						string[] splitLine = FileContents[i].Split(',');
						SetColumnIndexes(splitLine);
						headersProcessed = true;
					}
					//removes everything up to and including column headers
					FileContents.RemoveAt(i);
					i--;
				}
				else
				{
					//replaces commas with another character so that split(',') doesn't split it
					FileContents[i] = FileContents[i].Replace("Survival, Evasion, Resistance, Escape", "Survival~ Evasion~ Resistance~ Escape");
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
			_emailIndex = Array.IndexOf(columnHeaders, "EMAIL4CAREER");
			_catmCourseNameIndex = Array.IndexOf(columnHeaders, "COURSE");
			_catmCompletionDateIndex = Array.IndexOf(columnHeaders, "COMPLETION_DATE");
			_catmExperationDateIndex = Array.IndexOf(columnHeaders, "EXPIRE_DATE");
		}

		public void DigestLines()
		{
			for (int i = 0; i < FileContents.Count; i++)
			{
				var splitLine = FileContents[i].Split(',').Select(d => d.Trim()).ToArray();


				if (string.IsNullOrWhiteSpace(splitLine[_emailIndex]))
				{
					//if name is not valid, can't find associated person
					//option is to try name optionally, but the fomatting is less than optimal
					continue;
				}

				//name is extracted out of email due to the ambigious formatting of the name column in the source data and because it truncates it at 18 characters
				string[] names = splitLine[_emailIndex].Substring(0, splitLine[_emailIndex].IndexOf("@")).Split('.');

				string firstName = names[0];
				string lastName = names[1].Replace("_", "-");
				
				Person person = insightController.GetPersonByName(firstName, lastName).Result;

				if(person == null)
				{
					//TODO handle null person
				}
				else
				{
					person.Email = splitLine[_emailIndex];
					insightController.Update(person);

					//CATM course is not empty
					if(splitLine[_catmCourseNameIndex] != "")
					{
						Course catmCourse = base.GetOrCreateCourse(splitLine[_catmCourseNameIndex]);
						DateTime catmCompletionDate = DateTime.Parse(splitLine[_catmCompletionDateIndex]);
						DateTime catmExperationDate = DateTime.Parse(splitLine[_catmExperationDateIndex]);

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
}
