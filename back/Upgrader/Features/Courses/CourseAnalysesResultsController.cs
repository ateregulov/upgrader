using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrisAppBack.Features.Bot;
using Upgrader.Auth;

namespace Upgrader.Features.Courses;

[ApiController]
[Route("api/courses-analyses/results")]
public class CourseAnalysesResultsController : ControllerBase
{
    private readonly MyContext _dbContext;
    private readonly AppBot _appBot;

    public CourseAnalysesResultsController(MyContext dbContext, AppBot appBot)
    {
        _dbContext = dbContext;
        _appBot = appBot;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid courseId)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var result = await _dbContext.CourseAnalyzeResults
            .Where(x => x.Request.CourseId == courseId && x.Request.UserId == headersData.UserId)
            .FirstOrDefaultAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateResult(CreateResultDto dto)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TelegramId == headersData.TelegramId);

        var request = await _dbContext.CourseAnalyzeRequests
            .Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.Id == dto.RequestId);
        if (request == null)
            return NotFound();

        var result = new CourseAnalyzeResult
        {
            RequestId = dto.RequestId,
            Message = dto.Message,
        };

        await _dbContext.CourseAnalyzeResults.AddAsync(result);
        await _dbContext.SaveChangesAsync();

        await _appBot.SendMessageAsync(
            $"Анализ ваших ответов в рамках курса: {request.Course.Title} завершен.",
            user.TelegramId.Value
        );

        return Ok();
    }

    public class CreateResultDto
    {
        public Guid RequestId { get; set; }
        public string Message { get; set; }
    }
}
