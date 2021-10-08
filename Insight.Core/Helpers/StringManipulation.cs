using System;
using System.Collections.Generic;
using Insight.Core.Models;
using System.Globalization;
using System.Linq;

namespace Insight.Core.Helpers
{
	public static class StringManipulation
	{
		/// <summary>
		/// Converts string to have proper title case.
		/// </summary>
		/// <param name="improperCase"></param>
		/// <returns></returns>
		public static string ConvertToTitleCase(string improperCase)
		{
			if (improperCase == null)
			{
				return null;
			}
			var ti = CultureInfo.CurrentCulture.TextInfo;

			return ti.ToTitleCase(improperCase.ToLower().Trim());
		}

		/// <summary>
		/// Converts a status code (g/y/r) to the Status enum
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static Status StatusReader(string input)
		{
			switch (input?.ToLower())
			{
				case "g":
					return Status.Current;
				case "y":
					return Status.Upcoming;
				case "r":
					return Status.Overdue;
				default:
					return Status.Unknown;
			}
		}

		/// <summary>
		/// Returns a list of failed file names with new lines spaced in between.
		/// </summary>
		/// <param name="failedFileNames"></param>
		/// <returns></returns>
		public static string FileNameFormatter(IEnumerable<string> failedFileNames)
		{
			var result = string.Empty;

			foreach (var name in failedFileNames)
			{
				result = result + Environment.NewLine + name;
			}

			return result;
		}
	}
}
