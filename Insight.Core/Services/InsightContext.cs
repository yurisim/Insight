using Insight.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services
{
	internal class InsightContext : DbContext
	{
		/// <summary>
		///
		/// </summary>
		public DbSet<AFSC> AFSCs { get; set; }

		/// <summary>
		///
		/// </summary>
		public DbSet<Course> Courses { get; set; }

		public DbSet<CourseInstance> CourseInstances { get; set; }

		public DbSet<Medical> Medicals { get; set; }

		public DbSet<Training> Trainings { get; set; }

		public DbSet<Personnel> Personnels { get; set; }

		public DbSet<Org> Orgs { get; set; }

		public DbSet<OrgAlias> OrgAliases { get; set; }

		public DbSet<Person> Persons { get; set; }

		public DbSet<PEX> PEXs { get; set; }

		public DbSet<TBA> TBAs { get; set; }

		/// <summary>
		///
		/// </summary>
		private const string DBName = "Insight.db";
		
		/// <summary>
		///
		/// </summary>
		/// <param name="optionsBuilder"></param>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Filename={DBName}");

			base.OnConfiguring(optionsBuilder);
		}

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

			//modelBuilder.Entity<Medical>()
			//    .Property(entity => entity.PersonId)
			//    .ValueGeneratedOnAdd();

			//modelBuilder.Entity<Training>()
			//    .Property(entity => entity.Id)
			//    .ValueGeneratedOnAdd();

			//modelBuilder.Entity<Personnel>()
			//    .Property(entity => entity.Id)
			//    .ValueGeneratedOnAdd();

			//modelBuilder.Entity<Org>()
			//	.Property(entity => entity.Id)
			//	.ValueGeneratedOnAdd();

			//modelBuilder.Entity<OrgAlias>()
			//	.Property(entity => entity.Id)
			//	.ValueGeneratedOnAdd();

			modelBuilder.Entity<PEX>()
				.Property(entity => entity.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<TBA>()
			   .Property(entity => entity.Id)
				.ValueGeneratedOnAdd();

			// TODO Write custom exception to deal w/ duplicate DoDIDs
			//modelBuilder.Entity<Person>()
			//    .HasIndex(b => b.DoDID)
			//    .IsUnique(true);

			//modelBuilder.Entity<Medical>()
			//    .HasIndex(b => b.Id)
			//    .IsUnique(true);

			// Configured ono-to-one relationship between Person and Medical
			//modelBuilder.Entity<Person>()
			//  .HasOne(person => person.Medical)
			//  .WithOne(medical => medical.Person)
			//  .HasForeignKey<Medical>(medical => medical.Id);

			// Configured ono-to-one relationship between Person and Training
			//modelBuilder.Entity<Person>()
			//  .HasOne(p => p.Training)
			//  .WithOne(p => p.Person)
			//  .HasForeignKey<Training>(m => m.Id);

			//// Configured ono-to-one relationship between Person and Personnel
			//modelBuilder.Entity<Person>()
			//  .HasOne(p => p.Personnel)
			//  .WithOne(p => p.Person)
			//  .HasForeignKey<Personnel>(m => m.Id);

			base.OnModelCreating(modelBuilder);
		}
	}
}
