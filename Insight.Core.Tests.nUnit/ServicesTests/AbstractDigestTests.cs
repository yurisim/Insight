using Insight.Core.Services.Database;
using Insight.Core.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Insight.Core.Services.File;
using Insight.Core.Models;
using FluentAssertions;

namespace Insight.Core.Tests.nUnit.ServicesTests
{
	[TestFixture]
	public class AbstractDigestTests : AbstractDigest
	{
		private static readonly DbContextOptions<InsightContext> dbContextOptions =
				new DbContextOptionsBuilder<InsightContext>()
					.UseInMemoryDatabase("InsightTestDB")
					.Options;

		public InsightController controller;


		[SetUp]
		public void SetUp()
		{
			controller = new InsightController(dbContextOptions);
		}

		/// <summary>
		/// Invokes base constructor
		/// </summary>
		public AbstractDigestTests() : base(null, dbContextOptions) { }


		[TestCase("3D0X4", "3D0X4")]
		[TestCase("17DA", "17DA")]
		[TestCase("invalid afsc", null)]
		public void GetOrCreateAFSC_Create(string input, string expected)
		{
			//act
			AFSC afsc = base.GetOrCreateAFSC(input);

			//assert
			afsc.Should().NotBeNull();
			afsc.Name.Should().Be(expected);
		}

		[TestCase("3D0X4", "3D0X4")]
		[TestCase("17DA", "17DA")]
		public void GetOrCreateAFSC_GetExisting(string input, string expected)
		{
			//arrange
			AFSC afscToCreate = new AFSC()
			{
				Name = input,
			};

			controller.Add(afscToCreate);

			//act
			AFSC afsc = base.GetOrCreateAFSC(input);

			//assert
			afsc.Should().NotBeNull();
			afsc.Name.Should().Be(expected);
		}
	}
}
