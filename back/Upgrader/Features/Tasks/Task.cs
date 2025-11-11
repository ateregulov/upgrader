using System.ComponentModel.DataAnnotations;
using Upgrader.Features.Courses;

namespace Upgrader.Features.Tasks;

public class Task
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public int Order { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Text { get; set; }
}