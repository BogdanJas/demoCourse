using CourseDemo.Domain.Enums;

namespace CourseDemo.Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
        public int StatusId { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual Status Status { get; set; } = null!;

        public bool CanBePublished() => !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Duration);
        public bool IsArchived() => StatusId == (int)StatusEnums.Archived;
        public bool CanBeEdited() => !IsArchived();
    }
}
