using Microsoft.AspNetCore.Mvc;
using Upgrader.Auth;
using static Upgrader.Features.Tasks.TaskResultService;

namespace Upgrader.Features.Tasks;

[ApiController]
[Route("api/task-results")]
public class TaskResultsController : ControllerBase
{
    private readonly TaskResultService _taskResultService;

    public TaskResultsController(TaskResultService taskResultService)
    {
        _taskResultService = taskResultService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTaskResultAsync(CreateTaskResultDto dto)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var result = await _taskResultService.CreateResultAsync(dto, headersData.UserId);

        if (!result.Succeeded)
        {
            if (result.ErrorsString.Contains("не найдено"))
            {
                return NotFound(result.ErrorsString);
            }
            else
            {
                return BadRequest(result.ErrorsString);
            }
        }

        return Ok();
    }
}
