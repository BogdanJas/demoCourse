using CourseDemo.Application.DTOs;
using CourseDemo.Application.Services;
using CourseDemo.Domain.Entities;
using CourseDemo.Domain.Enums;
using CourseDemo.Domain.Interfaces;
using Moq;
using Xunit;

namespace CourseDemo.Tests.Unit.Services
{
    public class CourseServiceTests
    {
        private readonly Mock<ICourseRepository> _mockRepository;
        private readonly CourseService _courseService;

        public CourseServiceTests()
        {
            _mockRepository = new Mock<ICourseRepository>();
            _courseService = new CourseService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateCourseAsync_ValidData_ReturnsCreatedCourse()
        {
            // Arrange
            var createDto = new CreateCourseDto
            {
                Title = "Test Course",
                Description = "Test Description",
                Duration = "40"
            };

            var course = new Course
            {
                Id = Guid.NewGuid(),
                Title = createDto.Title,
                Description = createDto.Description,
                Duration = createDto.Duration,
                StatusId = (int)StatusEnums.Draft,
                Status = new Status { Id = 1, Name = "Draft" }
            };

            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Course>()))
                          .ReturnsAsync(course);

            // Act
            var result = await _courseService.CreateCourseAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.Title, result.Title);
            Assert.Equal(createDto.Description, result.Description);
            Assert.Equal(createDto.Duration, result.Duration);
            Assert.Equal((int)StatusEnums.Draft, result.StatusId);
        }

        [Fact]
        public async Task UpdateCourseStatusAsync_PublishWithoutRequiredFields_ThrowsException()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var course = new Course
            {
                Id = courseId,
                Title = "",
                Description = "Test Description",
                Duration = "40",
                StatusId = (int)StatusEnums.Draft,
                Status = new Status { Id = 1, Name = "Draft" }
            };

            var updateStatusDto = new UpdateCourseStatusDto
            {
                StatusId = (int)StatusEnums.Published
            };

            _mockRepository.Setup(r => r.GetByIdAsync(courseId))
                          .ReturnsAsync(course);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _courseService.UpdateCourseStatusAsync(courseId, updateStatusDto));

            Assert.Contains("cannot be published", exception.Message);
        }

        [Fact]
        public async Task UpdateCourseAsync_ArchivedCourse_ThrowsException()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var course = new Course
            {
                Id = courseId,
                Title = "Test Course",
                Description = "Test Description",
                Duration = "40",
                StatusId = (int)StatusEnums.Archived,
                Status = new Status { Id = 3, Name = "Archived" }
            };

            var updateDto = new UpdateCourseDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                Duration = "50"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(courseId))
                          .ReturnsAsync(course);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _courseService.UpdateCourseAsync(courseId, updateDto));

            Assert.Contains("Archived courses cannot be edited", exception.Message);
        }

        [Fact]
        public async Task GetAllCoursesAsync_ReturnsAllCourses()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Course 1",
                    Description = "Description 1",
                    Duration = "30",
                    StatusId = (int)StatusEnums.Published,
                    Status = new Status { Id = 2, Name = "Published" }
                },
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Course 2",
                    Description = "Description 2",
                    Duration = "45",
                    StatusId = (int)StatusEnums.Draft,
                    Status = new Status { Id = 1, Name = "Draft" }
                }
            };

            _mockRepository.Setup(r => r.GetAllAsync())
                          .ReturnsAsync(courses);

            // Act
            var result = await _courseService.GetAllCoursesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}