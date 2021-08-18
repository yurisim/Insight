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
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.File
{
	public class DigestETMS : AbstractDigest, IDigest
	{
		private int _pasDescriptionIndex = -1;
		private int _courseTitleIndex = -1;
		private int _lastNameIndex = -1;
		private int _firstNameIndex = -1;
		private int _completionDateIndex = -1;

		int IDigest.Priority => 3;

		public DigestETMS(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions) : base(FileContents, dbContextOptions)
		{

		}

		/// <summary>
		/// Removed duplicate lines in the ETMS Report as well as any other formatting
		/// </summary>
		public void CleanInput()
		{
			//headers found and indexes set for the columns to be digested
			bool headersProcessed = false;

			for (var i = 0; i < FileContents.Count; i++)
			{
				string[] splitLine = FileContents[i].Split(',');
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

		/// <summary>
		/// Sets the indexes for columns of data that needs to be digested
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

		/// <summary>
		/// Determines which course on ETMS FileContents is for
		/// </summary>
		private Course CreateCourse(string courseName)
		{
			var foundCourse = insightController.GetCourseByName(courseName);

			// If the course is not found, it will be null, so create the course
			if (foundCourse == null)
			{
				// TODO make custom intervals for each course. Default is hard coded to 1 year
				Course newCourse = new Course()
				{
					Name = courseName,
					Interval = 1
				};

				insightController.Add(newCourse);

				foundCourse = newCourse;
			}
			return foundCourse;
		}

		public void DigestLines()
		{
			string courseName = FileContents[1].Split(',')[_courseTitleIndex];
			Course course = CreateCourse(courseName);

			if (course == null)
			{
				return;
			}

			for (int i = 0; i < FileContents.Count; i++)
			{
				var splitLine = FileContents[i].Split(',').Select(d => d.Trim()).ToArray();

				string squadron = splitLine[_pasDescriptionIndex];

				string firstName = splitLine[_firstNameIndex];
				string lastName = splitLine[_lastNameIndex];
				
				string completionDate = splitLine[_completionDateIndex];

				// TODO: Exception if person is not found
				var foundPerson = insightController.GetPersonByName(firstName, lastName, includeSubref: true).Result;

				if (foundPerson == null)
				{
					continue;
				}
				//if (foundPerson.CourseInstances == null)
				//{
				//	foundPerson.CourseInstances = new List<CourseInstance>();
				//	insightController.Update(foundPerson);
				//}

				// TODO: Make this a try parse
				var parsedCompletion = DateTime.Parse(completionDate);

				CourseInstance courseInstance = new CourseInstance()
				{
					Course = course,
					Person = foundPerson,
					Completion = parsedCompletion,
					Expiration = parsedCompletion.AddDays(course.Interval * 365)

					// TODO: Make custom expiration by JSON object
					//Expiration = DateTime.Parse(completionDate).AddYears(1)
				};

				insightController.AddCourseInstance(courseInstance, course, foundPerson);

				//courseInstance.Course = CourseType;
				//courseInstance.Person = foundPerson;

				//InsightController.Update(courseInstance);
			}
		}
	}
}
