using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Insight.Core.Helpers;

namespace Insight.Core.Services.Database
{
	public partial class InsightController
	{
		/// <summary>
		/// Returns all Person objects from database
		/// TODO :// Take off the includes and make other methods to handle specific cases
		/// </summary>
		/// <returns></returns>
		public async Task<List<Person>> GetAllPersons()
		{
			var foundPersons = new List<Person>();

			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					foundPersons = await insightContext.Persons
						.Include(person => person.Medical)
						.Include(person => person.Personnel)
						.Include(person => person.Training)
						.Include(person => person.Organization)
						// Maybe have an overload because their course instances are big and we may not need to ever call these except for specific instances?
						.Include(person => person.CourseInstances).ThenInclude(courseInstance => courseInstance.Course)
						?.ToListAsync();

					// Don't know why this is here. You don't need to map person into person. Tests still run after this change.
					//.Select(person => person)?.ToListAsync();
				}
			}
			catch
			{
				//return null if there's a problem
				return null;
			}
			//returns person or empty array if none exist
			return foundPersons;
		}

		/// <summary>
		/// Get all persons that are apart of the given org
		/// </summary>
		/// <param name="org"></param>
		/// <returns></returns>
		public async Task<List<Person>> GetAllPersons(Org org)
		{
			var foundPersons = new List<Person>();

			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					foundPersons = await insightContext.Persons.Where(x => x.Organization == org)
						.Include(p => p.Medical)
						.Include(p => p.Personnel)
						.Include(p => p.Training)
						.Include(p => p.Organization)
						.Select(x => x)?.ToListAsync();
				}
			}
			catch
			{
				//return null if there's a problem
				return null;
			}
			//returns person or empty array if none exist
			return foundPersons;
		}

		/// <summary>
		/// Returns person that matches First/Last name or null if none exist
		/// </summary>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <returns></returns>
		public async Task<List<Person>> GetPersonsByName(string firstName, string lastName, bool includeSubref = true)
		{
			if (firstName == null || lastName == null) return null;

			var foundPersons = new List<Person>();

			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					// TODO Make if else or make more readable
					foundPersons = includeSubref ? await insightContext.Persons
						.Include(p => p.Medical)
						.Include(p => p.Personnel)
						.Include(p => p.Training)
						.Include(p => p.Organization)
						.Include(p => p.AFSC)
						.Include(p => p.CourseInstances).ThenInclude(courseInstance => courseInstance.Course)
						.Where(x => x.FirstName == firstName.ToUpperInvariant() && x.LastName == lastName.ToUpperInvariant())?.ToListAsync()
						: await insightContext.Persons.Where(x => x.FirstName == firstName.ToUpperInvariant() && x.LastName == lastName.ToUpperInvariant())?.ToListAsync();
				}
			}
			catch
			{
				//return null if there's a problem
				return null;
			}
			//returns person or empty array if none exist
			return foundPersons;
		}

		/// <summary>
		/// Returns person that matches shortName within a organization
		/// </summary>
		/// <param name="shortName">A "shortName" is a name like SmithJ</param>
		/// <returns></returns>
		[CanBeNull]
		public async Task<List<Person>> GetPersonsByShortName(string shortName)
		{
			var (firstLetters, lastName) = StringManipulation.ConvertShortNameToNames(shortName);

			List<Person> foundPersons = null;

			try
			{
				using (var insightContext = new InsightContext(_dbContextOptions))
				{
					foundPersons = await insightContext.Persons
						.Include(p => p.Medical)
						.Include(p => p.Personnel)
						.Include(p => p.Training)
						.Include(p => p.Organization)
						.Where(person => person.FirstName.Contains(firstLetters.ToUpperInvariant())
										 && person.LastName == lastName.ToUpperInvariant())
						.ToListAsync();
				}
			}
			catch
			{
				//return null if there's a problem
				return null;
			}
			//returns person or empty array if none exist
			return foundPersons;
		}

		/// <summary>
		/// Returns person that matches First, Last, SSN or null if none exist
		/// </summary>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="SSN"></param>
		/// <returns></returns>
		public async Task<List<Person>> GetPersonsByNameSSN(string firstName, string lastName, string SSN)
		{
			//TODO refactor to reuse code more and have better methods
			var foundPersons = new List<Person>();
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					foundPersons = await insightContext.Persons
						.Include(p => p.Medical)
						.Include(p => p.Personnel)
						.Include(p => p.Training)
						.Include(p => p.Organization)
						.Where(x => x.FirstName == firstName.ToUpperInvariant() && x.LastName == lastName.ToUpperInvariant() && x.SSN == SSN).ToListAsync();
				}
			}
			catch
			{
				//return null if there's a problem
				return null;
			}
			//returns person or empty array if none exist
			return foundPersons;
		}
	}
}
