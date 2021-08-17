﻿using Insight.Core.Models;
using Insight.Core.Services.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Insight.Core.Services.File
{
	public abstract class AbstractDigest
	{
		/// <summary>
		/// Instantianted InsightController used by the Digest Methods
		/// </summary>
		protected InsightController insightController;

		/// <summary>
		/// Represents the contents of a file to be digested
		/// </summary>
		protected IList<string> FileContents = new List<string>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="FileContents">Represents the contents of a file to be digested</param>
		/// <param name="dbContextOptions">DB Context Options. Determines which database is used</param>
		public AbstractDigest(IList<string> FileContents, DbContextOptions<InsightContext> dbContextOptions)
		{
			this.FileContents = FileContents;

			//default InsightController() constructor uses the live/production database. The dbContextOptions constructor can specify either - uses in tests
			insightController = (dbContextOptions == null) ? new InsightController() : new InsightController(dbContextOptions);
		}

		/// <summary>
		/// Gets AFSC entity associated with name if it exists, creates it otherwise
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		protected AFSC GetOrCreateAFSC(string name)
		{
			AFSC afsc = insightController.GetAFSC(name).Result;

			//AFSC exists, returns it
			if(afsc !=  null) { return afsc; }

			//AFSC does not already exists, creates it
			afsc = new AFSC()
			{
				Name = name,
			};

			insightController.Add(afsc);

			return afsc;
		}

	}
}
