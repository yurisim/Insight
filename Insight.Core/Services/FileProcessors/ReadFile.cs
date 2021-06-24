using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Insight.Core.Models;
using System.Diagnostics;

namespace Insight.Core.Services.FileProcessors
{
   public static class ReadFile
   {
      /// <summary>
      /// Returns true if upload is successful, returns false if failed.
      /// </summary>
      /// <param name="filePath"></param>
      /// <param name="output"></param>
      /// <returns></returns>
      public static bool ReadText(string filePath, out List<string> output)
      {
         var status = true;

         output = new List<string>();

         try
         {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (var sr = new StreamReader(filePath))
            {
               string line;
               // Read and display lines from the file until the end of
               // the file is reached.
               while ((line = sr.ReadLine()) != null)
               {
                  output.Add(line);
               }
            }
         }
         catch (Exception e)
         {
            Debug.WriteLine(e.Message);
            status = false;
         }

         return status;
      }
   }


}