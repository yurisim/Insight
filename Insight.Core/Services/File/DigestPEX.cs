using System;
using System.Collections.Generic;
using Insight.Core.Properties;
using Insight.Core.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.File
{
	public class DigestPEX : IDigest
	{
		int IDigest.Priority { get => 4; }

		private readonly IList<string> FileContents = new List<string>();

		private InsightController insightController;

		public DigestPEX(IList<string> FileContents)
		{
			this.FileContents = FileContents;
			insightController = new InsightController();
		}

		public DigestPEX(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions)
		{
			this.FileContents = FileContents;
			insightController = new InsightController(dbContextOptions);
		}

		public void DigestLines()
		{
			// TODO dialog exception for schema differences
			if (!FileContents[0].StartsWith(Resources.PEXExpected))
			{
				throw new NotImplementedException();
			}

			// We start at i = 1 so that we ignore the initial schema.
			for (var lineIndex = 1; lineIndex < FileContents.Count; lineIndex++)
			{
				string[] digestedLines = FileContents[lineIndex].Split(',');

				// short name of person, format is "SmithJ" if name is "John Smith"
				string shortName = digestedLines[0];

				// Flight Designation 
				string PEXName = digestedLines[1];

				// Now try to find the name of the person
				// Find all people who have the short Name
				var foundPerson = insightController.GetPersonByShortName(shortName);

				// try to find the PEX Account
				foundPerson.Flight = digestedLines[1];

				insightController.Update(foundPerson);

			}
		}
	}
}
