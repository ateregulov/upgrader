using Microsoft.EntityFrameworkCore;
using Upgrader.Users;

namespace Upgrader.Features.Courses;

[Index(nameof(CourseId), nameof(UserId), IsUnique = true)]
public class CourseAnalyzeRequest
{
    public Guid Id { get; set; }
    public CourseAnalyzeResult Result { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}
