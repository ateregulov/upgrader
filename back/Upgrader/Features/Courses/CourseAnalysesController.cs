using Microsoft.AspNetCore.Mvc;
using Upgrader.Auth;

namespace Upgrader.Features.Courses;

[ApiController]
[Route("api/course-analyses")]
public class CourseAnalysesController : ControllerBase
{
    private readonly CourseAnalyzeService _analyzeService;

    public CourseAnalysesController(CourseAnalyzeService analyzeService)
    {
        _analyzeService = analyzeService;
    }

    [HttpGet("price")]
    public async Task<IActionResult> GetPrice()
    {
        return Ok(_analyzeService.GetAnalyzePrice());
    }

    [HttpGet]
    public async Task<IActionResult> GetAnalyze(Guid courseId, bool includeResult = false)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var requestResult = await _analyzeService.GetAsync(courseId, headersData.UserId, includeResult);
        if (!requestResult.Succeeded)
        {
            if (requestResult.ErrorsString.Contains("не найден"))
                return NotFound(requestResult.ErrorsString);
            return BadRequest(requestResult.ErrorsString);
        }

        return Ok(requestResult.Data.Id == Guid.Empty ? null : requestResult.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest(CreateRequestDto dto)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var result = await _analyzeService.CreateRequestAsync(new CreateAnalyzeRequestDto
        {
            CourseId = dto.CourseId,
            UserId = headersData.UserId
        });

        if (!result.Succeeded)
        {
            if (result.ErrorsString.Contains("не найден"))
                return NotFound(result.ErrorsString);
            return BadRequest(result.ErrorsString);
        }

        return Ok();
    }

    public class CreateRequestDto
    {
        public Guid CourseId { get; set; }
    }
}
