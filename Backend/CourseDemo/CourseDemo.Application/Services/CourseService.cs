using CourseDemo.Application.DTOs;
using CourseDemo.Domain.Entities;
using CourseDemo.Domain.Enums;
using CourseDemo.Domain.Interfaces;

namespace CourseDemo.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.Select(MapToDto);
        }

        public async Task<CourseDto?> GetCourseByIdAsync(Guid id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            return course != null ? MapToDto(course) : null;
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
        {
            var course = new Course
            {
                Id = Guid.NewGuid(),
                Title = createCourseDto.Title.Trim(),
                Description = createCourseDto.Description.Trim(),
                Duration = createCourseDto.Duration.Trim(),
                StatusId = (int)StatusEnums.Draft,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdCourse = await _courseRepository.CreateAsync(course);
            return MapToDto(createdCourse);
        }

        public async Task<CourseDto> UpdateCourseAsync(Guid id, UpdateCourseDto updateCourseDto)
        {
            var existingCourse = await _courseRepository.GetByIdAsync(id);
            if (existingCourse == null)
            {
                throw new KeyNotFoundException($"Course with ID {id} not found.");
            }

            if (!existingCourse.CanBeEdited())
            {
                throw new InvalidOperationException("Archived courses cannot be edited.");
            }

            existingCourse.Title = updateCourseDto.Title.Trim();
            existingCourse.Description = updateCourseDto.Description.Trim();
            existingCourse.Duration = updateCourseDto.Duration.Trim();
            existingCourse.UpdatedAt = DateTime.UtcNow;

            var updatedCourse = await _courseRepository.UpdateAsync(existingCourse);
            return MapToDto(updatedCourse);
        }

        public async Task<CourseDto> UpdateCourseStatusAsync(Guid id, UpdateCourseStatusDto updateStatusDto)
        {
            var existingCourse = await _courseRepository.GetByIdAsync(id);
            if (existingCourse == null)
            {
                throw new KeyNotFoundException($"Course with ID {id} not found.");
            }

            var newStatus = (StatusEnums)updateStatusDto.StatusId;

            if (newStatus == StatusEnums.Published && !existingCourse.CanBePublished())
            {
                throw new InvalidOperationException("Course cannot be published. Title and duration are required.");
            }

            if (existingCourse.IsArchived())
            {
                throw new InvalidOperationException("Archived courses cannot be modified.");
            }

            existingCourse.StatusId = updateStatusDto.StatusId;
            existingCourse.UpdatedAt = DateTime.UtcNow;

            if (newStatus == StatusEnums.Published && existingCourse.PublishedAt == null)
            {
                existingCourse.PublishedAt = DateTime.UtcNow;
            }

            var updatedCourse = await _courseRepository.UpdateAsync(existingCourse);
            return MapToDto(updatedCourse);
        }

        private static CourseDto MapToDto(Course course)
        {
            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title ?? string.Empty,
                Description = course.Description ?? string.Empty,
                Duration = course.Duration ?? string.Empty,
                StatusId = course.StatusId,
                StatusName = ((StatusEnums)course.StatusId).ToString(),
                PublishedAt = course.PublishedAt,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            };
        }
    }
}