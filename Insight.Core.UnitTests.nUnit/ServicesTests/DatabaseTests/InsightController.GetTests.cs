using FluentAssertions;
using Insight.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Insight.Core.UnitTests.nUnit.ServicesTests.DatabaseTests
{
	public partial class InsightControllerTests
	{
		[Test]
		public void GetNullCourseInstance()
		{
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

		[Test]
		public void GetOrgByAliasTests()
		{
			//arrange
			string orgName = "random org name";
			string orgAliasName = "this is my org alias";

			var orgToAdd = new Org { Name = "random org name", Aliases = new List<OrgAlias>() };
			var orgAliasesToAdd = new OrgAlias { Name = orgAliasName, Org = orgToAdd };

			orgToAdd.Aliases.Add(orgAliasesToAdd);

			controller.Add(orgToAdd);

			//act
			var orgAliasFromDB = controller.GetOrgByAlias(orgAliasName);

			//assert
			orgAliasFromDB.Name.Should().Be(orgName.ToUpper());
		}
	}
}
