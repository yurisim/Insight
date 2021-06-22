using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Insight.Core.Services
{
   public static class Ingest
   {
      /// <summary>
      /// Returns true if upload is successful, returns false if failed.
      /// </summary>
      /// <param name="filePath"></param>
      /// <param name="output"></param>
      /// <returns></returns>
      public static bool ReadFile(string filePath, out List<string> output)
      {
         bool status = true;

         output = new List<string>();

         try
         {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(filePath))
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
         catch
         {
            status = false;
         }

         return status;
      }
   }
}