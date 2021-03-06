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

			SetColumnIndexes(FileContents[0].ToUpper().Split(','));
			FileContents.RemoveAt(0);
		}

		/// <summary>
		/// Sets the indexes for columns of data that needs to be digested
		/// Values must be in all caps
		/// </summary>
		/// <param name="columnHeaders">Represents the row of headers for data columns</param>
		private void SetColumnIndexes(string[] columnHeaders)
		{
			//Converts everything to upper case for comparison
			columnHeaders = columnHeaders.Select(d => d.Trim()).ToArray();

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
				// Alpha Roster filetype is detected by looking at the the column header row. If any of the columns are missing, the filetype won't be detected.
				var splitLine = line.Split(',').Select(d => d.Trim()).ToArray();

				string firstName = splitLine.ElementAtOrDefault(_firstNameIndex).Replace("\"", "").Trim();
				string lastName = splitLine.ElementAtOrDefault(_lastNameIndex).Replace("\"", "").Trim();
				string grade = splitLine.ElementAtOrDefault(_gradeIndex);
				string ssn = splitLine.ElementAtOrDefault(_ssnIndex).Replace("-", "");
				DateTime dateOnStation = DateTime.Parse(splitLine[_dateOnStationIndex]);
				string homePhone = splitLine.ElementAtOrDefault(_homePhoneIndex);
				AFSC afsc = base.GetOrCreateAFSC(pafsc: splitLine.ElementAtOrDefault(_pafsc), cafsc: splitLine.ElementAtOrDefault(_cafsc), dafsc: splitLine.ElementAtOrDefault(_dafsc));

				//TODO handle picking which person in the frontend
				var person = insightController.GetPersonsByName(firstName, lastName).Result.FirstOrDefault();

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
