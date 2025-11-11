using System.ComponentModel.DataAnnotations;

namespace Upgrader.Features.Tasks;

public class TaskResultImage
{
    public Guid Id { get; set; }
    public Guid TaskResultId { get; set; }
    public TaskResult TaskResult { get; set; }

    [Required]
    public string Extension { get; set; }

    [Required]
    public byte[] Data { get; set; }
}
