﻿using System;
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
		int IDigest.Priority { get => 3; }

		private IList<string> FileContents = new List<string>();

		InsightController insightController = new InsightController();

		public Course CourseType { get; set; }

		public DigestETMS(IList<string> input)
		{
			CleanInput(input);
		}

		private void CleanInput(IList<string> inputToClean)
		{
			for (var i = 0; i < inputToClean.Count; i++)
			{
				var splitLine = inputToClean[i].Split(',');

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
				var newCourse = new Course()
				{
					Name = courseName
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
				var foundPerson = insightController.GetPersonByName(firstName, lastName);

				CourseInstance courseInstance = new CourseInstance()
				{
					Course = CourseType,
					Person = foundPerson,
					Completion = DateTime.Parse(completionDate),

					// TODO: Make custom expiration by JSON object
					//Expiration = DateTime.Parse(completionDate).AddYears(1)
				};

				insightController.Add(courseInstance, CourseType, foundPerson);

				//courseInstance.Course = CourseType;
				//courseInstance.Person = foundPerson;

				insightController.Update(courseInstance);


			}
		}

	}
}
