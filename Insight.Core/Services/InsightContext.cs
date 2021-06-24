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

        public DbSet<Org> Orgs { get; set; }

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
            modelBuilder.Entity<Org>().ToTable("Orgs");
            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<PEX>().ToTable("PEXs");
            modelBuilder.Entity<TBA>().ToTable("TBAs");

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

            modelBuilder.Entity<Medical>()
                .Property(entity => entity.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Org>()
                .Property(entity => entity.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PEX>()
                .Property(entity => entity.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TBA>()
               .Property(entity => entity.Id)
                .ValueGeneratedOnAdd();

            // Configured ono-to-one relationship between Person and Medical
            modelBuilder.Entity<Person>()
              .HasOne(p => p.Medicals)
              .WithOne(p => p.Person)
              .HasForeignKey<Medical>(m => m.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}