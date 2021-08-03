using Insight.Core.Models;
using Insight.Core.Services.Database;
using Insight.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq;
using FluentAssertions;

namespace Insight.Core.Tests.nUnit.ServicesTests
{
	[TestFixture]
	public class InsightControllerTests
	{
		public InsightController controller;

		/// <summary>
		/// Set up run before every test
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			DbContextOptions<InsightContext> dbContextOptions = new DbContextOptionsBuilder<InsightContext>()
				.UseInMemoryDatabase(databaseName: "InsightTestDB")
				.Options;

			controller = new InsightController(dbContextOptions);

			SeedDb();

		}

		/// <summary>
		/// Adds mock data to database
		/// </summary>
		private void SeedDb()
		{
			var persons = new List<Person>
			{
				new Person { FirstName = "John", LastName = "Smith" },
				new Person { FirstName = "Jacob", LastName = "Smith" },
				new Person { FirstName = "Constantine", LastName = "Quintrell" },
				new Person { FirstName = "Annabell", LastName = "Turner" },
				new Person { FirstName = "Graham", LastName = "Soyer" },
			};

			foreach (var person in persons)
			{
				controller.Add(person);
			}
		}


		[Test]
		public async Task GetPeople()
		{
			var people = await controller.GetAllPersons();

			people.Count().Should().Be(5);
		}

		[Test]
		public void GetPersonByName()
		{
			var person = controller.GetPersonByName("John", "Smith").Result;

			person.Should().NotBeNull();
		}

		[Test]
		public void GetPersonCaps()
		{
			var person = controller.GetPersonByName("JOHN", "SMITH").Result;

			person.Should().NotBeNull();
		}

		[Test]
		public void GetNullPerson()
		{
			var person = controller.GetPersonByName("I should", "not exist").Result;

			person.Should().BeNull();
		}

		[Test]
		public void AddPerson()
		{
			var person = new Person { FirstName = "Jonathan", LastName = "Xander" };

			controller.Add(person);

			var personFromDB = controller.GetPersonByName("Jonathan", "Xander").Result;

			person.Id.Should().Be(personFromDB.Id);
		}

		[Test]
		public void UpdatePerson()
		{
			var person = controller.GetPersonByName("John", "Smith").Result;

			person.FirstName = "Johnathan";

			controller.Update(person);

			var personFromDB = controller.GetPersonByName("Johnathan", "Smith").Result;

			personFromDB.Id.Should().Be(person.Id);
		}

		/// <summary>
		/// Teardown run after every test
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			controller.EnsureDatabaseDeleted();
		}
	}
}
