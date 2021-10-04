using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.File
{
	public class DigestPEX : AbstractDigest, IDigest
	{
		private int _shortNameIndex = -1;
		private int _pexDesignationIndex = -1;

		int IDigest.Priority { get => 4; }

		public DigestPEX(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions) : base(FileContents, dbContextOptions)
		{

		}

		public void CleanInput()
		{
			//headers found and indexes set for the columns to be digested
			bool headersProcessed = false;

			for (var i = 0; i < FileContents.Count; i++)
			{
				string[] splitLine = FileContents[i].Split(',');
				if (!headersProcessed)
				{
					SetColumnIndexes(splitLine);
					headersProcessed = true;
					FileContents.RemoveAt(i);
					i--;
				}
			}
		}

		/// <summary>
		/// Sets the indexes for columns of data that needs to be digested
		/// </summary>
		/// <param name="columnHeaders">Represents the row of headers for data columns</param>
		private void SetColumnIndexes(string[] columnHeaders)
		{
			//Converts everything to upper case for comparison
			columnHeaders = columnHeaders.Select(d => d.ToUpper().Trim()).ToArray();

			_shortNameIndex = Array.IndexOf(columnHeaders, "SHORTNAME");
			_pexDesignationIndex = Array.IndexOf(columnHeaders, "PEX DESIGNATION");
		}

		public void DigestLines()
		{
			foreach (string line in FileContents)
			{
				var splitLine = line.Split(',').Select(d => d.Trim()).ToArray();

				// short name of person, format is "SmithJ" if name is "John Smith"
				string shortName = splitLine[_shortNameIndex];

				// Flight Designation 
				string pexName = splitLine[_pexDesignationIndex];

				// Now try to find the name of the person
				// Find all people who have the short Name
				var foundPersons = insightController.GetPersonsByShortName(shortName)?.Result;

				//TODO handle picking which person in the frontend
				var person = foundPersons.FirstOrDefault();

				if (foundPersons != null && foundPersons.Count > 0)
				{
					person.Flight = pexName;

					insightController.Update(person);
				}

				// try to find the PEX Account
			}
		}
	}
}
