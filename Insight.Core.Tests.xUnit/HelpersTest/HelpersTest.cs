using FluentAssertions;
using Insight.Core.Models;
using Insight.Core.Services.File;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Insight.Core.Helpers;
using Xunit;

namespace Insight.Core.Tests.xUnit.HelpersTest
{
	public class HelpersTest
	{
		public class ConvertToTitleCaseTests
		{
			[Fact]
			public void AllCaps()
			{
				string value = "ALL CAPS";

				string result = StringManipulation.ConvertToTitleCase(value);

				string expected = "All Caps";

				result.Should().Be(expected);

			}

			[Fact]
			public void AllLower()
			{
				string value = "all lower";

				string result = StringManipulation.ConvertToTitleCase(value);

				string expected = "All Lower";

				result.Should().Be(expected);

			}

			[Fact]
			public void Null()
			{
				string value = null;

				string result = StringManipulation.ConvertToTitleCase(value);

				result.Should().BeNull();

			}

			[Fact]
			public void Empty()
			{
				string value = "";

				string result = StringManipulation.ConvertToTitleCase(value);

				string expected = "";

				result.Should().Be(expected);

			}
		}

		public class StatusReaderTests
		{
			[Fact]
			public void Current()
			{
				string value = "g";

				Status result = StringManipulation.StatusReader(value);

				Status expected = Status.Current;

				result.Should().Be(expected);
			}

			[Fact]
			public void Upcoming()
			{
				string value = "y";

				Status result = StringManipulation.StatusReader(value);

				Status expected = Status.Upcoming;

				result.Should().Be(expected);
			}

			[Fact]
			public void Overdue()
			{
				string value = "r";

				Status result = StringManipulation.StatusReader(value);

				Status expected = Status.Overdue;

				result.Should().Be(expected);
			}

			[Fact]
			public void Null()
			{
				string value = null;

				Status result = StringManipulation.StatusReader(value);

				Status expected = Status.Unknown;

				result.Should().Be(expected);
			}

			[Fact]
			public void Empty()
			{
				string value = "";

				Status result = StringManipulation.StatusReader(value);

				Status expected = Status.Unknown;

				result.Should().Be(expected);
			}
		}
	}
}
