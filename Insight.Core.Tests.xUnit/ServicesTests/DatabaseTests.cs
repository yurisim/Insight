using Insight.Core.Models;
using Insight.Core.Services.Database;
using Insight.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using FluentAssertions;

namespace Insight.Core.Tests.xUnit.ServicesTests
{
	public class InsightControllerTests : IDisposable
	{
		private InsightController controller;
		private bool disposedValue;

		public InsightControllerTests()
		{
			DbContextOptions<InsightContext> dbContextOptions = new DbContextOptionsBuilder<InsightContext>()
				.UseInMemoryDatabase(databaseName: "InsightTestDB")
				.Options;
			controller = new InsightController(dbContextOptions);

			SeedDb();

		}

		private void SeedDb()
		{

			var persons = new List<Person>
			{
				new Person { Id = 1, FirstName = "John", LastName = "Smith" },
				new Person { Id = 2, FirstName = "Jacob", LastName = "Smith" },
				new Person { Id = 3, FirstName = "Constantine", LastName = "Quintrell" },
				new Person { Id = 4, FirstName = "Annabell", LastName = "Turner" },
				new Person { Id = 5, FirstName = "Graham", LastName = "Soyer" },
			};

			foreach(var person in persons)
			{
				controller.Add(person);
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					controller.EnsureDatabaseDeleted();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~InsightControllerTests()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		[Fact]
		public async Task GetPeople()
		{
			var people = await controller.GetAllPersons();

			people.Count().Should().Be(5);
		}

		[Fact]
		public void GetNullPerson()
		{
			var person = controller.GetPersonByName("I should", "not exist");

			person.Should().BeNull();
		}

		[Fact]
		public void AddPerson()
		{
			var person = new Person { FirstName = "Jonathan", LastName = "Xander" };

			controller.Add(person);

			var personFromDB = controller.GetPersonByName("Jonathan", "Xander");

			person.Id.Should().Be(personFromDB.Id);
		}

		[Fact]
		public void UpdatePerson()
		{
			var person = controller.GetPersonByName("John", "Smith");

			person.FirstName = "Johnathan";

			controller.Update(person);

			var personFromDB = controller.GetPersonByName("Johnathan", "Smith");

			personFromDB.Id.Should().Be(person.Id);
		}
	}
}
