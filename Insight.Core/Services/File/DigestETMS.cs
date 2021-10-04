using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Insight.Core.Services.File
{
	public class DigestETMS : AbstractDigest, IDigest
	{
		private int _completionDateIndex = -1;
		private int _courseTitleIndex = -1;
		private int _firstNameIndex = -1;
		private int _lastNameIndex = -1;
		private int _pasDescriptionIndex = -1;

		public DigestETMS(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions) : base(
			FileContents, dbContextOptions)
		{
		}

		int IDigest.Priority => 3;

		/// <summary>
		///     Removed duplicate lines in the ETMS Report as well as any other formatting
		/// </summary>
		public void CleanInput()
		{
			//headers found and indexes set for the columns to be digested
			var headersProcessed = false;

			for (var i = 0; i < FileContents.Count; i++)
			{
				// why not start i = 1 instead of removign header?
				var splitLine = FileContents[i].Split(',');
				if (!headersProcessed)
				{
					SetColumnIndexes(splitLine);
					headersProcessed = true;
					FileContents.RemoveAt(i);
					i--;
				}
				// Remove the lines that have empty courses
				else if (string.IsNullOrEmpty(splitLine[_completionDateIndex]))
				{
					FileContents.RemoveAt(i);
					i--;
				}
			}
		}

		public void DigestLines()
		{
			//return if contents is empty
			if (FileContents.Count == 0) return;

			string courseName = FileContents[0].Split(',')[_courseTitleIndex];
			Course course = base.GetOrCreateCourse(courseName);

			// Parallel.ForEach(FileContents, t =>
			foreach (string line in FileContents)
			{
				string[] splitLine = line.Split(',').Select(d => d.Trim()).ToArray();
				string squadron = splitLine[_pasDescriptionIndex].ToUpper();

				string firstName = splitLine[_firstNameIndex];
				string lastName = splitLine[_lastNameIndex];
				string completionDate = splitLine[_completionDateIndex];

				//TODO handle picking which person in the frontend
				var foundPerson = insightController.GetPersonsByName(firstName, lastName, true).Result.FirstOrDefault();

				// TODO: Exception if person is not found
				if (foundPerson == null) continue;

				//if (foundPerson.CourseInstances == null)
				//{
				//	foundPerson.CourseInstances = new List<CourseInstance>();
				//	insightController.Update(foundPerson);
				//}
				//	foundPerson.CourseInstances = new List<CourseInstance>();
				//	insightController.Update(foundPerson);
				//}

				// TODO: Make this a try parse
				var parsedCompletion = DateTime.Parse(completionDate);

				var courseInstance = new CourseInstance
				{
					Course = course,
					Person = foundPerson,
					Completion = parsedCompletion,
					Expiration = parsedCompletion.AddDays(course.Interval * 365)

					// TODO: Make custom expiration by JSON object
				};

				// check if the course instance exists
				//TODO handle picking which one in frontend
				var foundInstance = insightController.GetCourseInstances(courseInstance).Result.FirstOrDefault();

				// if no instance is found
				if (foundInstance == null)
				{
					insightController.AddCourseInstance(courseInstance, course, foundPerson);
				}
			}
		}

		/// <summary>
		///     Sets the indexes for columns of data that needs to be digested
		/// </summary>
		/// <param name="columnHeaders">Represents the row of headers for data columns</param>
		private void SetColumnIndexes(string[] columnHeaders)
		{
			//Converts everything to upper case for comparison
			columnHeaders = columnHeaders.Select(d => d.ToUpper().Trim()).ToArray();

			_pasDescriptionIndex = Array.IndexOf(columnHeaders, "PAS DESCRIPTION");
			_courseTitleIndex = Array.IndexOf(columnHeaders, "COURSE TITLE");
			_lastNameIndex = Array.IndexOf(columnHeaders, "LAST NAME");
			_firstNameIndex = Array.IndexOf(columnHeaders, "FIRST NAME");
			_completionDateIndex = Array.IndexOf(columnHeaders, "COMPLETION DATE");
		}

		private T CreateOrReturn<T, T1>(string courseName, ref T1 entity)
		{
			var foundEntity = insightController.GetCoursesByName(courseName);

			return default;
		}
	}
}
