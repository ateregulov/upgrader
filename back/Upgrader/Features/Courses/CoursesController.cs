using Microsoft.AspNetCore.Mvc;
using Upgrader.Auth;

namespace Upgrader.Features.Courses;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly CoursesService _coursesService;

    public CoursesController(CoursesService coursesService)
    {
        _coursesService = coursesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var courses = await _coursesService
            .GetAsync(headersData.UserId);

        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourse(Guid id)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var course = await _coursesService
            .GetByIdAsync(headersData.UserId, id);

        return Ok(course);
    }
}
