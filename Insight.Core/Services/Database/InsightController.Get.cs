using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Insight.Core.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.Database
{
	public partial class InsightController
	{

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

		/// <summary>
		/// Generic get by id
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<T> GetByID<T>(int id)
			where T : class
		{
			Task<T> output;
			try
			{
				using (var insightContext = new InsightContext(_dbContextOptions))
				{
					output = insightContext.FindAsync<T>(id).AsTask();
				}
			}

			//TODO implement exception
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return await output;
		}

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

		[ItemCanBeNull]
		public async Task<Course> GetCourseByName(string courseName)
		{
			// now try to find the course with the name
			Task<Course> foundCourse;

			try
			{
				using (var insightContext = new InsightContext(_dbContextOptions))
				{
					foundCourse = insightContext.Courses
						.Where(course => course.Name == courseName)
						.Include(course => course.CourseInstances)
						.FirstOrDefaultAsync();
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
		/// Returns OrgAlias that matches name
		/// </summary>
		/// <param alias="alias"></param>
		/// <param name="alias"></param>
		/// <returns></returns>
		[ItemCanBeNull]
		public async Task<Org> GetOrgByAlias(string alias)
		{
			Task<Org> org;

			try
			{
				using (var insightContext = new InsightContext(_dbContextOptions))
				{
					org = insightContext.OrgAliases
						.Where(x => x.Name == alias.ToUpper())
						.Select(x => x.Org).FirstOrDefaultAsync();
				}
			}
			//TODO implement exception
			catch (Exception e)
			{
				throw new Exception("Insight.db access error");
			}

			//returns org or null if none exist
			return await org;
		}

		/// <summary>
		/// Gets AFSC by name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public async Task<AFSC> GetAFSC(string pafsc)
		{
			//TODO search by any of p/c/d afsc
			AFSC afsc = null;
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					afsc = await insightContext.AFSCs.FirstOrDefaultAsync(x => x.PAFSC == pafsc.ToUpper());

				}
			}
			catch (Exception e)
			{
				throw new Exception("Insight.db access error");
			}

			return afsc;
		}
	}
}
