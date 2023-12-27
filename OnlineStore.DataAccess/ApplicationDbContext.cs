using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Models;

namespace OnlineStore.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //DB sections
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<StockEvent> StockEvents { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        // End of DB sections

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>()
                .HasOne(e => e.Clerk)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // <--

            builder.Entity<Order>()
                .HasOne(e => e.Customer)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // <--
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .HasOne(p => p.Stock)
                .WithOne(s => s.Product)
                .HasForeignKey<Stock>(s => s.ProductID)
                .IsRequired(false);
            //ApplicationDbContextConfigurations.Configure(builder);
            //ApplicationDbContextConfigurations.SeedData(builder);
        }
    }
}
