using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Upgrader.Auth;

namespace Upgrader.Features.Tasks;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly MyContext _dbContext;

    public TasksController(MyContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(Guid courseId)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var course = await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
        if (course == null)
            return NotFound("Курс не найден");

        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TelegramId == headersData.TelegramId);

        var tasks = await _dbContext
            .Tasks.Where(x => x.CourseId == courseId)
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
                IsUnlocked = x.Results.Where(x => x.UserId == user.Id).Any(),
            })
            .ToListAsync();

        var maxOrderUnlocked = tasks
            .Where(x => x.IsUnlocked)
            .Select(x => x.Order)
            .DefaultIfEmpty(0)
            .Max();

        if (tasks.Count > 0 && tasks.Count > maxOrderUnlocked)
            tasks[maxOrderUnlocked].IsUnlocked = true;

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid id)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        if (task == null)
            return NotFound("Задание не найдено");

        return Ok(task);
    }
}
