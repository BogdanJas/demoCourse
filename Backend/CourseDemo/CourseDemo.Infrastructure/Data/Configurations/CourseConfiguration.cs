using CourseDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseDemo.Infrastructure.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Title)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(c => c.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(c => c.Duration)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(c => c.StatusId)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(c => c.PublishedAt)
                .IsRequired(false);

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(c => c.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(c => c.Status)
                .WithMany(s => s.Courses)
                .HasForeignKey(c => c.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.StatusId)
                .HasDatabaseName("IX_Courses_StatusId");

            builder.HasIndex(c => c.CreatedAt)
                .HasDatabaseName("IX_Courses_CreatedAt");
        }
    }
}