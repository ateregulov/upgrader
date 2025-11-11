using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Upgrader.Features.Courses;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly MyContext _dbContext;

    public CoursesController(MyContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _dbContext.Courses.ToListAsync();
        return Ok(courses);
    }
}
