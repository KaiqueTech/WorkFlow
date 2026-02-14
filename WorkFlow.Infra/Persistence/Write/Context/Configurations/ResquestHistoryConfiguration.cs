using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WorkFlow.Domain.Models;

namespace WorkFlow.Infra.Persistence.Write.Context.Configurations
{
    public class ResquestHistoryConfiguration : IEntityTypeConfiguration<RequestHistoryModel>
    {
        public void Configure(EntityTypeBuilder<RequestHistoryModel> builder)
        {
            builder.ToTable("RequestHistories");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.RequestId)
                .IsRequired();

            builder.Property(h => h.FromStatus)
                .HasConversion<int?>();

            builder.Property(h => h.ToStatus)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(h => h.ChangedBy)
                .IsRequired();

            builder.Property(h => h.ChangedAt)
                .IsRequired();

            builder.Property(h => h.Comment)
                .HasMaxLength(1000)
                .IsRequired(false);
        }
    }
}
