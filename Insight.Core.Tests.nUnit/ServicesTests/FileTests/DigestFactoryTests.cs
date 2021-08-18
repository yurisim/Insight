using Insight.Core.Models;
using Insight.Core.Services.File;
using Insight.Core.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using FluentAssertions;

namespace Insight.Core.Tests.nUnit.ServicesTests
{
	/// <summary>
	/// Digest Factory Tests
	/// </summary>
	[TestFixture]
	public class DigestFactoryTests
	{
		private DbContextOptions<InsightContext> dbContextOptions = new DbContextOptionsBuilder<InsightContext>()
			.UseInMemoryDatabase(databaseName: "InsightTestDB")
			.Options;

		[TestCase(FileType.AEF, typeof(DigestAEF))]
		[TestCase(FileType.AlphaRoster, typeof(DigestAlphaRoster))]
		[TestCase(FileType.ETMS, typeof(DigestETMS))]
		[TestCase(FileType.LOX, typeof(DigestLOX))]
		[TestCase(FileType.PEX, typeof(DigestPEX))]
		[TestCase(FileType.SFMIS, typeof(DigestSFMIS))]
		public void DigestFactoryTestCases(FileType input, Type expected)
		{
			IDigest digest = DigestFactory.GetDigestor(input, fileContents: null, dbContextOptions);

			digest.Should().BeOfType(expected);
		}

		/// <summary>
		/// Tests factory creating IDgest for Unknownk FileType
		/// </summary>
		[Test]
		public void DigestFactoryUnknown()
		{
			FileType input = FileType.Unknown;

			IDigest digest = DigestFactory.GetDigestor(input, fileContents: null, dbContextOptions);

			digest.Should().BeNull();
		}
	}
}
