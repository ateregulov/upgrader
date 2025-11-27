using Microsoft.EntityFrameworkCore;
using TLabs.DotnetHelpers;

namespace Upgrader.Features.Tasks;

public class TaskResultService
{
    private readonly MyContext _dbContext;

    public TaskResultService(MyContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<QueryResult> CreateResultAsync(CreateTaskResultDto dto, Guid userId,
        bool isLocal = true, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(dto.Text) && dto.ListItems.Count != 0)
            return QueryResult.CreateFailed("Нельзя мешать ответы для конкретного типа задания");

        var task = await _dbContext
            .Tasks.Include(x => x.Course)
            .ThenInclude(x => x.Purchases.Where(x => isLocal ? x.UserId == userId : x.ExternalUserId == userId))
            .FirstOrDefaultAsync(x => x.Id == dto.TaskId, cancellationToken);

        if (task == null)
            return QueryResult.CreateFailed("Задание не найдено");
        if (task.Course.Purchases.Count == 0)
            return QueryResult.CreateFailed(
                "Нельзя отвечать на задания курса, который вы не купили"
            );

        if (task.Type == TaskType.TextList)
        {
            if (dto.ListItems.Count == 0)
                return QueryResult.CreateFailed(
                    "Нельзя не вводить список элементов для задания такого типа"
                );

            dto.ListItems = dto.ListItems.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (task.MinListItemsCount.HasValue && dto.ListItems.Count < task.MinListItemsCount)
                return QueryResult.CreateFailed(
                    $"Нельзя вводить меньше {task.MinListItemsCount} элементов для задания такого типа"
                );
            if (task.MaxListItemsCount.HasValue && dto.ListItems.Count > task.MaxListItemsCount)
                return QueryResult.CreateFailed(
                    $"Нельзя вводить больше {task.MaxListItemsCount} элементов для задания такого типа"
                );
        }

        if (task.Type == TaskType.Text && string.IsNullOrEmpty(dto.Text))
            return QueryResult.CreateFailed("Нельзя не вводить текст для задания такого типа");

        var taskResult = new TaskResult
        {
            UserId = isLocal ? userId : null,
            ExternalUserId = isLocal ? null : userId,
            TaskId = dto.TaskId,
            Text = dto.Text,
            ListItems = dto.ListItems,
        };

        await _dbContext.TaskResults.AddAsync(taskResult, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return QueryResult.CreateSucceeded();
    }

    public class CreateTaskResultDto
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
        public string Text { get; set; }
        public List<string> ListItems { get; set; } = [];
    }
}
