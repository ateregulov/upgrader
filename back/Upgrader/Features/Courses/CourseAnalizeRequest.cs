using Microsoft.EntityFrameworkCore;
using Upgrader.Users;

namespace Upgrader.Features.Courses;

[Index(nameof(CourseId), nameof(UserId), IsUnique = true)]
[Index(nameof(CourseId), nameof(ExternalUserId), IsUnique = true)]
public class CourseAnalyzeRequest
{
    public Guid Id { get; set; }
    public CourseAnalyzeResult Result { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public Guid? UserId { get; set; }
    public Guid? ExternalUserId { get; set; }
    public User User { get; set; }
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
}
