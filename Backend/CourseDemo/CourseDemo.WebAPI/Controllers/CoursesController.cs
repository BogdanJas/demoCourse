using CourseDemo.Application.DTOs;
using CourseDemo.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseDemo.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(ICourseService courseService, ILogger<CoursesController> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
    {
        _logger.LogInformation("Getting all courses");
        var courses = await _courseService.GetAllCoursesAsync();
        return Ok(courses);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDto>> GetCourseById(Guid id)
    {
        _logger.LogInformation("Getting course with ID {CourseId}", id);
        var course = await _courseService.GetCourseByIdAsync(id);

        if (course == null)
        {
            return NotFound($"Course with ID {id} not found.");
        }

        return Ok(course);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Creating new course with title {Title}", createCourseDto.Title);
        var createdCourse = await _courseService.CreateCourseAsync(createCourseDto);

        return CreatedAtAction(
            nameof(GetCourseById),
            new { id = createdCourse.Id },
            createdCourse);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDto>> UpdateCourse(Guid id, [FromBody] UpdateCourseDto updateCourseDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _logger.LogInformation("Updating course with ID {CourseId}", id);
            var updatedCourse = await _courseService.UpdateCourseAsync(id, updateCourseDto);
            return Ok(updatedCourse);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Course with ID {id} not found.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}/status")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDto>> UpdateCourseStatus(Guid id, [FromBody] UpdateCourseStatusDto updateStatusDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!updateStatusDto.IsValid())
        {
            return BadRequest("Invalid status value. Valid values are: 1 (Draft), 2 (Published), 3 (Archived)");
        }

        try
        {
            _logger.LogInformation("Updating status for course with ID {CourseId} to {Status}", id, updateStatusDto.StatusId);
            var updatedCourse = await _courseService.UpdateCourseStatusAsync(id, updateStatusDto);
            return Ok(updatedCourse);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Course with ID {id} not found.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}