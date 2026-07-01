using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasKey(c => c.Id);
        
        
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(t => t.Transactions)
            .WithOne(a => a.Category)
            .HasForeignKey(tt => tt.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}