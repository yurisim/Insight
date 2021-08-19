using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Core.Tests.nUnit.ServicesTests
{
	public class Helper
	{
		//TODO this is a stop gap to allow me to continue writing tasks. In the future read file needs to be moved out of the front end
		public static IList<string> ReadFile(string filePath)
		{
			IList<string> result = new List<string>();
			using (var sr = new StreamReader(filePath))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					result.Add(line);
				}
			}
			return result;
			//return File.ReadLines(filePath).ToList();
		}
	}

	[TestFixture]
	public class ReadFilesTest
	{
		/// <summary>
		/// Tests that a file is read properly and that the returned List<string> as expected
		/// </summary>
		/// <returns></returns>
		[Test]
		public void ReadGoodFile()
		{
			IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\short_file_test.csv");


			IList<string> Result = new List<string>();
			Result.Add("\"Churchill, Olivia\",ABM,E-3G(II),SMSgt,D,CMR,E,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
			Result.Add("\"Clark, Joshua\",MSO,E-3G,AB,B,CMR,E,,,,,,,,,,,,,,,,,,,X,,X,,,,,,PERS: WDA OGRA,");
			Result.Add("\"Clark, Joshua\",MSO,E-3G(II),AB,B,CMR,E,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: WDA OGRA,");
			Result.Add("\"Hart, Anthony\",ABM,E-3G,TSgt,E,CMR,I,,,,,,,,,,,,,,,,,,,X,,,,,,,,PERS: LTDNIF ,");
			Result.Add("\"Hart, Anthony\",ABM,E-3G(II),TSgt,E,CMR,I,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: LTDNIF ,");
			Result.Add("\"Hart, Grace\",FE,E-3G,MSgt,A,CMR,E,,,,,,,X,,,,,X,X,,,,,,X,,,,,,,,,");
			Result.Add("\"Jones, Stewart\",MCC,E-3G,SrA,E,CMR,I,,,,,,,,,,,,,,,,,,,,,,,,,,,PERS: ABM TP, DNIF");
			Result.Add("\"Jones, Stewart\",MSO,E-3G(II),SrA,E,IQT,I,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
			fileContents.Should().BeEquivalentTo(Result);
		}

		/// <summary>
		/// Tests reading a file that does not exist
		/// </summary>
		/// <returns></returns>
		[Test]
		public void ReadFileThatDoesNotExist()
		{
			IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\this_file_does_not_exist.csv");

			fileContents.Should().BeNull();
		}

		/// <summary>
		/// Tests reading an empty file
		/// </summary>
		/// <returns></returns>
		[Test]
		public void ReadEmptyFile()
		{
			IList<string> fileContents = Helper.ReadFile(@"Test Mock Data\empty_file.csv");

			fileContents.Should().BeNullOrEmpty();
		}
	}
}
