using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using Insight.Core.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions.Execution;

namespace Insight.Core.UnitTests.nUnit.ServicesTests.DatabaseTests
{
	public partial class InsightControllerTests
	{
		[Test]
		public async Task GetPeople()
		{
			var people = await controller.GetAllPersons();

			people.Count.Should().Be(6);
		}

		[Test]
		public async Task GenericGetPeople()
		{
			var people = await controller.GetAll<Person>();

			people.Count.Should().Be(6);
		}

		[Test]
		public void GetPersonsByName_ExpectOne()
		{

			var persons = controller.GetPersonsByName("John", "Smith").Result;
			var person = persons.FirstOrDefault();

			using (new AssertionScope())
			{
				persons.Should().HaveCount(1);
				person.Should().NotBeNull();
				person.Name.Should().Be("SMITH, JOHN");
			}
		}

		[Test]
		public void GetPersonsByName_ExpectTwo()
		{
			var persons = controller.GetPersonsByName("Joe", "Murray").Result;
			var person0 = persons.ElementAtOrDefault(0);
			var person1 = persons.ElementAtOrDefault(1);

			using (new AssertionScope())
			{
				persons.Should().HaveCount(2);

				person0.Should().NotBeNull();
				person0.Name.Should().Be("MURRAY, JOE");

				person1.Should().NotBeNull();
				person1.Name.Should().Be("MURRAY, JOE");
			}
		}

		[TestCase("I should", "not exist")]
		[TestCase("John", "not exist")]
		[TestCase(null, null)]
		[TestCase("", "")]
		[TestCase(" ", " ")]
		[TestCase("idk", "Smith")]
		public void GetPersonsByName_ExpectNone(string firstName, string lastName)
		{
			//arrange

			//act
			var persons = controller.GetPersonsByName(firstName, lastName).Result;

			//assert
			persons.Should().BeNullOrEmpty();
		}

		[Test]
		public void GetPersonsCaps()
		{
			//arrange

			//act
			var persons = controller.GetPersonsByName("JOHN", "SMITH").Result;
			var person = persons.FirstOrDefault();

			using (new AssertionScope())
			{
				persons.Should().HaveCount(1);
				person.Should().NotBeNull();
				person.Name.Should().Be("SMITH, JOHN");
			}
		}

		[Test]
		public void AddThenGetPersons()
		{
			//arrange
			var personToAdd = new Person { FirstName = "Jonathan", LastName = "Xander" };

			//act
			controller.Add(personToAdd);

			var persons = controller.GetPersonsByName("Jonathan", "Xander").Result;
			var person = persons.FirstOrDefault();

			using (new AssertionScope())
			{
				persons.Should().HaveCount(1);
				person.Should().NotBeNull();
				person.Id.Should().Be(personToAdd.Id);
				person.Name.Should().Be("XANDER, JONATHAN");
			}
		}

		[TestCase("SmithJ", "John", "Smith")]
		[TestCase("TurnerAnnab", "Annabell", "Turner")]
		public void GetPersonsShortName_ExpectOne(string shortName, string expectedFirstName, string expectedLastName)
		{
			//arrange

			//act
			var persons = controller.GetPersonsByShortName(shortName).Result;
			var person = persons.FirstOrDefault();

			//assert
			persons.Should().HaveCount(1);

			person.FirstName = expectedFirstName;
			person.LastName = expectedLastName;
		}

		[TestCase("MurrayJ", "JOE", "MURRAY")]
		[TestCase("MurrayJo", "JOE", "MURRAY")]
		[TestCase("MurrayJoe", "JOE", "MURRAY")]
		public void GetPersonsShortName_ExpectTwo(string shortName, string expectedFirstName, string expectedLastName)
		{
			var persons = controller.GetPersonsByShortName(shortName)?.Result;

			var person0 = persons.ElementAtOrDefault(0);
			var person1 = persons.ElementAtOrDefault(1);

			using (new AssertionScope())
			{
				persons.Should().HaveCount(2);

				person0.Should().NotBeNull();
				person0.Name.Should().Be($"{expectedLastName}, {expectedFirstName}");

				person1.Should().NotBeNull();
				person1.Name.Should().Be("MURRAY, JOE");
			}
		}

		[TestCase("doesnotexist")]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		[TestCase("i dont know")]
		public void GetPersonsShortName_ExpectNone(string shortName)
		{
			var persons = controller.GetPersonsByShortName(shortName)?.Result;

			persons.Should().BeNullOrEmpty();
		}

		[TestCase("Graham", "Soyer", "123456789")]
		public void GetPersonsByNameSSN_ExpectOne( string expectedFirstName, string expectedLastName, string expectedSSN)
		{
			var persons = controller.GetPersonsByNameSSN("Graham", "Soyer", "123456789").Result;
			var person = persons.FirstOrDefault();

			using (new AssertionScope())
			{
				persons.Should().HaveCount(1);

				person.FirstName = expectedFirstName;
				person.LastName = expectedLastName;
				person.SSN = expectedSSN;
			}
		}
		[TestCase("doesnot", "exist", "123456789")]
		[TestCase("", "", "")]
		[TestCase(" ", " ", " ")]
		[TestCase(null, null, null)]
		[TestCase("i dont", "know", "adgadga")]
		public void GetPersonsByNameSSN_ExpectNone(string expectedFirstName, string expectedLastName, string expectedSSN)
		{
			//arrange

			//act
			var persons = controller.GetPersonsByNameSSN("random", "name", "123456789").Result;

			//assert
			persons.Should().BeNullOrEmpty();
		}

		[Test]
		public void GetPersonsByNameSSN_PersonExistsButNoSSN()
		{
			//arrange

			//act
			var persons = controller.GetPersonsByNameSSN("John", "Smith", "123456789").Result;

			//assert
			persons.Should().BeNullOrEmpty();
		}

		
	}
}
