using System;
using System.Collections.Generic;
using System.Linq;
using Insight.Core.Models;
using Insight.Core.Properties;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.File
{
	public class DigestAlphaRoster : AbstractDigest, IDigest
	{
		private int _lastNameIndex = -1;
		private int _firstNameIndex = -1;
		private int _ssnIndex = -1;
		private int _gradeIndex = -1;
		private int _homePhoneIndex = -1;
		private int _dateOnStationIndex = -1;
		private int _pafsc = -1;
		private int _cafsc = -1;
		private int _dafsc = -1;

		int IDigest.Priority => 1;

		public DigestAlphaRoster(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions) : base(FileContents, dbContextOptions)
		{

		}

		public void CleanInput()
		{
			// TODO dialog exception for schema differences
			if (!FileContents[0].StartsWith(Resources.AlphaRosterExpected))
			{
				throw new NotImplementedException();
			}

			SetColumnIndexes(FileContents[0].Split(','));
			FileContents.RemoveAt(0);
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

			_lastNameIndex = Array.IndexOf(columnHeaders, "FULL_NAME");
			_firstNameIndex = _lastNameIndex + offset;
			_gradeIndex = Array.IndexOf(columnHeaders, "GRADE") + offset;
			_ssnIndex = Array.IndexOf(columnHeaders, "SSAN") + offset;
			_homePhoneIndex = Array.IndexOf(columnHeaders, "HOME_PHONE_NUMBER") + offset;
			_dateOnStationIndex = Array.IndexOf(columnHeaders, "DATE_ARRIVED_STATION") + offset;
			_pafsc = Array.IndexOf(columnHeaders, "PAFSC") + offset;
			_cafsc = Array.IndexOf(columnHeaders, "CAFSC") + offset;
			_dafsc = Array.IndexOf(columnHeaders, "DAFSC") + offset;
		}

		public void DigestLines()
		{
			foreach (string line in FileContents)
			{
				var splitLine = line.Split(',').Select(d => d.Trim()).ToArray();

				string firstName = splitLine[_firstNameIndex].Replace("\"", "").Trim();
				string lastName = splitLine[_lastNameIndex].Replace("\"", "").Trim();
				string grade = splitLine[_gradeIndex];
				string ssn = splitLine[_ssnIndex].Replace("-", "");
				DateTime dateOnStation = DateTime.Parse(splitLine[_dateOnStationIndex]);
				string homePhone = splitLine[_homePhoneIndex];
				AFSC afsc = base.GetOrCreateAFSC(pafsc: splitLine[_pafsc], cafsc: splitLine[_cafsc], dafsc: splitLine[_dafsc]);

				//TODO look for existing person and update if it exists. Lookup by name and SSN
				var person = insightController.GetPersonByName(firstName, lastName).Result;

				// If you don't find the person (because we value LOXs, throw them out)
				if (person == null) continue;

				person.SSN = ssn;
				person.DateOnStation = dateOnStation;
				person.HomePhone = homePhone;
				person.AFSC = afsc;
				person.Rank = grade;

				insightController.Update(person);
			}
		}
	}
}
