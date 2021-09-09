using FluentAssertions;
using Insight.Core.Models;
using NUnit.Framework;
using System;

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
	}
}
