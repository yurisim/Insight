using Insight.Core.Models;
using Insight.Core.Services.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
		protected IList<string> FileContents { get; set; } = new List<string>();

		/// <summary>
		/// Abstract class for Digest classes to implement
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
		protected AFSC GetOrCreateAFSC(string pafsc, string cafsc, string dafsc)
		{
			//TODO handle picking which person in the frontend
			AFSC afsc = insightController.GetAFSCs(pafsc: pafsc).Result.FirstOrDefault();

			//TODO does not upadate CAFSC/DAFSC if they're missing
			//AFSC exists, returns it
			if (afsc != null) { return afsc; }

			//AFSC does not already exists, creates it
			afsc = new AFSC()
			{
				PAFSC = pafsc,
				CAFSC = cafsc,
				DAFSC = dafsc,
			};

			insightController.Add(afsc);

			return afsc;
		}

		/// <summary>
		/// Determines which course on ETMS FileContents is for
		/// </summary>
		protected Course GetOrCreateCourse(string name)
		{
			//TODO handle picking which person in the frontend
			Course course = insightController.GetCoursesByName(name).Result.FirstOrDefault();

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
