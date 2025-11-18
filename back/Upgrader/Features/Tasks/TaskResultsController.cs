using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Upgrader.Auth;

namespace Upgrader.Features.Tasks;

[ApiController]
[Route("api/task-results")]
public class TaskResultsController : ControllerBase
{
    private readonly MyContext _dbContext;

    public TaskResultsController(MyContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTaskResultAsync(CreateTaskResultDto dto)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        if (!string.IsNullOrEmpty(dto.Text) && dto.ListItems.Count != 0)
            return BadRequest("Нельзя мешать ответы для конкретного типа задания");

        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TelegramId == headersData.TelegramId);

        var task = await _dbContext
            .Tasks.Include(x => x.Course)
            .ThenInclude(x => x.Purchases.Where(x => x.UserId == user.Id))
            .FirstOrDefaultAsync(x => x.Id == dto.TaskId);

        if (task == null)
            return NotFound("Задание не найдено");
        if (task.Course.Purchases.Count == 0)
            return BadRequest("Нельзя отвечать на задания курса, который вы не купили");

        if (task.Type == TaskType.TextList)
        {
            if (dto.ListItems.Count == 0)
                return BadRequest("Нельзя не вводить список элементов для задания такого типа");

            dto.ListItems = dto.ListItems.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (task.MinListItemsCount.HasValue && dto.ListItems.Count < task.MinListItemsCount)
                return BadRequest($"Нельзя вводить меньше {task.MinListItemsCount} элементов для задания такого типа");
            if (task.MaxListItemsCount.HasValue && dto.ListItems.Count > task.MaxListItemsCount)
                return BadRequest($"Нельзя вводить больше {task.MaxListItemsCount} элементов для задания такого типа");
        }

        if (task.Type == TaskType.Text && string.IsNullOrEmpty(dto.Text))
            return BadRequest("Нельзя не вводить текст для задания такого типа");


        var taskResult = new TaskResult
        {
            UserId = user.Id,
            TaskId = dto.TaskId,
            Text = dto.Text,
            ListItems = dto.ListItems,
        };

        await _dbContext.TaskResults.AddAsync(taskResult);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    public class CreateTaskResultDto
    {
        public Guid TaskId { get; set; }
        public string Text { get; set; }
        public List<string> ListItems { get; set; } = [];
    }
}
