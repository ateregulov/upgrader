using Microsoft.AspNetCore.Mvc;
using Upgrader.Auth;

namespace Upgrader.Features.Courses;

[ApiController]
[Route("api/course-purchases")]
public class CoursePurchasesController : ControllerBase
{
    private readonly CoursePurchaseService _coursePurchaseService;

    public CoursePurchasesController(CoursePurchaseService coursePurchaseService)
    {
        _coursePurchaseService = coursePurchaseService;
    }

    [HttpPost]
    public async Task<IActionResult> PurchaseCourse(PurchaseCourseDto dto)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var result = await _coursePurchaseService.BuyAsync(dto.CourseId, headersData.UserId);

        if (!result.Succeeded)
        {
            if (result.ErrorsString.Contains("Курс не найден"))
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

    public class PurchaseCourseDto
    {
        public Guid CourseId { get; set; }
    }
}
