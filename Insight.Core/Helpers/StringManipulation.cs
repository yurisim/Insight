using Insight.Core.Models;
using System.Globalization;

namespace Insight.Core.Helpers
{
	public static class StringManipulation
	{
		/// <summary>
		/// Takes a short name (SmithJ for John Smith) to a partial first name and a last name
		/// </summary>
		/// <param name="shortName"></param>
		/// <returns></returns>
		public static (string, string) ConvertShortNameToNames(string shortName)
		{

			if (string.IsNullOrEmpty(shortName)) return (null, null);

			// break up shortname into first letters and last name
			// if name is SmithJ, then J Smith

			// TODO MAKE MORE FACTORS to find the correct person

			int indexOfCapital;
			for (indexOfCapital = shortName.Length - 1; indexOfCapital >= 0; indexOfCapital--)
			{
				if (char.IsUpper(shortName[indexOfCapital]))
				{
					break;
				}
			}

			//no caps for last name found
			if (indexOfCapital < 0)
			{
				return (null, null);
			}

			var firstLetters = shortName.Substring(indexOfCapital);
			var lastName = shortName.Substring(0, indexOfCapital);

			return (firstLetters, lastName);
		}

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
	}
}
