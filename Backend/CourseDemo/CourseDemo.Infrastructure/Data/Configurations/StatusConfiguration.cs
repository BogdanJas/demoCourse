using CourseDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseDemo.Infrastructure.Data.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Statuses");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(s => s.Name)
                .IsUnique()
                .HasDatabaseName("IX_Statuses_Name");
        }
    }
}