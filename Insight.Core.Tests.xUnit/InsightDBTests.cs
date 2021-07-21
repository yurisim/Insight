using System;
using System.Collections.Generic;
using System.Text;
using Insight.Core.Services;
using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;
using Insight.Core.Services.Database;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using FluentAssertions;

namespace Insight.Core.Tests.xUnit
{
	public class InsightControllerTests
	{
		private DbContextOptions<InsightContext> dbContextOptions = new DbContextOptionsBuilder<InsightContext>()
			.UseInMemoryDatabase(databaseName: "InsightTestDB")
			.Options;

		private InsightController controller;

		public InsightControllerTests()
		{
			SeedDb();

			controller = new InsightController(dbContextOptions);
		}

		[Fact]
		public async Task GetPeopleTest()
		{
			using var context = new InsightContext(dbContextOptions);

			var people = await controller.GetAllPersons();

			people.Count().Should().Be(5);
		}

		private void SeedDb()
		{
			using var context = new InsightContext(dbContextOptions);

			var persons = new List<Person>
			{
				new Person { Id = 1, FirstName = "John", LastName = "Smith" },
				new Person { Id = 2, FirstName = "Jacob", LastName = "Smith" },
				new Person { Id = 3, FirstName = "Constantine", LastName = "Quintrell" },
				new Person { Id = 4, FirstName = "Annabell", LastName = "Turner" },
				new Person { Id = 5, FirstName = "Graham", LastName = "Soyer" },
			};

			context.AddRange(persons);

			context.SaveChanges();
		}
	}
}
