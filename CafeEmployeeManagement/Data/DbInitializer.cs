
using CafeEmployeeManagement.Data;
using CafeEmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CafeEmployeeManagement.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Check if cafes already exist
            if (context.Cafes.Any())
            {
                return; // Database has been seeded
            }

            // Seed initial cafe data
            var cafes = new Cafe[]
            {
                new Cafe { Id = Guid.NewGuid(), Name = "Cafe Mocha", Description = "A cozy place to enjoy your coffee.", Location = "Downtown", Logo = null },
                new Cafe { Id = Guid.NewGuid(), Name = "The Coffee House", Description = "Specialty coffee and pastries.", Location = "Uptown", Logo = null }
            };

            context.Cafes.AddRange(cafes);
            context.SaveChanges();
        }
    }
}
