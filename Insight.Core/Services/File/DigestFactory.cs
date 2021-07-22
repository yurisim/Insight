﻿using Insight.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insight.Core.Services.File
{
	public static class DigestFactory
	{
		public static IDigest GetDigestor(FileType fileType, List<string> fileContents)
		{
			switch (fileType)
			{
				case FileType.AlphaRoster:
					return new DigestAlphaRoster(fileContents);
				case FileType.PEX:
					return new DigestPEX(fileContents);
				case FileType.AEF:
					return new DigestAEF(fileContents);
				case FileType.ETMS:
					// TODO: Implement ETMS
					// Instead of returning null, do try/catch and throw custom exception of digestor not implemented (or something similar)
					return null;
				case FileType.LOX:
					return new DigestLOX(fileContents);
				default:
					// Throw custom exception indicating the digestor requested hasn't been implemented yet.
					return null;
			}
		}
	}
}
