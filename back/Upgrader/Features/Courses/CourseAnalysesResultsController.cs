using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Upgrader.Auth;

namespace Upgrader.Features.Courses;

[ApiController]
[Route("api/courses/{courseId}/analyses/results")]
public class CourseAnalysesResultsController : ControllerBase
{
    private readonly MyContext _dbContext;

    public CourseAnalysesResultsController(MyContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var courseId = Guid.Parse(RouteData.Values["courseId"].ToString());

        var result = await _dbContext.CourseAnalyzeResults
            .Where(x => x.Request.CourseId == courseId && x.Request.UserId == headersData.UserId)
            .FirstOrDefaultAsync();

        return Ok(result);
    }
}
