using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Insight.Core.Services.Database;

namespace Insight.Core.Tests.nUnit.ServicesTests.DatabaseTests
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
	}
}
