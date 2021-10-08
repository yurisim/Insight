﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Insight.Core.Models;
using Insight.Core.Helpers;
using NUnit.Framework;

namespace Insight.Core.UnitTests.nUnit.HelpersTest
{
	[TestFixture]
	public class HelpersTests
	{
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
			var result = StringManipulation.StatusReader(input);

			result.Should().Be(expected);
		}


		/// <summary>
		/// This test probably fails in unix systems btw.
		/// Unix systems only use \n instead of \r\n
		/// </summary>
		/// <param name="input"></param>
		/// <param name="expected"></param>
		[TestCase(new[] {"Cater.csv", "Is.csv", "Alaskan.csv" }, "\r\nCater.csv\r\nIs.csv\r\nAlaskan.csv")]
		public void FileNameFormatterTest(IEnumerable<string> input, string expected)
		{
			var result = StringManipulation.FileNameFormatter(input);

			result.Should().Be(expected);
		}
	}
}
