using CourseDemo.Domain.Entities;
using CourseDemo.Domain.Enums;
using CourseDemo.Domain.Interfaces;
using CourseDemo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CourseDemo.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses
                .Include(c => c.Status)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(Guid id)
        {
            return await _context.Courses
                .Include(c => c.Status)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course> CreateAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(course.Id) ?? course;
        }

        public async Task<Course> UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(course.Id) ?? course;
        }
    }
}