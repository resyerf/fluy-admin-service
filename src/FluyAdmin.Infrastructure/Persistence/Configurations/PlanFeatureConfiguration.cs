using FluyAdmin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluyAdmin.Infrastructure.Persistence.Configurations;

public class PlanFeatureConfiguration : IEntityTypeConfiguration<PlanFeature>
{
    public void Configure(EntityTypeBuilder<PlanFeature> builder)
    {
        builder.ToTable("PlanFeatures");

        builder.Property(pf => pf.Value).IsRequired().HasMaxLength(50);

        builder.HasIndex(pf => new { pf.PlanId, pf.FeatureId }).IsUnique();
    }
}
