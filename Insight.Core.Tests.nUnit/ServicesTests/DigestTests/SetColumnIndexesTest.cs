using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Insight.Core.Models;
using Insight.Core.Services.File;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Tests.nUnit.ServicesTests
{
	public partial class DigestTests
	{
		[TestFixture]
		public class SetColumnIndexesTests
		{
			[Test]
			public void Test()
			{
				string digest = null;

				digest.Should().BeNull();
			}
		}
	}
}
