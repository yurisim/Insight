using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insight.Core.Services.File
{
	public static class DigestFactory
	{
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
				case FileType.SFMIS:
					return new DigestSFMIS(fileContents, dbContextOptions);
				default:
					//TODO Throw custom exception indicating the digestor requested hasn't been implemented yet.
					return null;
			}
		}
	}
}
