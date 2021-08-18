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

namespace Insight.Core.Tests.nUnit.ServicesTests.DatabaseTests
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
				new() { FirstName = "John", LastName = "Smith" },
				new() { FirstName = "Jacob", LastName = "Smith" },
				new() { FirstName = "Constantine", LastName = "Quintrell" },
				new() { FirstName = "Annabell", LastName = "Turner" },
				new() { FirstName = "Graham", LastName = "Soyer" },
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
			//arrange

			//act
			var person = controller.GetPersonByName("JOHN", "SMITH").Result;

			//assert
			person.Should().NotBeNull();
		}

		[Test]
		public void GetNullPerson()
		{
			//arrange

			//act
			var person = controller.GetPersonByName("I should", "not exist").Result;

			//assert
			person.Should().BeNull();
		}

		[Test]
		public void AddPerson()
		{
			//arrange
			var person = new Person { FirstName = "Jonathan", LastName = "Xander" };

			//act
			controller.Add(person);

			var personFromDB = controller.GetPersonByName("Jonathan", "Xander").Result;

			//assert
			person.Id.Should().Be(personFromDB.Id);
			
		}

		[Test]
		public void UpdatePerson()
		{
			//arrange
			var person = controller.GetPersonByName("John", "Smith").Result;

			person.FirstName = "Johnathan";

			//act
			controller.Update(person);

			var personFromDB = controller.GetPersonByName("Johnathan", "Smith").Result;
			//assert

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
