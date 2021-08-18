using Insight.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Core.Tests.nUnit.ModelsTests
{
	[TestFixture]
	public class PersonTests
	{
		[TestCase("lowercase","LOWERCASE")]
		[TestCase("UPPERCAE", "UPPERCASE")]
		[TestCase("mIxEdCaSiNg", "MIXEDCASING")]
		[TestCase("idontknow,numbersandstuff&3236!!", "idontknow,numbersandstuff&3236!!")]
		[TestCase("", "")]
		public void Person_TestNameCasing(string input, string expected)
		{
			//arrange

			//act
			var person = new Person { FirstName = input, LastName = input };

			//assert
			person.FirstName = expected;
			person.LastName = expected;
		}
	}
}
