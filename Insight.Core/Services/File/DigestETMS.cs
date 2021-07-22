using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Insight.Core.Helpers;
using Insight.Core.Models;
using Insight.Core.Properties;
using Insight.Core.Services.Database;

namespace Insight.Core.Services.File
{
	public class DigestETMS : IDigest
	{
		int IDigest.Priority { get => 3; }

		private readonly IList<string> FileContents = new List<string>();

		public DigestETMS(IList<string> input)
		{
			this.input = CleanInput(input);
		}

		private IList<string> CleanInput(IList<string> inputToClean)
		{
			foreach (var line in inputToClean)
			{
				var splitLine = line.Split(',');

				if (string.IsNullOrEmpty(splitLine[4]))
				{
					inputToClean.Remove(line);
				}
			}

			return inputToClean;
		}

		public void DigestLines()
		{
			// TODO dialog exception for schema differences
			if (!input[0].StartsWith(Resources.AlphaRosterExpected))
			{
				throw new NotImplementedException();
			}

			// We start at i = 1 so that we ignore the initial schema.
			for (var lineIndex = 1; lineIndex < input.Count; lineIndex++)
			{
				

			}
		}



	}
}
