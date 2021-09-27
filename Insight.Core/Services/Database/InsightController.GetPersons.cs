using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
			var persons = new List<Person>();

			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					persons = await insightContext.Persons
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
			catch (Exception)
			{
				//throw new Exception("Insight.db access error");
			}

			return persons;
		}

		/// <summary>
		/// Get all persons that are apart of the given org
		/// </summary>
		/// <param name="org"></param>
		/// <returns></returns>
		public async Task<List<Person>> GetAllPersons(Org org)
		{
			List<Person> persons;

			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					persons = await insightContext.Persons.Where(x => x.Organization == org)
						.Include(p => p.Medical)
						.Include(p => p.Personnel)
						.Include(p => p.Training)
						.Include(p => p.Organization)
						.Select(x => x)?.ToListAsync();
				}
			}
			catch (Exception)
			{
				throw new Exception("Insight.db access error");
			}

			return persons;
		}

		/// <summary>
		/// Returns person that matches First/Last name or null if none exist
		/// </summary>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <returns></returns>
		public async Task<Person> GetPersonByName(string firstName, string lastName, bool includeSubref = true)
		{
			Person person;
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					// TODO Make if else or make more readable
					var persons = includeSubref ? await insightContext.Persons
						.Include(p => p.Medical)
						.Include(p => p.Personnel)
						.Include(p => p.Training)
						.Include(p => p.Organization)
						.Include(p => p.AFSC)
						.Include(p => p.CourseInstances).ThenInclude(courseInstance => courseInstance.Course)
						.Where(x => x.FirstName == firstName.ToUpperInvariant() && x.LastName == lastName.ToUpperInvariant())?.ToListAsync()
						: await insightContext.Persons.Where(x => x.FirstName == firstName.ToUpperInvariant() && x.LastName == lastName.ToUpperInvariant())?.ToListAsync();

					//TODO implement better exceptions
					if (persons.Count > 1)
					{
						throw new Exception("Too many Persons found, should be null or 1");
					}

					person = persons.FirstOrDefault();
				}

			}
			//TODO implement exception
			catch (Exception e)
			{
				throw new Exception("Insight.db access error");
			}

			//returns person or null if none exist
			return person;
		}

		/// <summary>
		/// Returns person that matches shortName within a organization
		/// </summary>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <returns></returns>
		/// 
		public Person GetPersonByShortName(string shortName)
		{

			// break up shortname into first letters and last name
			// if name is SmithJ, then J Smith

			// TODO MAKE MORE FACTORS to find the correct person

			int indexOfCapital;
			for (indexOfCapital = shortName.Length - 1; indexOfCapital >= 0; indexOfCapital--)
			{
				if (char.IsUpper(shortName[indexOfCapital]))
				{
					break;
				}
			}

			var firstLetters = shortName.Substring(indexOfCapital);
			var lastName = shortName.Substring(0, indexOfCapital);

			// now try to find the person with the name
			Person foundPerson = null;
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					var foundPeople = insightContext.Persons
						.Include(p => p.Medical)
						.Include(p => p.Personnel)
						.Include(p => p.Training)
						.Include(p => p.Organization)
						.Where(person => person.FirstName.Contains(firstLetters.ToUpperInvariant()) && person.LastName == lastName.ToUpperInvariant());

					//TODO implement better exceptions
					if (foundPeople.Count() > 1)
					{
						throw new Exception("Too many Persons found, should be null or 1");
					}

					foundPerson = foundPeople.FirstOrDefault();
				}
			}
			//TODO implement exception
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}
			//returns person or null if none exist
			return foundPerson;
		}

		/// <summary>
		/// Returns person that matches First, Last, SSN or null if none exist
		/// </summary>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="SSN"></param>
		/// <returns></returns>
		public Person GetPersonByNameSSN(string firstName, string lastName, string SSN)
		{
			//TODO refactor to reuse code more and have better methods
			List<Person> persons = new List<Person>();
			Person person;
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					persons = insightContext.Persons
						.Include(p => p.Medical)
						.Include(p => p.Personnel)
						.Include(p => p.Training)
						.Include(p => p.Organization)
						.Where(x => x.FirstName == firstName.ToUpperInvariant() && x.LastName == lastName.ToUpperInvariant() && x.SSN == SSN).ToList();
					//TODO implement better exceptions
					if (persons.Count > 1)
					{
						throw new Exception("Too many Persons found, should be null or 1");
					}

					person = persons.FirstOrDefault();

				}
			}
			//TODO implement exception
			catch (Exception)
			{
				throw new Exception("Insight.db access error");
			}
			//returns person or null if none exist
			return person;
		}
	}
}
