using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess
{
    public class ApplicationDbContextConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<IdentityUser>().ToTable("Users");
            //modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }

        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Add any seed data here
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductID = 1, Name = "Product 1", UnitPrice = 9.99f}
            );

        }
    }
}
