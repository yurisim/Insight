using Insight.Core.Services.Database;
using Insight.Core.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Insight.Core.Services.File;
using Insight.Core.Models;
using FluentAssertions;
using System.Collections.Generic;

namespace Insight.Core.Tests.nUnit.ServicesTests.FileTests
{
	[TestFixture]
	public class AbstractDigestTests : AbstractDigest
	{
		private static readonly DbContextOptions<InsightContext> dbContextOptions =
				new DbContextOptionsBuilder<InsightContext>()
					.UseInMemoryDatabase("InsightTestDB")
					.Options;

		public InsightController controller;

		/// <summary>
		/// Set ups for tests
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			controller = new InsightController(dbContextOptions);
		}

		/// <summary>
		/// Invokes base constructor
		/// </summary>
		public AbstractDigestTests() : base(null, dbContextOptions)
		{

		}

		[TestCaseSource(typeof(AFSCTestCasesObject), nameof(AFSCTestCasesObject.GetOrCreateAFSCTestCases))]
		public void GetOrCreateAFSC_Create(string input, string expected)
		{
			//arrange

			//act
			AFSC afsc = base.GetOrCreateAFSC(input, input, input);

			//assert
			afsc.Should().NotBeNull();
			afsc.PAFSC.Should().Be(expected);
			afsc.DAFSC.Should().Be(expected);
			afsc.CAFSC.Should().Be(expected);
		}

		[TestCaseSource(typeof(AFSCTestCasesObject), nameof(AFSCTestCasesObject.GetOrCreateAFSCTestCases))]
		public void GetOrCreateAFSC_GetExisting(string input, string expected)
		{
			//arrange
			AFSC afscToCreate = new AFSC()
			{
				PAFSC = input,
				CAFSC = input,
				DAFSC = input,
			};

			controller.Add(afscToCreate);

			//act
			AFSC afsc = base.GetOrCreateAFSC(input, input, input);

			//assert
			afsc.Should().NotBeNull();
			afsc.PAFSC.Should().Be(expected);
			afsc.CAFSC.Should().Be(expected);
			afsc.DAFSC.Should().Be(expected);
		}


		private class AFSCTestCasesObject
		{
			public static object[] GetOrCreateAFSCTestCases =
			{
				//test cases	input, expected
				new[] { "3D0X4", "3D0X4" },
				new[] { "3d0x1", "3D0X1" },
				new[] { "2a0x4", "2A0X4" },
				new[] { "17DA", "17DA" },
				new[] { "T17DA", "T17DA" },
				new[] { "invalid afsc", "INVALID AFSC" }, //what ever the input is, expected it back

			};
		}
	}
}
