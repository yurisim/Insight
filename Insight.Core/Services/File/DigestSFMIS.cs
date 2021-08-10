using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Insight.Core.Helpers;
using Insight.Core.Models;
using Insight.Core.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.File
{
	public class DigestSFMIS : AbstractDigest, IDigest
	{
		int IDigest.Priority { get => 5; }

		public DigestSFMIS(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions) : base(FileContents, dbContextOptions)
		{

		}

		public void CleanInput()
		{
			//headers found and indexes set for the columns to be digested
			bool headersProcessed = false;

			for (int i = 0; i < FileContents.Count; i++)
			{
				string[] splitLine = FileContents[i].Split(',');
				string lineToUpper = FileContents[i].ToUpper();
				
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

			
		}

		public void DigestLines()
		{
			for (int i = 0; i < FileContents.Count - 1; i++)
			{
				string[] splitLine = FileContents[i].Split(',');

				
			}
		}
	}
}
