using CourseDemo.Domain.Entities;

namespace CourseDemo.Domain.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(Guid id);
        Task<Course> CreateAsync(Course course);
        Task<Course> UpdateAsync(Course course);
    }
}