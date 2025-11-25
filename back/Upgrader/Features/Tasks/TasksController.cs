using Microsoft.AspNetCore.Mvc;
using Upgrader.Auth;

namespace Upgrader.Features.Tasks;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(Guid courseId)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var tasks = await _taskService.GetTasksAsync(courseId, headersData.UserId);

        if (tasks == null)
            return NotFound("Курс не найден");

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid id, bool includeResult)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var task = await _taskService.GetByIdAsync(id, headersData.UserId, includeResult);

        if (task == null)
            return NotFound("Задание не найдено");

        return Ok(task);
    }
}
