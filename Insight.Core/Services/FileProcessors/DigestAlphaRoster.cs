using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Insight.Core.Helpers;
using Insight.Core.Models;
using Insight.Core.Properties;
using Insight.Core.Services.Database;

namespace Insight.Core.Services.FileProcessors
{
	public class DigestAlphaRoster : IDigest
	{
		private readonly IList<string> input = new List<string>();

		public DigestAlphaRoster(IList<string> input)
		{
			this.input = input;
		}

		public void DigestLines()
		{
			// TODO dialog exception for schema differences
			if (!input[0].StartsWith(Resources.AlphaRosterExpectedSchema))
			{
				throw new NotImplementedException();
			}

			// We start at i = 1 so that we ignore the initial schema.
			for (var lineIndex = 1; lineIndex < input.Count; lineIndex++)
			{
				string[] digestedLines = input[lineIndex].Split(',');

				string LastName = StringManipulation.ConvertToTitleCase(digestedLines[0].Substring(1));
				string FirstName = StringManipulation.ConvertToTitleCase(digestedLines[1].Substring(0, digestedLines[1].Length - 1));
				string SSN = digestedLines[2].Replace("-", "");

				//TODO look for existing person and update if it exists. Lookup by name and SSN
				var person = Interact.GetPersonByName(FirstName, LastName);

				if (person == null)
				{
					//TODO input validation
					person = new Person()
					{
						LastName = LastName,
						FirstName = FirstName,
						//Medical = new Medical(),
						//Training = new Training(),
						//Personnel = new Personnel(),
						//PEX = new PEX()

						// TODO get AFSC from alpha roster and create/use existing in database
						//AFSC =

						// TODO get Organization from alpha roster and create/use existing in database
						//Organization =
					};

					//Interact.AddPerson(person);
					//Interact.AddMedical(person.Medical, person);
				}
				person.SSN = SSN;
				person.DateOnStation = digestedLines[17];
				person.Phone = digestedLines[43];
				person.DateOnStation = digestedLines[17];

				Interact.Add(person);
			}
		}
	}
}
