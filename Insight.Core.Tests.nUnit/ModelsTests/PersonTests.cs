using FluentAssertions;
using Insight.Core.Models;
using NUnit.Framework;

namespace Insight.Core.Tests.nUnit.ModelsTests
{
	[TestFixture]
	public class PersonTests
	{
		[TestCase("lowercase","LOWERCASE")]
		[TestCase("UPPERCASE", "UPPERCASE")]
		[TestCase("mIxEdCaSiNg", "MIXEDCASING")]
		[TestCase("idontknow,numbersandstuff&3236!!", "IDONTKNOW,NUMBERSANDSTUFF&3236!!")]
		[TestCase("", "")]
		public void Person_TestNameCasing(string input, string expected)
		{
			//arrange

			//act
			var person = new Person { FirstName = input, LastName = input };

			//assert
			person.FirstName.Should().Be(expected);
			person.LastName.Should().Be(expected);
		}
	}
}
