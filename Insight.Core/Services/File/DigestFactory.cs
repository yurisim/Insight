using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Insight.Core.Services.File
{
	public static class DigestFactory
	{
		/// <summary>
		/// Creates IDigest based on fileType
		/// </summary>
		/// <param name="fileType">Type of file digest to be created</param>
		/// <param name="fileContents">Contents of file to be digested</param>
		/// <param name="dbContextOptions"></param>
		/// <returns>Returns IDigest object, can be null</returns>
		[CanBeNull]
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
				case FileType.ARIS_Handgun:
				case FileType.ARIS_Rifle_Carbine:
					return new DigestARIS(fileContents, dbContextOptions);
				case FileType.Unknown:
					return null;
				default:
					return null;
			}
		}
	}
}
