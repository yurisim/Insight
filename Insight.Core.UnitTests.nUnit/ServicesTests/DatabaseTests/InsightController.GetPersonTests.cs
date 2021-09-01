using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using Insight.Core.Models;

namespace Insight.Core.UnitTests.nUnit.ServicesTests.DatabaseTests
{
	public partial class InsightControllerTests
	{
		[TestCase("SmithJ", "John", "Smith")]
		public void GetPersonByShortNameTest(string shortName, string expectedFirstName, string expectedLastName)
		{
			//arrange

			//act
			var person = controller.GetPersonByShortName(shortName);

			//assert
			person.Should().NotBeNull();
			person.FirstName = expectedFirstName;
			person.LastName = expectedLastName;
		}

		[Test]
		public async Task GetPeople()
		{
			var people = await controller.GetAllPersons();

			people.Count.Should().Be(5);
		}

		[Test]
		public async Task GenericGetPeople()
		{
			var people = await controller.GetAll<Person>();

			people.Count.Should().Be(5);
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
	}
}
