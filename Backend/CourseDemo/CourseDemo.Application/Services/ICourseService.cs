using CourseDemo.Application.DTOs;

namespace CourseDemo.Application.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(Guid id);
        Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto);
        Task<CourseDto> UpdateCourseAsync(Guid id, UpdateCourseDto updateCourseDto);
        Task<CourseDto> UpdateCourseStatusAsync(Guid id, UpdateCourseStatusDto updateStatusDto);
    }
}