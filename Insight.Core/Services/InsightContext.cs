using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services
{
	public class InsightContext : DbContext
	{
		public InsightContext(DbContextOptions<InsightContext> options) : base(options)
		{
		}

		public DbSet<AFSC> AFSCs { get; set; }

		public DbSet<Course> Courses { get; set; }

		public DbSet<CourseInstance> CourseInstances { get; set; }

		public DbSet<Medical> Medicals { get; set; }

		public DbSet<Training> Trainings { get; set; }

		public DbSet<Personnel> Personnels { get; set; }

		public DbSet<Org> Orgs { get; set; }

		public DbSet<GradeAlias> GradeAliases { get; set; }

		public DbSet<OrgAlias> OrgAliases { get; set; }

		public DbSet<Person> Persons { get; set; }

		public DbSet<PEX> PEXs { get; set; }

		public DbSet<TBA> TBAs { get; set; }

		///// <summary>
		/////
		///// </summary>
		///// <param name="optionsBuilder"></param>
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlite($"Filename={DBName}");

		//	base.OnConfiguring(optionsBuilder);
		//}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Map table names
			modelBuilder.Entity<AFSC>().ToTable("AFSCs");
			modelBuilder.Entity<Course>().ToTable("Courses");
			modelBuilder.Entity<CourseInstance>().ToTable("CourseInstances");
			modelBuilder.Entity<Medical>().ToTable("Medicals");
			modelBuilder.Entity<Training>().ToTable("Trainings");
			modelBuilder.Entity<Personnel>().ToTable("Personnels");
			modelBuilder.Entity<Org>().ToTable("Orgs");
			modelBuilder.Entity<OrgAlias>().ToTable("OrgAliases");
			modelBuilder.Entity<GradeAlias>().ToTable("GradeAliases");

			modelBuilder.Entity<Person>().ToTable("Persons");
			modelBuilder.Entity<PEX>().ToTable("PEXs");
			modelBuilder.Entity<TBA>().ToTable("TBAs");

			//modelBuilder.Entity<Person>()
			//    .HasKey(c => new { c.PersonId });

			// This makes the primary key of the below entity
			modelBuilder.Entity<AFSC>()
				.Property(entity => entity.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Course>()
				.Property(entity => entity.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<CourseInstance>()
				.Property(entity => entity.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<PEX>()
				.Property(entity => entity.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<TBA>()
			   .Property(entity => entity.Id)
				.ValueGeneratedOnAdd();

			base.OnModelCreating(modelBuilder);


		}
	}
}
