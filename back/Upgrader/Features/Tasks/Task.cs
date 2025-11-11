using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Upgrader.Features.Courses;

namespace Upgrader.Features.Tasks;

public class Task
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public int Order { get; set; }
    public List<TaskResult> Results { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Text { get; set; }

    [NotMapped]
    public bool IsUnlocked { get; set; }
}