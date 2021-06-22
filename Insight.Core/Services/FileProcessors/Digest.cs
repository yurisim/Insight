using System;
using System.Collections.Generic;
using System.Text;
using Insight.Core.Models;
using Insight.Core.Properties;

namespace Insight.Core.Services.FileProcessors
{
   public class DigestAlphaRoster : IDigest
   {
      private readonly List<string> input = new List<string>();

      public DigestAlphaRoster(List<string> input)
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
         for (int lineIndex = 1; lineIndex < input.Count; lineIndex++)
         {
            string[] digestedLines = input[lineIndex].Split(',');

            Person person = new Person()
            {
               FirstName = digestedLines[0].Split(',')[1].Trim(),
               LastName = digestedLines[0].Split(',')[0].Trim(),
               Phone = digestedLines[42],
               SSN = digestedLines[1],
               DateOnStation = digestedLines[16],

               // TODO get AFSC from alpha roster and create/use existing in database
               //AFSC =

               // TODO get Organization from alpha roster and create/use existing in database
               //Organization =
            };
         }
      }
   }
}