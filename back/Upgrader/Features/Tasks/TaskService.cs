using Microsoft.EntityFrameworkCore;

namespace Upgrader.Features.Tasks;

public class TaskService
{
    private readonly MyContext _dbContext;

    public TaskService(MyContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Task>> GetTasksAsync(
        Guid courseId,
        Guid userId,
        bool isLocal = true,
        CancellationToken cancellationToken = default
    )
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(
            x => x.Id == courseId,
            cancellationToken
        );
        if (course == null)
            return null;

        var tasksResultIds = await _dbContext.TaskResults
            .Where(x => isLocal ? x.UserId == userId : x.ExternalUserId == userId && x.Task.CourseId == courseId)
            .Select(x => x.TaskId)
            .ToHashSetAsync(cancellationToken);

        var tasks = await _dbContext
            .Tasks.Where(x => x.CourseId == courseId)
            .OrderBy(x => x.Order)
            .Select(x => new Task
            {
                Id = x.Id,
                CourseId = x.CourseId,
                Order = x.Order,
                Title = x.Title,
                Text = x.Text,
                Type = x.Type,
                MaxListItemsCount = x.MaxListItemsCount,
                MinListItemsCount = x.MinListItemsCount,
                IsUnlocked = tasksResultIds.Contains(x.Id),
                IsCompleted = tasksResultIds.Contains(x.Id),
            })
            .ToListAsync(cancellationToken);

        var maxOrderUnlocked = tasks
            .Where(x => x.IsUnlocked)
            .Select(x => x.Order)
            .DefaultIfEmpty(0)
            .Max();

        if (tasks.Count > 0 && tasks.Count > maxOrderUnlocked)
            tasks[maxOrderUnlocked].IsUnlocked = true;

        return tasks;
    }

    public async Task<Task> GetByIdAsync(
        Guid taskId,
        Guid userId,
        bool includeResults,
        bool isLocal = true,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Task> taskQuery = _dbContext.Tasks;
        if (includeResults)
        {
            taskQuery = taskQuery.Include(x => x.Results.Where(x => isLocal ? x.UserId == userId : x.ExternalUserId == userId));
        }

        var task = await taskQuery.FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken);
        return task;
    }
}
