using FluyAdmin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluyAdmin.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Subdomain).IsRequired().HasMaxLength(63);
        builder.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(t => t.Subdomain).IsUnique();
    }
}
