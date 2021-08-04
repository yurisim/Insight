using Insight.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Insight.Core.Services.Database
{
	public class InsightController
	{
		private DbContextOptions<InsightContext> _dbContextOptions;

		public InsightController(DbContextOptions<InsightContext> dbContextOptions)
		{
			_dbContextOptions = dbContextOptions;
		}

		/// <summary>
		/// Whenever the blank constructor is used, it will use the default insight solution. 
		/// </summary>
		public InsightController()
		{
			_dbContextOptions = new DbContextOptionsBuilder<InsightContext>()
			.UseSqlite("Filename=Insight.db")
			.Options;
		}

		/// <summary>
		/// Ensures database has been created.
		/// </summary>
		public void EnsureDatabaseCreated()
		{
			using (InsightContext insightContext = new InsightContext(_dbContextOptions))
			{
				//Ensure database is created
				_ = insightContext.Database.EnsureCreated();
			}
		}

		/// <summary>
		/// Ensures database has been deleted.
		/// </summary>
		public void EnsureDatabaseDeleted()
		{
			using (InsightContext insightContext = new InsightContext(_dbContextOptions))
			{
				//Ensure database is created
				_ = insightContext.Database.EnsureDeleted();
			}
		}


		/// <summary>
		/// Returns all Person objects from database
		/// </summary>
		/// <returns></returns>
		public async Task<List<Person>> GetAllPersons()
		{
			List<Person> persons;

			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					persons = await insightContext.Persons
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
		/// Returns all OrgAlias objects from database
		/// </summary>
		/// <returns></returns>
		public async Task<List<OrgAlias>> GetAllOrgAliases()
		{
			List<OrgAlias> orgAliases;

			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					orgAliases = await insightContext.OrgAliases.Select(x => x)?.ToListAsync();

				}
			}
			catch (Exception)
			{
				throw new Exception("Insight.db access error");
			}

			return orgAliases;
		}

		/// <summary>
		/// Returns OrgAlias that matches name
		/// </summary>
		/// <param alias="alias"></param>
		/// <returns></returns>
		public Org GetOrgByAlias(string alias)
		{
			List<Org> orgs = new List<Org>();

			Org org = null;
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					orgs = insightContext.OrgAliases
						.Where(x => x.Name.ToLower() == alias.ToLower())?
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

		#region GetPersonByProperty
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
						.Where(x => x.FirstName == firstName.ToUpperInvariant() && x.LastName == lastName.ToUpperInvariant())?.ToListAsync()
						: await insightContext.Persons.Where(x => x.FirstName.ToLower() == firstName.ToLower() && x.LastName.ToLower() == lastName.ToLower())
							?.ToListAsync();

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

			int indexOfCapital = 0;
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

			Person foundPerson = new Person();
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					var foundPeople = insightContext.Persons
						.Include(p => p.Medical)
						.Include(p => p.Personnel)
						.Include(p => p.Training)
						.Include(p => p.Organization)
						.Where(person => person.FirstName.Contains(firstLetters) && person.LastName == lastName);

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
		#endregion

		/// <summary>
		/// Add entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		public async void Add<T>(T t)
		{
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					_ = insightContext.Add(t);
					_ = await insightContext.SaveChangesAsync();
				}
			}

			//TODO implement exception
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}

		/// <summary>
		/// Add entity
		/// </summary>
		/// <param name="courseInstance"></param>
		/// <param name="course"></param>
		/// <param name="person"></param>
		public async void Add(CourseInstance courseInstance, Course course, Person person)
		{
			using (var insightContext = new InsightContext(_dbContextOptions))
			{
				course.CourseInstances.Add(courseInstance);
				person.CourseInstances.Add(courseInstance);

				_ = insightContext.Update(courseInstance);

				_ = await insightContext.SaveChangesAsync();
			}
			//TODO implement exception
		}

		/// <summary>
		/// Update entity in database
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		public async void Update<T>(T t)
		{
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					_ = insightContext.Update(t);
					_ = await insightContext.SaveChangesAsync();
				}
			}
			//TODO implement exception
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
	}
}
