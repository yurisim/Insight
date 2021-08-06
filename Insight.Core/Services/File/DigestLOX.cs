using System.Collections.Generic;
using System.Text.RegularExpressions;
using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;

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
		private int _cpIndex = -1;

		private string _squadron = "";


		public DigestLOX(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions) : base(FileContents, dbContextOptions)
		{
			CleanInput(FileContents);
		}

		/// <summary>
		/// Cleans/prepares input for digestion by removing anything that isn't person data and duplicated person data
		/// </summary>
		/// <param name="inputToClean"></param>
		private void CleanInput(IList<string> inputToClean)
		{
			//headers found and indexes set for the columns to be digested
			bool headersProcessed = false;
			//end of file found, no more person data left
			bool endOfDataReached = false;

			for (var i = 0; i < inputToClean.Count; i++)
			{
				var splitLine = new List<string>(inputToClean[i].Split(','));

				//if column headers are not processed yet, we're still in the top section of the file before the person data
				if (!headersProcessed)
				{
					//finds squadron string
					Regex regexSquadron = new Regex(@"^Squadron: (.+?),");
					if (regexSquadron.IsMatch(inputToClean[i]))
					{
						_squadron = regexSquadron.Match(inputToClean[i]).Groups[1].Value;
					}
					//checks for strings that identify lines that can be ignored
					else if (!inputToClean[i].Contains("CONTROLLED UNCLASSIFIED INFORMATION")
						&& !inputToClean[i].Contains("(Controlled with Standard Dissemination)")
						&& !inputToClean[i].Contains("Letter of Certifications")
						&& !(inputToClean[i].Contains("Flight Quals") && inputToClean[i].Contains("Dual Qual")))
					{

						//Sets the index of the data columnns that need to be accessed
						SetColumnIndexes(splitLine);

						headersProcessed = true;
					}
					inputToClean.Remove(inputToClean[i]);
					i--;
				}
				//person data and the end of person data can only be reached after column headers are processed
				else
				{
					//checls if end of person data reached. assumes a completely empty line signals of person data
					if (new Regex("^,+$").IsMatch(inputToClean[i]))
					{

						endOfDataReached = true;
					}

					//remove persons with a mds of "E-3G(II)" or anything after the end of data
					if (splitLine[_mdsIndex].Trim() == "E-3G(II)" || endOfDataReached)
					{
						inputToClean.Remove(inputToClean[i]);
						i--;
					}
				}
			}
			FileContents = inputToClean;
		}

		/// <summary>
		/// Sets the indexes for columns of data that needs to be digested
		/// </summary>
		/// <param name="columnHeaders">Represents the row of headers for data columns</param>
		private void SetColumnIndexes(List<string> columnHeaders)
		{
			//Converts everything to upper case for comparison
			columnHeaders = columnHeaders.ConvertAll(d => d.ToUpper());

			int offset = 1;  //this offset is to account for the comma in the Name field

			_lastNameIndex = columnHeaders.IndexOf("NAME");
			_firstNameIndex = _lastNameIndex + offset;
			_cpIndex = columnHeaders.IndexOf("CP") + offset;
			_mdsIndex = columnHeaders.IndexOf("MDS") + offset;
			_rankIndex = columnHeaders.IndexOf("RANK") + offset;
			_flightIndex = columnHeaders.IndexOf("FLIGHT") + offset;
		}

		public void DigestLines()
		{
			for (int i = 0; i < FileContents.Count; i++)
			{
				List<string> data = new List<string>(FileContents[i].Split(','));

				//TODO handle column mising (index of -1)
				string lastName = data[_firstNameIndex].Replace("\"", "").Trim().ToUpperInvariant();
				string firstName = data[_lastNameIndex].Replace("\"", "").Trim().ToUpperInvariant();
				string crewPosition = data[_cpIndex].Trim();
				string MDS = data[_mdsIndex].Trim();
				string rank = data[_rankIndex].Trim();
				string flight = data[_flightIndex].Trim();

				Org org = insightController.GetOrgByAlias(_squadron);

				//TODO this is here so that org is created. Eventually, the user will have to determine that "960 AACS" is the same as "960 AIRBORNE AIR CTR" to facilitating create orgAliases in database
				if (org == null)
				{
					//TODO ask user to define what org this is
					Org orgNew = new Org()
					{
						Name = "960 AACS",
						Aliases = new List<OrgAlias>(),
					};
					OrgAlias orgAlias = new OrgAlias()
					{
						Name = "960 AIRBORNE AIR CTR",
						Org = orgNew,
					};
					OrgAlias orgAlias2 = new OrgAlias()
					{
						Name = "960 AACS",
						Org = orgNew,
					};
					orgNew.Aliases.Add(orgAlias);
					orgNew.Aliases.Add(orgAlias2);
					insightController.Add(orgNew);
					org = orgNew;
				}

				var person = insightController.GetPersonByName(firstName, lastName).Result;

				//This will assume if person is null at this point that a new one needs to be created.
				if (person == null)
				{
					person = new Person()
					{
						FirstName = firstName,
						LastName = lastName,
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
				//person.Rank = ;

				insightController.Update(person);
			}
		}
	}
}
