using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Upgrader.Auth;
using static Upgrader.Features.Courses.CourseAnalyzeResultService;

namespace Upgrader.Features.Courses;

[ApiController]
[Route("api/courses-analyses/results")]
public class CourseAnalysesResultsController : ControllerBase
{
    private readonly MyContext _dbContext;
    private readonly CourseAnalyzeResultService _resultService;

    public CourseAnalysesResultsController(MyContext dbContext, CourseAnalyzeResultService resultService)
    {
        _dbContext = dbContext;
        _resultService = resultService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid courseId)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var result = await _resultService.GetAsync(courseId, headersData.UserId);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateResult(CreateResultDto dto)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TelegramId == headersData.TelegramId);

        var result = await _resultService.CreateAsync(
            new CourseAnalyzeResultDto
            {
                RequestId = dto.RequestId,
                Message = dto.Message,
                UserId = user.Id,
                TgId = user.TelegramId,
            }
        );

        if (!result.Succeeded)
        {
            if (result.ErrorsString.Contains("не найдена"))
                return NotFound(result.ErrorsString);
            return BadRequest(result.ErrorsString);
        }

        return Ok();
    }

    public class CreateResultDto
    {
        public Guid RequestId { get; set; }
        public string Message { get; set; }
    }
}
