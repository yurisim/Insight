using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insight.Core.Services.File
{
	public static class DigestFactory
	{
		//TDOD refactor this duplicated code. Would be easier to do if we were in C# 9.0 with nullable reference types
		public static IDigest GetDigestor(FileType fileType, IList<string> fileContents)
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
					return new DigestETMS(fileContents);
				case FileType.LOX:
					return new DigestLOX(fileContents);
				default:
					//TODO Throw custom exception indicating the digestor requested hasn't been implemented yet.
					return null;
			}
		}

		public static IDigest GetDigestor(FileType fileType, IList<string> fileContents, DbContextOptions<InsightContext> dbContextOptions)
		{
			switch (fileType)
			{
				case FileType.AlphaRoster:
					return new DigestAlphaRoster(fileContents, dbContextOptions);
				case FileType.PEX:
					return new DigestPEX(fileContents, dbContextOptions);
				case FileType.AEF:
					return new DigestAEF(fileContents, dbContextOptions);
				case FileType.ETMS:
					return new DigestETMS(fileContents, dbContextOptions);
				case FileType.LOX:
					return new DigestLOX(fileContents, dbContextOptions);
				default:
					//TODO Throw custom exception indicating the digestor requested hasn't been implemented yet.
					return null;
			}
		}
	}
}
