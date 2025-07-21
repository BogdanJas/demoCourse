using CourseDemo.Domain.Entities;
using CourseDemo.Infrastructure.Data.Configurations;
using CourseDemo.Infrastructure.Data.Seeds;
using Microsoft.EntityFrameworkCore;

namespace CourseDemo.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());

            StatusSeed.SeedStatuses(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Course && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var course = (Course)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    course.CreatedAt = DateTime.UtcNow;
                }

                course.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}