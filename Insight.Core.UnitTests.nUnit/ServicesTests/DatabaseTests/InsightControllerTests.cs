using Insight.Core.Models;
using Insight.Core.Services.Database;
using Insight.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using System.Reflection;
using System.Linq;

namespace Insight.Core.UnitTests.nUnit.ServicesTests.DatabaseTests
{
	[TestFixture]
	public partial class InsightControllerTests
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
				new() { FirstName = "Hacob", LastName = "Smith" },
				new() { FirstName = "Constantine", LastName = "Quintrell" },
				new() { FirstName = "Annabell", LastName = "Turner" },
				new() { FirstName = "Graham", LastName = "Soyer" , SSN = "123456789" },
			};

			foreach (var person in persons)
			{
				controller.Add(person);
			}

			var course = new Course()
			{
				Name = "Underwater Basket Weaving",
				Interval = 2,
			};

			controller.Add(course);
		}

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.AddTestCases))]
		public void AddTest<T>(T entityToAdd) where T : class
		{
			controller.Add(entityToAdd);

			PropertyInfo prop = entityToAdd.GetType().GetProperty("Id");

			prop.Should().NotBeNull();

			T entityFromDb = controller.GetByID<T>((int)prop.GetValue(entityToAdd)).Result;

			entityFromDb.Should().BeEquivalentTo(entityToAdd);
		}

		[Test]
		public void UpdatePerson()
		{
			//arrange
			var person = controller.GetPersonsByName("John", "Smith").Result.FirstOrDefault();

			person.FirstName = "Johnathan";

			//act
			controller.Update(person);

			var personFromDB = controller.GetPersonsByName("Johnathan", "Smith").Result.FirstOrDefault();
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

		/// <summary>
		/// Objects containing the data for the test cases
		/// </summary>
		private class TestCasesObjects
		{
			public static object[] AddTestCases =
			{
				//test cases
				new[] {
					new Person()
					{
						FirstName = "Test",
						LastName = "Test"
					}
				},

				//test cases
				new[] {
					new AFSC()
					{
						CAFSC = "AFSC",
						PAFSC = "AFSC",
						DAFSC = "AFSC",
					}
				},
			};
		}
	}
}
