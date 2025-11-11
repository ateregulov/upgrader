using Microsoft.EntityFrameworkCore;
using Upgrader.Users;

namespace Upgrader.Features.Tasks;

[Index(nameof(UserId), nameof(TaskId), IsUnique = true)]
public class TaskResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid TaskId { get; set; }
    public Task Task { get; set; }
    public string Text { get; set; }
    public List<TaskResultImage> Images { get; set; } = [];
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
}
