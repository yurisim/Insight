using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

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
		[ItemCanBeNull]
		public async Task<T> GetByID<T>(int id) where T : class
		{
			Task<T> output;

			try
			{
				using (var insightContext = new InsightContext(_dbContextOptions))
				{
					output = insightContext.FindAsync<T>(id).AsTask();
				}
			}
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
		[CanBeNull]
		public async Task<List<CourseInstance>> GetCourseInstances(CourseInstance instanceToCheck)
		{
			List<CourseInstance> foundInstances;
			try
			{
				using (var insightContext = new InsightContext(_dbContextOptions))
				{
					foundInstances = await insightContext.CourseInstances
						.Include(p => p.Person)
						.Include(p => p.Course)
						.Where(x => x.Person == instanceToCheck.Person &&
									x.Course == instanceToCheck.Course &&
									x.Completion == instanceToCheck.Completion).ToListAsync();
				}
			}
			//TODO implement exception
			catch (Exception)
			{
				throw new Exception("Unable to find matching Course Instance");
			}

			//returns person or null if none exist
			return foundInstances;
		}

		/// <summary>
		/// Gets course by its name
		/// </summary>
		/// <param name="courseName"></param>
		/// <returns></returns>
		[ItemCanBeNull]
		public async Task<List<Course>> GetCoursesByName(string courseName)
		{
			// now try to find the course with the name
			List<Course> foundCourses = null ;

			try
			{
				using (var insightContext = new InsightContext(_dbContextOptions))
				{
					foundCourses = await insightContext.Courses
						.Where(course => course.Name == courseName)
						.Include(course => course.CourseInstances)
						.ToListAsync();
				}
			}
			//TODO implement exception
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}

			//returns person or null if none exist
			// ReSharper disable once PossibleNullReferenceException
			return foundCourses;
		}

		/// <summary>
		/// Returns OrgAlias that matches name
		/// </summary>
		/// <param alias="alias"></param>
		/// <param name="alias"></param>
		/// <returns></returns>
		[ItemCanBeNull]
		public async Task<List<Org>> GetOrgsByAlias(string alias)
		{
			List<Org> orgs;

			try
			{
				using (var insightContext = new InsightContext(_dbContextOptions))
				{
					orgs = await insightContext.OrgAliases
						.Where(x => x.Name == alias.ToUpper())
						.Select(x => x.Org).ToListAsync();
				}
			}
			//TODO implement exception
			catch (Exception e)
			{
				throw new Exception("Insight.db access error");
			}

			//returns org or null if none exist
			return orgs;
		}

		/// <summary>
		/// Gets AFSC by name
		/// </summary>
		/// <param name="pafsc"></param>
		/// <returns></returns>
		public async Task<List<AFSC>> GetAFSCs(string pafsc)
		{
			//TODO search by any of p/c/d afsc
			List<AFSC> afscs = null;
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					afscs = await insightContext.AFSCs.Where(x => x.PAFSC == pafsc.ToUpper()).ToListAsync();

				}
			}
			catch (Exception e)
			{
				throw new Exception("Insight.db access error");
			}

			return afscs;
		}
	}
}
