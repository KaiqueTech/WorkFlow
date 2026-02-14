using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkFlow.Domain.Models;

namespace WorkFlow.Infra.Persistence.Write.Context.Configurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<RequestModel>
    {
        public void Configure(EntityTypeBuilder<RequestModel> builder)
        {
            builder.ToTable("Requests");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(r => r.Category)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(r => r.Priority)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(r => r.CreatedByUserId)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.UpdatedAt)
                .IsRequired(false);

            builder.HasMany(r => r.History)
                .WithOne()
                .HasForeignKey(h => h.RequestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
