using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<InvoiceItem> InvoiceItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.InvoiceNumber).IsUnique();
            entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.Items)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
