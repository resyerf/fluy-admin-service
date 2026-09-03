using FluyAdmin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluyAdmin.Infrastructure.Persistence.Configurations;

public class UsageRecordConfiguration : IEntityTypeConfiguration<UsageRecord>
{
    public void Configure(EntityTypeBuilder<UsageRecord> builder)
    {
        builder.ToTable("UsageRecords");
        builder.Property(u => u.MetricCode).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Period).IsRequired().HasMaxLength(7);
        builder.HasIndex(u => new { u.TenantId, u.MetricCode, u.Period }).IsUnique();
    }
}
