using Insight.Core.Services.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insight.Core.Services.File
{
	public abstract class AbstractDigest
	{
		protected InsightController insightController;

		protected IList<string> FileContents = new List<string>();

		public AbstractDigest(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions)
		{
			this.FileContents = FileContents;
			insightController = dbContextOptions == null ? new InsightController() : new InsightController(dbContextOptions);
		}
	}
}
