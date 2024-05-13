using UniversalSearchCriteria.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace UniversalSearchCriteria
{
    public class MyDbContext : DbContext
    {
        // Constructor that initializes the DbContext with the database connection string
        // Change connection string and value depends from your DB (Properties/Settings.Settings)
        public MyDbContext() : base(Properties.Settings.Default.DbConnection)
        {
        }

        // DbSet properties for accessing the entities in the database
        public DbSet<SearchBook> Books { get; set; }
        public DbSet<SearchAuthor> Authors { get; set; }

        // Method called when the model for the context is being created
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // SearchBook entity configuration
            modelBuilder.Entity<SearchBook>()
                .HasKey(b => b.Id)
                .Property(b => b.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<SearchBook>()
                .Property(b => b.Title)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<SearchBook>()
                .Property(b => b.Author)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<SearchBook>()
                .Property(b => b.Status)
                .HasMaxLength(15)
                .IsRequired();

            modelBuilder.Entity<SearchBook>()
                .Property(b => b.PublicationDate)
                .HasColumnType("datetime2")
                .IsRequired();

            // SearchAuthor entity configuration
            modelBuilder.Entity<SearchAuthor>()
                .HasKey(a => a.Id)
                .Property(a => a.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<SearchAuthor>()
                .Property(a => a.Name)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<SearchAuthor>()
                .Property(a => a.Country)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<SearchAuthor>()
                .Property(a => a.BirthDate)
                .HasColumnType("datetime2")
                .IsRequired();
        }
    }
}
