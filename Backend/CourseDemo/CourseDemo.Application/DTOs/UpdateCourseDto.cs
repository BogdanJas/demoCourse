using System.ComponentModel.DataAnnotations;

namespace CourseDemo.Application.DTOs;

public class UpdateCourseDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Duration is required")]
    [StringLength(50, ErrorMessage = "Duration must not exceed 50 characters")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Duration must be a positive number")]
    [Range(1, int.MaxValue, ErrorMessage = "Duration must be a positive number")]
    public string Duration { get; set; } = string.Empty;
}