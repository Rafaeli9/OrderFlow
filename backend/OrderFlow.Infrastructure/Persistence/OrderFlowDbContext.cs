using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Persistence;

public sealed class OrderFlowDbContext : DbContext
{
    public OrderFlowDbContext(DbContextOptions<OrderFlowDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(cfg =>
        {
            cfg.ToTable("products");
            cfg.HasKey(x => x.Id);

            cfg.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            cfg.Property(x => x.Sku)
                .HasMaxLength(50)
                .IsRequired();

            cfg.HasIndex(x => x.Sku).IsUnique();

            cfg.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            cfg.Property(x => x.Active)
                .IsRequired();
        });
    }
}
