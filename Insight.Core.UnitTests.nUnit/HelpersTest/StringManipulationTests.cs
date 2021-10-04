using FluentAssertions;
using Insight.Core.Models;
using Insight.Core.Helpers;
using NUnit.Framework;

namespace Insight.Core.UnitTests.nUnit.HelpersTest
{
	[TestFixture]
	public class StringManipulationTests
	{
		[TestCase("SmithJ", "J", "Smith")]
		[TestCase("SmithJa", "Ja", "Smith")]
		[TestCase("SimsssYo", "Yo", "Simsss")]
		[TestCase("YamatoJ", "J", "Yamato")]
		public void ConvertShortNameToNames_GoodInput(string shortName, string expectedFirstName, string expectedLastName)
		{
			//arange

			//act
			var (partialFirstName, lastName) = StringManipulation.ConvertShortNameToNames(shortName);

			//assert
			partialFirstName.Should().Be(expectedFirstName);
			lastName.Should().Be(expectedLastName);
		}

		[TestCase("Yamatoj")]
		[TestCase("yamatoj")]
		[TestCase("yamatoj")]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void ConvertShortNameToNames_BadInput(string shortName)
		{
			//arange

			//act
			var (_, lastName) = StringManipulation.ConvertShortNameToNames(shortName);

			//assert
			lastName.Should().BeNullOrEmpty();
		}

		[TestCase("this is a sentence", "This Is A Sentence")]
		[TestCase("THIS IS A SENTENCE", "This Is A Sentence")]
		[TestCase("tHiS iS a SeNtEnCe", "This Is A Sentence")]
		[TestCase("tHiS iS â SeNtÉnCe", "This Is Â Senténce")]
		[TestCase("", "")]
		[TestCase(null, null)]
		public void ConvertToTitleCaseTests(string input, string expected)
		{
			string result = StringManipulation.ConvertToTitleCase(input);

			result.Should().Be(expected);
		}

		[TestCase("g", Status.Current)]
		[TestCase("G", Status.Current)]
		[TestCase("y", Status.Upcoming)]
		[TestCase("Y", Status.Upcoming)]
		[TestCase("r", Status.Overdue)]
		[TestCase("R", Status.Overdue)]
		[TestCase(null, Status.Unknown)]
		[TestCase("", Status.Unknown)]
		public void StatusReaderTests(string input, Status expected)
		{
			Status result = StringManipulation.StatusReader(input);

			result.Should().Be(expected);
		}
	}
}
