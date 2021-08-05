using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Insight.Core.Helpers;
using Insight.Core.Models;
using Insight.Core.Properties;
using Insight.Core.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.File
{
	public class DigestLOX : AbstractDigest, IDigest
	{
		int IDigest.Priority { get => 0; }

		int NameIndex;
		int MDSIndex;
		int RankIndex;
		int FlightIndex;
		int CPIndex;


		bool HeadersProcessed = false;

		public DigestLOX(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions) : base(FileContents, dbContextOptions)
		{

		}

		public void DigestLines()
		{
			//Assumptions about schema:
			//There are 5 lines of the file are irrelevant information
			//Column headers come directly after those first 5 lines.
			//All relavent person data starts with a '\"' (double quotes) character.
			//	-this assumption is not necessarily required.
			//The column headers are "Name", "MDS", "Rank", and "Flight" etc
			//The name is in the format "Lastname, Firstname" (capitalization or surrounding quotes not important. Order and separating comments are).
			//After all person data there is a completely empty line (in a .csv that is denoated by a string of only commas)
			//Everything after that empty line is irrelevant information

			//Assumptions about the data:
			//Any person with "E-3G(II)" (opposed to "E-3G") can be disregarded.

			string Squadon = "";

			for (int i = 0; i < FileContents.Count; i++)
			{
				List<string> data = new List<string>(FileContents[i].Split(','));
				int offset = 1; //this offset is to account for the comma in the Name field
				if (!HeadersProcessed)
				{
					Regex regexSquadron = new Regex(@"^Squadron: (.+?),");
					if (regexSquadron.IsMatch(FileContents[i]))
					{
						Squadon = regexSquadron.Match(FileContents[i]).Groups[1].Value;
					}

					else if (!FileContents[i].Contains("CONTROLLED UNCLASSIFIED INFORMATION")
						&& !FileContents[i].Contains("(Controlled with Standard Dissemination)")
						&& !FileContents[i].Contains("Letter of Certifications")
						&& !(FileContents[i].Contains("Flight Quals") && FileContents[i].Contains("Dual Qual")))
					{
						//TODO handle column mising
						// TODO Handle different casing/handle schema better for variations
						NameIndex = data.IndexOf("Name");
						CPIndex = data.IndexOf("CP");
						MDSIndex = data.IndexOf("MDS");
						RankIndex = data.IndexOf("Rank");
						FlightIndex = data.IndexOf("Flight");
						HeadersProcessed = true;
					}
				}
				else if (new Regex("^,+$").IsMatch(FileContents[i]))
				{

					break;
				}
				else if (FileContents[i].StartsWith("\""))
				{
					string LastName = data[NameIndex].Replace("\"", "").Trim().ToUpperInvariant();
					string FirstName = data[NameIndex + offset].Replace("\"", "").Trim().ToUpperInvariant();
					string crewPosition = data[CPIndex + offset].Trim();
					string MDS = data[MDSIndex + offset].Trim();
					string Rank = data[RankIndex + offset].Trim();
					string Flight = data[FlightIndex + offset].Trim();

					//skips people who have a MDS of "E-3G(II)"
					if (MDS == "E-3G(II)")
					{
						continue;
					}

					Org org = insightController.GetOrgByAlias(Squadon);
					if(org == null)
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

					var person = insightController.GetPersonByName(FirstName, LastName).Result;

					//This will assume if person is null at this point that a new one needs to be created.
					if (person == null)
					{
						person = new Person()
						{
							FirstName = FirstName,
							LastName = LastName,
							CrewPosition = crewPosition,
							Medical = new Medical(),
							Training = new Training(),
							Personnel = new Personnel(),
							PEX = new PEX(),
							//Organization = org,
						};
						insightController.Add(person);
					}

					person.Flight = Flight;
					person.Organization = org;
					person.CrewPosition = crewPosition;
					//person.Rank = ;

					insightController.Update(person);
				}
			}
		}
	}
}
