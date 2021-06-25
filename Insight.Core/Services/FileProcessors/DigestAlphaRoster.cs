using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Insight.Core.Models;
using Insight.Core.Properties;
using Insight.Core.Services.Database;

namespace Insight.Core.Services.FileProcessors
{
   public class DigestAlphaRoster : IDigest
   {
      private readonly IList<string> input = new List<string>();

      public DigestAlphaRoster(IList<string> input)
      {
         this.input = input;
      }

      public void DigestLines()
      {
         // TODO dialog exception for schema differences
         if (!input[0].StartsWith(Resources.AlphaRosterExpectedSchema))
         {
            throw new NotImplementedException();
         }

         // We start at i = 1 so that we ignore the initial schema.
         for (var lineIndex = 1; lineIndex < input.Count; lineIndex++)
         {
            string[] digestedLines = input[lineIndex].Split(',');

            var person = new Person()
            {
               LastName = ConvertToTitleCase(digestedLines[0].Substring(1).ToLower()),
               FirstName = ConvertToTitleCase(digestedLines[1].Substring(0, digestedLines[1].Length - 1).ToLower()),
               Phone = digestedLines[43],
               SSN = digestedLines[2],
               DateOnStation = digestedLines[17],

               // TODO get AFSC from alpha roster and create/use existing in database
               //AFSC =

               // TODO get Organization from alpha roster and create/use existing in database
               //Organization =
            };

            Interact.AddPerson(person);
         }
      }

        //TODO move to helper
      public string ConvertToTitleCase(string improperCase)
      {
         var ti = CultureInfo.CurrentCulture.TextInfo;

         return ti.ToTitleCase(improperCase);
      }
   }
}