using FluentAssertions;
using Insight.Core.Models;
using NUnit.Framework;

namespace Insight.Core.UnitTests.nUnit.ModelsTests
{
	[TestFixture]
	public class PersonTests
	{
		/// <summary>
		/// Tests if the property of Person correctly upper cases the first and last names.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="expected"></param>
		[TestCase("lowercase", "LOWERCASE")]
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
