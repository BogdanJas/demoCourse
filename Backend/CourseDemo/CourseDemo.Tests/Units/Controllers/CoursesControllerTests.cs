using CourseDemo.Application.DTOs;
using CourseDemo.Application.Services;
using CourseDemo.Domain.Enums;
using CourseDemo.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CourseDemo.Tests.Unit.Controllers
{
    public class CoursesControllerTests
    {
        private readonly Mock<ICourseService> _mockCourseService;
        private readonly Mock<ILogger<CoursesController>> _mockLogger;
        private readonly CoursesController _controller;

        public CoursesControllerTests()
        {
            _mockCourseService = new Mock<ICourseService>();
            _mockLogger = new Mock<ILogger<CoursesController>>();
            _controller = new CoursesController(_mockCourseService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllCourses_ReturnsOkResult()
        {
            // Arrange
            var courses = new List<CourseDto>
            {
                new CourseDto
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Course",
                    Description = "Test Description",
                    Duration = "40",
                    StatusId = (int)StatusEnums.Published,
                    StatusName = "Published"
                }
            };

            _mockCourseService.Setup(s => s.GetAllCoursesAsync())
                             .ReturnsAsync(courses);

            // Act
            var result = await _controller.GetAllCourses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCourses = Assert.IsAssignableFrom<IEnumerable<CourseDto>>(okResult.Value);
            Assert.Single(returnedCourses);
        }

        [Fact]
        public async Task GetCourseById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var course = new CourseDto
            {
                Id = courseId,
                Title = "Test Course",
                Description = "Test Description",
                Duration = "40",
                StatusId = (int)StatusEnums.Published,
                StatusName = "Published"
            };

            _mockCourseService.Setup(s => s.GetCourseByIdAsync(courseId))
                             .ReturnsAsync(course);

            // Act
            var result = await _controller.GetCourseById(courseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCourse = Assert.IsType<CourseDto>(okResult.Value);
            Assert.Equal(courseId, returnedCourse.Id);
        }

        [Fact]
        public async Task GetCourseById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            _mockCourseService.Setup(s => s.GetCourseByIdAsync(courseId))
                             .ReturnsAsync((CourseDto?)null);

            // Act
            var result = await _controller.GetCourseById(courseId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateCourse_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            var createDto = new CreateCourseDto
            {
                Title = "New Course",
                Description = "New Description",
                Duration = "50"
            };

            var createdCourse = new CourseDto
            {
                Id = Guid.NewGuid(),
                Title = createDto.Title,
                Description = createDto.Description,
                Duration = createDto.Duration,
                StatusId = (int)StatusEnums.Draft,
                StatusName = "Draft"
            };

            _mockCourseService.Setup(s => s.CreateCourseAsync(createDto))
                             .ReturnsAsync(createdCourse);

            // Act
            var result = await _controller.CreateCourse(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCourse = Assert.IsType<CourseDto>(createdResult.Value);
            Assert.Equal(createDto.Title, returnedCourse.Title);
        }

        [Fact]
        public async Task UpdateCourseStatus_InvalidStatus_ReturnsBadRequest()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var updateStatusDto = new UpdateCourseStatusDto
            {
                StatusId = 999 // Invalid status
            };

            // Act
            var result = await _controller.UpdateCourseStatus(courseId, updateStatusDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}