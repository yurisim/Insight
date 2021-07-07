using Insight.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Insight.Core.Helpers
{ 
  class StringManipulation
  {
    /// <summary>
    /// Converts string to have proper title case.
    /// </summary>
    /// <param name="improperCase"></param>
    /// <returns></returns>
    public static string ConvertToTitleCase(string improperCase)
    {
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
      Status status;
      switch (input.ToLower())
      {
        case "g":
          status = Status.Current;
          break;
        case "y":
          status = Status.Upcoming;
          break;
        case "r":
          status = Status.Overdue;
          break;
        default:
          status = Status.Unknown;
          break;
      }
      return status;
    }
  }
}
