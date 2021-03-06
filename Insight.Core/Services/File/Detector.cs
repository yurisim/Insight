using Insight.Core.Properties;
using System.Collections.Generic;
using Insight.Core.Models;
using System.Linq;

namespace Insight.Core.Services.File
{
	/// <summary>
	/// The detector detects what type of files pass through it and return the appropriate designation
	/// </summary>
	public class Detector
	{
		private static readonly Dictionary<FileType, string> SupportedFileTypes = new Dictionary<FileType, string>()
			{
				{FileType.AlphaRoster, Resources.AlphaRosterExpected},
				{FileType.PEX, Resources.PEXExpected},
				{FileType.AEF, Resources.AEFExpected},
				{FileType.ETMS, Resources.ETMSExpected},
				{FileType.LOX, Resources.LOXExpected},
				{FileType.SFMIS, Resources.SFMISExpected},
				{FileType.ARIS_Handgun, Resources.ARISHandGunExpected},
				{FileType.ARIS_Rifle_Carbine, Resources.ARISRifleCarbineExpected},
			};

		/// <summary>
		/// Detects the type of file based on the first line of the inputFile
		/// </summary>
		/// <returns></returns>
		public static FileType DetectFileType(IList<string> inputFile)
		{
			if (inputFile == null) { return FileType.Unknown; }

			var firstFewLines = inputFile.Take(4);

			foreach (var line in firstFewLines)
			{
				foreach (var supportedFileType in SupportedFileTypes)
				{
					if (line.IndexOf(supportedFileType.Value, System.StringComparison.OrdinalIgnoreCase) >= 0)
					{
						return supportedFileType.Key;
					}
				}
			}

			// If we get here, we didn't find a match
			return FileType.Unknown;
		}
	}
}
