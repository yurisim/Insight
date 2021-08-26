﻿using Insight.Core.Models;
using Insight.Core.Services.Database;
using Insight.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq;
using FluentAssertions;
using System;
using System.Reflection;

namespace Insight.Core.Tests.nUnit.ServicesTests.DatabaseTests
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
				new() { FirstName = "Graham", LastName = "Soyer" },
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


		[Test]
		public async Task GetPeople()
		{
			var people = await controller.GetAllPersons();

			people.Count().Should().Be(5);
		}

		[Test]
		public async Task GenericGetPeople()
		{
			var people = await controller.GetAll<Person>();

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
			var person = controller.GetPersonByName("John", "Smith").Result;

			person.FirstName = "Johnathan";

			//act
			controller.Update(person);

			var personFromDB = controller.GetPersonByName("Johnathan", "Smith").Result;
			//assert

			personFromDB.Id.Should().Be(person.Id);
		}

		[Test]
		public void GetNullCourseInstance()
		{
			var person = controller.GetPersonByName("JOHN", "SMITH").Result;

			var shouldNotExist = new CourseInstance()
			{
				Completion = DateTime.Today,
				Person = new Person()
				{
					FirstName = "John",
					LastName = "Doe"
				},
				Course = new Course()
				{
					Id = 65475430
				}
			};
			var nullCourse = controller.GetCourseInstance(shouldNotExist).Result;

			nullCourse.Should().BeNull();
		}

		[Test]
		public void GetCourseInstanceTest()
		{
			var person = controller.GetPersonByName("JOHN", "SMITH").Result;
			var course = controller.GetCourseByName("Underwater Basket Weaving");

			var shouldExist = new CourseInstance()
			{
				Id = Guid.NewGuid().GetHashCode(),
				Completion = DateTime.Today.AddHours('5'),
				Person = person,
				Course = course
			};

			controller.AddCourseInstance(shouldExist, person: person, course: course);

			var parsedCourse = controller.GetCourseInstance(shouldExist).Result;

			parsedCourse.Completion.Should().Be(DateTime.Today.AddHours('5'));
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
