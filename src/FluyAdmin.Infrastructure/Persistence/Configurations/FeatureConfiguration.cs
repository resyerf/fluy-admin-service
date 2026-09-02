using FluyAdmin.Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluyAdmin.Infrastructure.Persistence.Configurations;

public class FeatureConfiguration : IEntityTypeConfiguration<Feature>
{
    public void Configure(EntityTypeBuilder<Feature> builder)
    {
        builder.ToTable("Features");

        builder.Property(f => f.Code).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Name).IsRequired().HasMaxLength(200);
        builder.Property(f => f.Type).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(f => f.Code).IsUnique();
    }
}
