using Insight.Core.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.Database
{
	//TODO: Make this a partial class and have one file/class be for Get functions and another file be for Add/Update. This file is getting too complex.

	public partial class InsightController
	{
		private readonly DbContextOptions<InsightContext> _dbContextOptions;

		public InsightController(DbContextOptions<InsightContext> dbContextOptions)
		{
			_dbContextOptions = dbContextOptions;
		}

		/// <summary>
		/// Whenever the blank constructor is used, it will use the default insight solution. 
		/// </summary>
		public InsightController()
		{
			_dbContextOptions = new DbContextOptionsBuilder<InsightContext>()
			.UseSqlite("Filename=Insight.db")
			.Options;
		}

		/// <summary>
		/// Ensures database has been created.
		/// </summary>
		public async void EnsureDatabaseCreated()
		{
			using (var insightContext = new InsightContext(_dbContextOptions))
			{
				//Ensure database is created
				_ = await insightContext.Database.EnsureCreatedAsync();
			}
		}

		/// <summary>
		/// Ensures database has been deleted.
		/// </summary>
		public async void EnsureDatabaseDeleted()
		{
			using (InsightContext insightContext = new InsightContext(_dbContextOptions))
			{
				//Ensure database is created
				_ = await insightContext.Database.EnsureDeletedAsync();
			}
		}

		/// <summary>
		/// Add entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		public async void Add<T>(T t)
		{
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					_ = await insightContext.AddAsync(t);
					_ = await insightContext.SaveChangesAsync();
				}
			}

			//TODO implement exception
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}

		/// <summary>
		/// Add entity
		/// </summary>
		/// <param name="courseInstance"></param>
		/// <param name="course"></param>
		/// <param name="person"></param>
		public async void AddCourseInstance(CourseInstance courseInstance, Course course, Person person)
		{
			using (var insightContext = new InsightContext(_dbContextOptions))
			{
				course.CourseInstances.Add(courseInstance);
				person.CourseInstances.Add(courseInstance);

				insightContext.Entry(course).State = EntityState.Modified;
				insightContext.Entry(person).State = EntityState.Modified;

				_ = await insightContext.AddAsync(courseInstance);

				_ = await insightContext.SaveChangesAsync();
			}
			//TODO implement exception
		}

		/// <summary>
		/// Update entity in database
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		public async void Update<T>(T t)
		{
			try
			{
				using (InsightContext insightContext = new InsightContext(_dbContextOptions))
				{
					_ = insightContext.Update(t);
					_ = await insightContext.SaveChangesAsync();
				}
			}
			//TODO implement exception
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
	}
}
