using BurgerShopOrdering.core.Data.Seeding;
using BurgerShopOrdering.core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.core.Data
{
    public class BurgerShopDbContext(DbContextOptions<BurgerShopDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .Property(a => a.UserName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Entity<ApplicationUser>()
                .Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.Entity<ApplicationUser>()
                .Property(a => a.FirstName)
                .HasMaxLength(100);
            builder.Entity<ApplicationUser>()
                .Property(a => a.LastName)
                .HasMaxLength(100);

            builder.Entity<Order>()
                .Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .IsRequired()
                .HasColumnType("money");
            builder.Entity<Order>()
                .Property(o => o.Quantity)
                .IsRequired();
            builder.Entity<Order>()
                .Property(o => o.Status)
                .IsRequired();
            builder.Entity<Order>()
                .Property(o => o.ApplicationUserId)
                .IsRequired();
            builder.Entity<Order>()
                .Property(o => o.DateOrdered)
                .IsRequired();

            builder.Entity<OrderItem>()
                .Property(o => o.Quantity)
                .IsRequired();
            builder.Entity<OrderItem>()
                .Property(o => o.Price)
                .IsRequired()
                .HasColumnType("money");
            builder.Entity<OrderItem>()
                .Property(o => o.OrderId)
                .IsRequired();
            builder.Entity<OrderItem>()
                .Property(o => o.ProductId)
                .IsRequired();

            builder.Entity<Category>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Entity<Product>()
                .Property(p => p.Price)
                .IsRequired()
                .HasColumnType("money");
            builder.Entity<Product>()
                .Property(p => p.Image)
                .IsRequired();

            Seeder.Seed(builder);
        }
    }
}
