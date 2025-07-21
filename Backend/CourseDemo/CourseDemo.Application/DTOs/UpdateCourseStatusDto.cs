using System.ComponentModel.DataAnnotations;
using CourseDemo.Domain.Enums;

namespace CourseDemo.Application.DTOs;

public class UpdateCourseStatusDto
{
    [Required(ErrorMessage = "StatusId is required")]
    [Range(1, 3, ErrorMessage = "StatusId must be 1 (Draft), 2 (Published), or 3 (Archived)")]
    public int StatusId { get; set; }

    public bool IsValid()
    {
        return Enum.IsDefined(typeof(StatusEnums), StatusId);
    }
} 