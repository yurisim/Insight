using System;
using System.Collections.Generic;
using Insight.Core.Properties;
using Insight.Core.Services.Database;

namespace Insight.Core.Services.FileProcessors
{
	public class DigestPEX : IDigest
	{
		private readonly IList<string> input = new List<string>();

		public DigestPEX(IList<string> input)
		{
			this.input = input;
		}

		public void DigestLines()
		{
			// TODO dialog exception for schema differences
			if (!input[0].StartsWith(Resources.PEXExpectedSchema))
			{
				throw new NotImplementedException();
			}

			// We start at i = 1 so that we ignore the initial schema.
			for (var lineIndex = 1; lineIndex < input.Count; lineIndex++)
			{
				string[] digestedLines = input[lineIndex].Split(',');

				// short name of person, format is "SmithJ" if name is "John Smith"
				string shortName = digestedLines[0];

				// Flight Designation 
				string PEXName = digestedLines[1];

				// Now try to find the name of the person
				// Find all people who have the short Name
				var foundPerson = Interact.GetPersonByShortName(shortName);

				// try to find the PEX Account
				foundPerson.Flight = digestedLines[1];

				Interact.Update(foundPerson);

			}
		}
	}
}