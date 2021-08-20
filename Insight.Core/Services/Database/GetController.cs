﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.Database
{
	public partial class InsightController
	{
		/// <summary>
		/// Checks if a course instance already exists in the database. If it does exist, it spits out the course instance with the proper ID.
		/// If it doesn't exist, it spits out null. 
		/// </summary>
		/// <returns></returns>
		public async Task<CourseInstance> GetCourseInstance(CourseInstance instanceToCheck)
		{
			CourseInstance foundInstance;
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					foundInstance = await insightContext.CourseInstances
						.Include(p => p.Person)
						.Include(p => p.Course)
						.FirstOrDefaultAsync(x => x.Person == instanceToCheck.Person &&
									x.Course == instanceToCheck.Course &&
									x.Completion == instanceToCheck.Completion);
				}
			}
			//TODO implement exception
			catch (Exception)
			{
				throw new Exception("Unable to find matching Course Instance");
			}

			//returns person or null if none exist
			return foundInstance;
		}

		public Course GetCourseByName(string courseName)
		{
			// now try to find the course with the name
			Course foundCourse = null;

			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					var foundCourses = insightContext.Courses.Where(course => course.Name == courseName);

					//TODO implement better exceptions
					if (foundCourses.Count() > 1)
					{
						throw new Exception("Too many found, should be null or 1");
					}

					foundCourse = foundCourses.FirstOrDefault();
				}
			}
			//TODO implement exception
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}

			//returns person or null if none exist
			return foundCourse;
		}

		/// <summary>
		/// Generic Get. Use case, var allPersons = GetAll<Person>();
		/// Note that it does not support .Includes by default.
		/// Be wary of subreferences.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public async Task<List<T>> GetAll<T>()
			where T : class
		{
			Task<List<T>> output;
			try
			{
				using (var insightContext = new InsightContext(_dbContextOptions))
				{
					output = insightContext.Set<T>().ToListAsync();
				}
			}

			//TODO implement exception
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return await output;
		}
	}


}
