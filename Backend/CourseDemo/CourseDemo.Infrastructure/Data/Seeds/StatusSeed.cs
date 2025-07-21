using CourseDemo.Domain.Entities;
using CourseDemo.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CourseDemo.Infrastructure.Data.Seeds
{
    public static class StatusSeed
    {
        public static void SeedStatuses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().HasData(
                new Status
                {
                    Id = (int)StatusEnums.Draft,
                    Name = nameof(StatusEnums.Draft)
                },
                new Status
                {
                    Id = (int)StatusEnums.Published,
                    Name = nameof(StatusEnums.Published)
                },
                new Status
                {
                    Id = (int)StatusEnums.Archived,
                    Name = nameof(StatusEnums.Archived)
                }
            );
        }
    }
}