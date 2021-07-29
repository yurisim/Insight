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

namespace Insight.Core.Services.File
{
	public class DigestETMS : IDigest
	{
		int IDigest.Priority => 3;

		/// <summary>
		/// This stores the file contents after they have been cleaned in the constructor
		/// </summary>
		private IList<string> FileContents;


		/// <summary>
		/// The course that the ETMS document represents, each ETMS document is a diff course
		/// </summary>
		public Course CourseType { get; set; }

		/// <summary>
		/// Constructor for ETMS, cleans the input of ETMS to clear up duplicate records, fix data to be readable
		/// </summary>
		/// <param name="input"></param>
		public DigestETMS(IList<string> input)
		{
			CleanInput(input);
		}

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

			var foundCourse = InsightController.GetCourseByName(courseName);

			// If the course is not found, it will be null, so create the course
			if (foundCourse == null)
			{
				var newCourse = new Course()
				{
					Name = courseName
				};

				InsightController.Add(newCourse);

				CourseType = InsightController.GetCourseByName(newCourse.Name);
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
				var foundPerson = InsightController.GetPersonByName(firstName, lastName, includeSubref: false);

				CourseInstance courseInstance = new CourseInstance()
				{
					Course = CourseType,
					Person = foundPerson,
					Completion = DateTime.Parse(completionDate),

					// TODO: Make custom expiration by JSON object
					//Expiration = DateTime.Parse(completionDate).AddYears(1)
				};

				InsightController.Add(courseInstance, CourseType, foundPerson);

				//courseInstance.Course = CourseType;
				//courseInstance.Person = foundPerson;

				//InsightController.Update(courseInstance);


			}
		}

	}
}
