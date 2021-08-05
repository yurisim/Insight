using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Insight.Core.Helpers;
using Insight.Core.Models;
using Insight.Core.Properties;
using Insight.Core.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.File
{
	public class DigestETMS : AbstractDigest, IDigest
	{
		int IDigest.Priority => 3;

		public DigestETMS(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions) : base(FileContents, dbContextOptions)
		{
			CleanInput(FileContents);
		}
		public Course CourseType { get; set; }


		/// <summary>
		/// Removed duplicate lines in the ETMS Report as well as any other formatting
		/// </summary>
		/// <param name="inputToClean"></param>
		private void CleanInput(IList<string> inputToClean)
		{
			for (var i = 0; i < inputToClean.Count; i++)
			{
				var splitLine = inputToClean[i].Split(',');

				// Remove the lines that have empty courses
				if (string.IsNullOrEmpty(splitLine[4]))
				{
					inputToClean.Remove(inputToClean[i]);
					i--;
				}
			}

			FileContents = inputToClean;
			Debug.WriteLine("hahaha");
		}

		public void DetectETMSType()
		{
			// Use Distinct Column to get the file type in case the first row is blank
			var courseName = FileContents[1].Split(',')[1];

			var foundCourse = insightController.GetCourseByName(courseName);

			// If the course is not found, it will be null, so create the course
			if (foundCourse == null)
			{
				// TODO make custom intervals for each course. Default is hard coded to 1 year
				var newCourse = new Course()
				{
					Name = courseName,
					Interval = 1
				};

				insightController.Add(newCourse);

				CourseType = insightController.GetCourseByName(newCourse.Name);
			}
			else
			{
				CourseType = foundCourse;
			}
		}

		public void DigestLines()
		{
			DetectETMSType();

			// We start at i = 1 so that we ignore the initial schema.
			for (var lineIndex = 1; lineIndex < FileContents.Count; lineIndex++)
			{
				var splitLine = FileContents[lineIndex].Split(',');
				var squadron = splitLine[0];

				var lastName = splitLine[2];
				var firstName = splitLine[3];
				var completionDate = splitLine[4];

				// TODO: Exception if person is not found
				var foundPerson = insightController.GetPersonByName(firstName, lastName, includeSubref: false).Result;

				// TODO: Make this a try parse
				var parsedCompletion = DateTime.Parse(completionDate);

				CourseInstance courseInstance = new CourseInstance()
				{
					Course = CourseType,
					Person = foundPerson,
					Completion = parsedCompletion,
					Expiration = parsedCompletion.AddDays(CourseType.Interval * 365)

					// TODO: Make custom expiration by JSON object
					//Expiration = DateTime.Parse(completionDate).AddYears(1)
				};

				insightController.Add(courseInstance, CourseType, foundPerson);

				//courseInstance.Course = CourseType;
				//courseInstance.Person = foundPerson;

				//InsightController.Update(courseInstance);


			}
		}

	}
}
