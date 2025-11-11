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

        var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == dto.TaskId);
        if (task == null)
            return NotFound("Задание не найдено");

        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TelegramId == headersData.TelegramId);

        var taskResult = new TaskResult
        {
            UserId = user.Id,
            TaskId = dto.TaskId,
            Text = dto.Text,
        };

        await _dbContext.TaskResults.AddAsync(taskResult);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    public class CreateTaskResultDto
    {
        public Guid TaskId { get; set; }
        public string Text { get; set; }
    }
}
