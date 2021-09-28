using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using Insight.Core.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Insight.Core.UnitTests.nUnit.ServicesTests.DatabaseTests
{
	public partial class InsightControllerTests
	{
		[TestCase("SmithJ", "John", "Smith")]
		public void GetPersonsShortName_Expect__One(string shortName, string expectedFirstName, string expectedLastName)
		{
			//arrange

			//act
			var persons = controller.GetPersonsByShortName(shortName)?.Result;

			//assert
			persons?.Should().HaveCount(1);

			Debug.Assert(persons != null, nameof(persons) + " != null");

			persons.First().FirstName = expectedFirstName;
			persons.First().LastName = expectedLastName;
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

		[Test]
		public void GetPersonByNameSSN_Good()
		{
			//arrange

			//act
			var person = controller.GetPersonByNameSSN("Graham", "Soyer", "123456789");

			//assert
			person.Should().NotBeNull();
		}

		[Test]
		public void GetPersonByNameSSN_DoesNotExist()
		{
			//arrange

			//act
			var person = controller.GetPersonByNameSSN("random", "name", "123456789");

			//assert
			person.Should().BeNull();
		}

		[Test]
		public void GetPersonByNameSSN_PersonExistsButNoSSN()
		{
			//arrange

			//act
			var person = controller.GetPersonByNameSSN("John", "Smith", "123456789");

			//assert
			person.Should().BeNull();
		}
	}
}
