using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AlphaProject.API.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

 
    /* 
     * 
     * proprietà inizializzate dallo scaffolding, in base allo schema del db  
     

     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            //entity.HasKey(e => e.ClientId).HasName("PK__Clients__E67E1A24F1C0CB3E");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            //entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFB8A80BE7");

            entity.Property(e => e.OrderDate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Clients");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            //entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED06818804C994");

            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull);
            //.HasConstraintName("FK_OrderItems_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull);
                //.HasConstraintName("FK_OrderItems_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            //entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CDC3DE53EE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
