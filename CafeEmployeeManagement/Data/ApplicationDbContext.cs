// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;


using CafeEmployeeManagement.Models;

namespace CafeEmployeeManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Cafe> Cafes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Cafe entity
            modelBuilder.Entity<Cafe>()
                .HasKey(c => c.Id); // Primary key

            modelBuilder.Entity<Cafe>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Cafe>()
                .Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<Cafe>()
                .Property(c => c.Location)
                .IsRequired()
                .HasMaxLength(100);

            // Configure Employee entity
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.Id); // Primary key

            modelBuilder.Entity<Employee>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Employee>()
                .Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(10); // Assuming it has 10 digits as per your requirement

            modelBuilder.Entity<Employee>()
                .Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(10); // Assuming 'Male' or 'Female'

            // Configure relationships
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Cafe)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CafeId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if Cafe is deleted

            // Ensure unique employee ID format
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Id)
                .IsUnique(); // Ensures that the employee ID is unique
        }

        //
        //
    } 
}
