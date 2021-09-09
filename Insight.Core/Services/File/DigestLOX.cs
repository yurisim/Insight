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
		int IDigest.Priority { get => 0; }

		//indexes of columns of the named piece of data. Set to -1 so that they're not defaulted to 0, since 0 is possible desired/valid index.
		private int _firstNameIndex = -1;
		private int _lastNameIndex = -1;
		private int _mdsIndex = -1;
		private int _rankIndex = -1;
		private int _flightIndex = -1;
		private int _crewPositionIndex = -1;

		private string _squadron = "";

		public DigestLOX(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions) : base(FileContents, dbContextOptions)
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
			bool endOfDataReached = false;

			for (int i = 0; i < FileContents.Count; i++)
			{
				string[] splitLine = FileContents[i].Split(',');

				//if column headers are not processed yet, we're still in the top section of the file before the person data
				if (!headersProcessed)
				{
					string lineToUpper = FileContents[i].ToUpper();

					//finds squadron string
					Regex regexSquadron = new Regex(@"^Squadron: (.+?),", RegexOptions.IgnoreCase);
					if (regexSquadron.IsMatch(FileContents[i]))
					{
						_squadron = regexSquadron.Match(FileContents[i]).Groups[1].Value;
					}
					//checks for strings that identify lines that can be ignored
					else if (!lineToUpper.Contains("CONTROLLED UNCLASSIFIED INFORMATION")
						&& !lineToUpper.Contains("FOR OFFICAL USE ONLY")
						&& !lineToUpper.Contains("CONTROLLED WITH STANDARD DISSEMINATION")
						&& !lineToUpper.Contains("LETTER OF CERTIFICATIONS")
						&& !(lineToUpper.Contains("FLIGHT QUALS") && lineToUpper.Contains("DUAL QUAL")))
					{
						//Sets the index of the data columnns that need to be accessed
						SetColumnIndexes(splitLine);

						headersProcessed = true;
					}
					FileContents.RemoveAt(i);
					i--;
				}
				//person data and the end of person data can only be reached after column headers are processed
				else
				{
					//checls if end of person data reached. assumes a completely empty line signals of person data
					if (new Regex("^,+$").IsMatch(FileContents[i]))
					{
						endOfDataReached = true;
					}

					//remove persons with a mds of "E-3G(II)" or anything after the end of data
					if (splitLine[_mdsIndex].Trim() == "E-3G(II)" || endOfDataReached)
					{
						FileContents.RemoveAt(i);
						i--;
					}
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

			int offset = 1;  //this offset is to account for the comma in the Name field

			_lastNameIndex = Array.IndexOf(columnHeaders, "NAME");
			_firstNameIndex = _lastNameIndex + offset;
			_crewPositionIndex = Array.IndexOf(columnHeaders, "CP") + offset;
			_mdsIndex = Array.IndexOf(columnHeaders, "MDS") + offset;
			_rankIndex = Array.IndexOf(columnHeaders, "RANK") + offset;
			_flightIndex = Array.IndexOf(columnHeaders, "FLIGHT") + offset;
		}

		public void DigestLines()
		{
			for (int i = 0; i < FileContents.Count; i++)
			{
				var splitLine = FileContents[i].Split(',').Select(d => d.Trim()).ToArray();

				//TODO handle column mising (index of -1)
				//need to make sure these are trimmed after quotes are removed
				string firstName = splitLine[_firstNameIndex].Replace("\"", "").Trim();
				string lastName = splitLine[_lastNameIndex].Replace("\"", "").Trim();

				string crewPosition = splitLine[_crewPositionIndex];
				string MDS = splitLine[_mdsIndex];
				string rank = splitLine[_rankIndex];
				string flight = splitLine[_flightIndex];

				//return if invalid squadron
				if (String.IsNullOrWhiteSpace(_squadron))
				{
					return;
				}

				var org = insightController.GetOrgByAlias(_squadron).Result;

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
				if (String.IsNullOrWhiteSpace(firstName) || String.IsNullOrWhiteSpace(lastName))
				{
					continue;
				}

				var person = insightController.GetPersonByName(firstName, lastName).Result;

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
