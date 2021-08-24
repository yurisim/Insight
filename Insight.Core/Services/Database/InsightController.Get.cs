using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
		/// Returns OrgAlias that matches name
		/// </summary>
		/// <param alias="alias"></param>
		/// <param name="alias"></param>
		/// <returns></returns>
		public Org GetOrgByAlias(string alias)
		{
			var orgs = new List<Org>();

			Org org = null;

			try
			{
				using (var insightContext = new InsightContext(_dbContextOptions))
				{
					orgs = insightContext.OrgAliases
						.Where(x => x.Name == alias.ToUpper())?
						.Select(x => x.Org).ToList();
					//TODO implement exception
					if (orgs.Count > 1)
					{
						throw new Exception("Too many Aliases found, count should not be greater than 1");
					}

					org = orgs.FirstOrDefault();
				}

			}
			//TODO implement exception
			catch (Exception e)
			{
				throw new Exception("Insight.db access error");
			}
			//returns org or null if none exist
			return org;
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
