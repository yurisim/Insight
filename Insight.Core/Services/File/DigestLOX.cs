using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Insight.Core.Services.File
{
	public class DigestLOX : AbstractDigest, IDigest
	{
		int IDigest.Priority => 0;

		//indexes of columns of the named piece of data. Set to -1 so that they're not defaulted to 0, since 0 is possible desired/valid index.
		private int _firstNameIndex = -1;
		private int _lastNameIndex = -1;
		private int _mdsIndex = -1;
		private int _rankIndex = -1;
		private int _flightIndex = -1;
		private int _crewPositionIndex = -1;

		private const int Offset = 1; //this offset is to account for the comma in the Name field


		private string _squadron = "";

		private readonly List<string> HeadersToIgnore = new List<string>()
		{
			"CONTROLLED UNCLASSIFIED INFORMATION",
			"FOR OFFICIAL USE ONLY",
			"(CONTROLLED WITH STANDARD DISSEMINATION)",
			"LETTER OF CERTIFICATIONS",
			"FLIGHT QUALS",
			"DUAL QUAL"
		};
	 

		public DigestLOX(IList<string> fileContents, DbContextOptions<InsightContext> dbContextOptions) : base(fileContents, dbContextOptions)
		{

		}

		/// <summary>
		/// Cleans/prepares input for digestion by removing anything that isn't person data and duplicated person data
		/// </summary>
		public void CleanInput()
		{
			//headers found and indexes set for the columns to be digested
			bool headersProcessed = false;

			//end of file found, no more person data left
			var endOfDataReached = false;

			for (int i = 0; i < FileContents.Count; i++)
			{
				var lineUpper = FileContents[i].ToUpper();
				var splitUpperLine = lineUpper.Split(',');

				//if column headers are not processed yet, we're still in the top section of the file before the person data
				if (!headersProcessed)
				{
					// finds squadron string
					// This command helps find "552 ACNS" from "Squadron: 552 ACNS"
					var regexSquadron = new Regex(@"^Squadron: (.+?),", RegexOptions.IgnoreCase);

					if (regexSquadron.IsMatch(lineUpper))
					{
						_squadron = regexSquadron.Match(lineUpper).Groups[1].Value;

					}
					else if (HeadersToIgnore.Any(header => lineUpper.Contains(header)))
					{
						//finds lines that should be ignored and skips them
					}
                    else
                    {
						//through process of elimination, everything has been removed from above the person data except for the column headers
						//Sets the index of the data columns that need to be accessed
						SetColumnIndexes(splitUpperLine);

						headersProcessed = true;
					}

					FileContents.RemoveAt(i);
					i--;
				}
				//person data and the end of person data can only be reached after column headers are processed
				else
				{
					//checks if end of person data reached. assumes a completely empty line signals of person data
					if (new Regex("^,+$").IsMatch(FileContents[i]))
					{
						endOfDataReached = true;
					}

					//remove persons with a mds of "E-3G(II)" or anything after the end of data
					if (splitUpperLine[_mdsIndex].Trim() == "E-3G(II)" || endOfDataReached)
					{
						FileContents.RemoveAt(i);
						i--;
					}
				}
			}
		}

		/// <summary>
		/// Sets the indexes for columns of data that needs to be digested
		/// Values must be in all caps
		/// </summary>
		/// <param name="columnHeaders">Represents the row of headers for data columns</param>
		private void SetColumnIndexes(string[] columnHeaders)
		{
			columnHeaders = columnHeaders.Select(d => d.Trim()).ToArray();

			_lastNameIndex = Array.IndexOf(columnHeaders, "NAME");
			_firstNameIndex = _lastNameIndex + Offset; // needs offset
			_crewPositionIndex = Array.IndexOf(columnHeaders, "CP") + Offset; // needs offset
			_mdsIndex = Array.IndexOf(columnHeaders, "MDS") + Offset; // needs offset
			_rankIndex = Array.IndexOf(columnHeaders, "RANK") + Offset; // needs offset
			_flightIndex = Array.IndexOf(columnHeaders, "FLIGHT") + Offset; // needs offset
		}

		public void DigestLines()
		{
			foreach (string line in FileContents)
			{
				var splitLine = line.Split(',').Select(d => d.Trim()).ToArray();

				//TODO handle column missing (index of -1)
				//need to make sure these are trimmed after quotes are removed
				string firstName = splitLine[_firstNameIndex].Replace("\"", "").Trim();
				string lastName = splitLine[_lastNameIndex].Replace("\"", "").Trim();

				string crewPosition = splitLine[_crewPositionIndex];
				string MDS = splitLine[_mdsIndex];
				string rank = splitLine[_rankIndex];
				string flight = splitLine[_flightIndex];

				//return if invalid squadron
				if (string.IsNullOrWhiteSpace(_squadron))
				{
					return;
				}

				//TODO handle picking which org in the frontend
				var org = insightController.GetOrgsByAlias(_squadron).Result.FirstOrDefault();

				//TODO this is here so that org is created. Eventually, the user will have to determine that "960 AACS" is the same as "960 AIRBORNE AIR CTR" to facilitating create orgAliases in database
				if (org == null)
				{
					//TODO ask user to define what org this is
					var orgNew = new Org()
					{
						Name = _squadron,
						Aliases = new List<OrgAlias>(),
					};

					var orgAlias = new OrgAlias()
					{
						Name = _squadron,
						Org = orgNew,
					};
					orgNew.Aliases.Add(orgAlias);
					insightController.Add(orgNew);
					org = orgNew;
				}

				//continue if invalid first/last name
				if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
				{
					continue;
				}

				//TODO handle picking which person in the frontend
				var person = insightController.GetPersonsByName(firstName, lastName).Result.FirstOrDefault();

				//This will assume if person is null at this point that a new one needs to be created.
				if (person == null)
				{
					person = new Person()
					{
						FirstName = firstName,
						LastName = lastName,
						///TODO change it so these entities aren't created until they're needed
						Medical = new Medical(),
						Training = new Training(),
						Personnel = new Personnel(),
						PEX = new PEX(),
						Rank = rank,
						//Organization = org,
					};
					insightController.Add(person);
				}

				//Update existing person - even if it was just created above
				person.Flight = flight;
				person.Organization = org;
				person.CrewPosition = crewPosition;

				insightController.Update(person);
			}
		}
	}
}
