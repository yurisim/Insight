﻿using Insight.Core.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using Insight.Core.Models;
using System.Diagnostics;

namespace Insight.Core.Services.File
{
	/// <summary>
	/// The detector detects what type of files pass through it and return the appropriate designation
	/// </summary>
	public class Detector
	{
		private static Dictionary<FileType, string> SupportedFileTypes = new Dictionary<FileType, string>()
			{
				{FileType.AlphaRoster, Resources.AlphaRosterExpected},
				{FileType.PEX, Resources.PEXExpected},
				{FileType.AEF, Resources.AEFExpected},
				{FileType.ETMS, Resources.ETMSExpected},
				{FileType.LOX, Resources.LOXExpected}
			};

		/// <summary>
		/// Detects the type of file based on the first line of the inputFile
		/// </summary>
		/// <returns></returns>
		public static FileType DetectFileType(IList<string> inputFile)
		{
			if(inputFile == null || inputFile.Count == 0)
			{
				throw new ArgumentNullException("null or length 0");
			}
			var detectedFileType = FileType.Unknown;

			string firstLine = inputFile[0];

			foreach (var supportedFileType in SupportedFileTypes)
			{
				if (firstLine.Contains(supportedFileType.Value))
				{
					detectedFileType = supportedFileType.Key;
					break;
				}
			}

			if (detectedFileType == FileType.Unknown)
			{
				throw new Exception(Resources.UnsupportedFileType);
			}

			return detectedFileType;
		}
	}
}
