using Insight.Core.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using Insight.Core.Models;
using Insight.Core.Services;
using Insight.Core.Services.File;
using System.Diagnostics;

namespace Insight.Core.Services.File
{
	/// <summary>
	/// The digestor, given a filetype, will provide custom handling of a file.
	/// </summary>
	public class Digestor
	{
		public FileType FileType { get; private set; }
		public List<string> FileContents { get; private set; }

		public Digestor(FileType fileType, List<string> fileContents)
		{
			this.FileType = fileType;
			this.FileContents = fileContents;
		}

		/// <summary>
		/// Calls DigestLines for the file based on the filetype.
		/// </summary>
		/// <param name="file">The file to digest.</param>
		/// <returns>A list of DigestLines.</returns>
		public void Digest()
		{
			switch (FileType)
			{
				case FileType.AlphaRoster:
					var digestAlphaRoster = new DigestAlphaRoster(FileContents);
					digestAlphaRoster.DigestLines();
					Debug.WriteLine(FileType.AlphaRoster);
					break;
				case FileType.PEX:
					var digestPEX = new DigestPEX(FileContents);
					digestPEX.DigestLines();
					Debug.WriteLine(FileType.PEX);
					break;
				case FileType.AEF:
					var digestAEF = new DigestAEF(FileContents);
					digestAEF.DigestLines();
					Debug.WriteLine(FileType.AEF);
					break;
				case FileType.ETMS:
					var digestETMS = new DigestETMS(FileContents);
					digestETMS.DigestLines();
					break;
				case FileType.LOX:
					var digestLOX = new DigestLOX(FileContents);
					digestLOX.DigestLines();
					Debug.WriteLine(FileType.LOX);
					break;
			}
		}
	}
}
