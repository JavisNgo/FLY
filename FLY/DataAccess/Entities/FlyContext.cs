using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLY.DataAccess.Entities;

public partial class FlyContext : DbContext
{
    public FlyContext()
    {
    }

    public FlyContext(DbContextOptions<FlyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<VoucherOfshop> VoucherOfshops { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
                .Property(a => a.AccountId)
                .ValueGeneratedOnAdd();

        modelBuilder.Entity<Shop>()
            .Property(s => s.ShopId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<VoucherOfshop>()
            .Property(v => v.VoucherId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ProductCategory>()
            .Property(pc => pc.ProductCategoryId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Product>()
            .Property(p => p.ProductId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Cart>()
            .Property(c => c.CartId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Blog>()
            .Property(b => b.BlogId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Order>()
            .Property(o => o.OrderId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<OrderDetail>()
            .Property(od => od.OrderDetailId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Feedback>()
            .Property(f => f.FeedbackId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Rating>()
            .Property(r => r.RateId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<RefreshToken>()
            .Property(rt => rt.RefreshTokenId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Shop>()
            .HasOne(s => s.Account)
            .WithMany(a => a.Shops)
            .HasForeignKey(s => s.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Shop)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.ShopId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductCategory)
            .WithMany(pc => pc.Products)
            .HasForeignKey(p => p.ProductCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Voucher)
            .WithMany(v => v.Products)
            .HasForeignKey(p => p.VoucherId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Cart>()
            .HasOne(c => c.Product)
            .WithMany(p => p.Carts)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Cart>()
            .HasOne(c => c.Account)
            .WithMany(a => a.Carts)
            .HasForeignKey(c => c.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Blog>()
            .HasOne(b => b.Account)
            .WithMany(a => a.Blogs)
            .HasForeignKey(b => b.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Account)
            .WithMany(a => a.Orders)
            .HasForeignKey(o => o.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Shop)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.ShopId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(od => od.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Account)
            .WithMany(a => a.Feedbacks)
            .HasForeignKey(f => f.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Shop)
            .WithMany(s => s.Feedbacks)
            .HasForeignKey(f => f.ShopId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Account)
            .WithMany(a => a.Ratings)
            .HasForeignKey(r => r.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Shop)
            .WithMany(s => s.Ratings)
            .HasForeignKey(r => r.ShopId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.Account)
            .WithMany(a => a.RefreshTokens)
            .HasForeignKey(rt => rt.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
