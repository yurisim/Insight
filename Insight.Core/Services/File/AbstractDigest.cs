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
		/// Determines which course on ETMS FileContents is for
		/// </summary>
		protected Course GetOrCreateCourse(string name)
		{
			Course course = insightController.GetCourseByName(name);

			//if a course for passed name already exists, return it
			if (course != null) { return course; }

			//course does not exist, create it
			// TODO make custom intervals for each course. Default is hard coded to 1 year
			course = new Course()
			{
				Name = name,
				Interval = 1
			};

			insightController.Add(course);

			return course;
		}
	}
}