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
    // if task waiting result as list of items
    public int? MinListItemsCount { get; set; }
    public int? MaxListItemsCount { get; set; }
    public TaskType Type { get; set; } = TaskType.Text;

    [Required]
    public string Title { get; set; }

    [Required]
    public string Text { get; set; }

    [NotMapped]
    public bool IsUnlocked { get; set; }

    [NotMapped]
    public bool IsCompleted { get; set; }
}

public enum TaskType
{
    Text = 10,
    TextList = 20,
}