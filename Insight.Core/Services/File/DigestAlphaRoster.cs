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
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.File
{
	public class DigestAlphaRoster : AbstractDigest, IDigest
	{
		private int _lastNameIndex = -1;
		private int _firstNameIndex = -1;
		private int _rankIndex = -1;
		private int _ssnIndex = -1;
		private int _phoneIndex = -1;
		private int _dateOnStationIndex = -1;

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
			_rankIndex = Array.IndexOf(columnHeaders, "GRADE") + offset;
			_ssnIndex = Array.IndexOf(columnHeaders, "SSAN") + offset;
			_phoneIndex = Array.IndexOf(columnHeaders, "HOME_PHONE_NUMBER") + offset;
			_dateOnStationIndex = Array.IndexOf(columnHeaders, "DATE_ARRIVED_STATION") + offset;
		}

		public void DigestLines()
		{
			for (int i = 0; i < FileContents.Count; i++)
			{
				string[] splitLine = FileContents[i].Split(',');

				string firstName = splitLine[_firstNameIndex].Replace("\"", "").Trim().ToUpperInvariant();
				string lastName = splitLine[_lastNameIndex].Replace("\"", "").Trim().ToUpperInvariant();
				string rank = splitLine[_rankIndex];
				string ssn = splitLine[_ssnIndex].Replace("-", "");
				string dateOnstation = splitLine[_dateOnStationIndex];
				string phone = splitLine[_phoneIndex];

				//TODO look for existing person and update if it exists. Lookup by name and SSN
				var person = insightController.GetPersonByName(firstName, lastName).Result;

				if (person != null)
				{
					person.SSN = ssn;
					person.DateOnStation = dateOnstation;
					person.Phone = phone;
					insightController.Update(person);
				}				
			}
		}
	}
}
