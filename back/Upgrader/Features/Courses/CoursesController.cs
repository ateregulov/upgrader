using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Upgrader.Auth;

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
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TelegramId == headersData.TelegramId);

        var courses = await _dbContext
            .Courses.Select(x => new Course
            {
                Id = x.Id,
                Title = x.Title,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                Price = x.Price,
                IsBought = x.Purchases.Any(x => x.UserId == user.Id),
                TasksCount = x.Tasks.Count,
            })
            .OrderByDescending(x => x.IsBought)
            .ToListAsync();

        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourse(Guid id)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TelegramId == headersData.TelegramId);

        var courses = await _dbContext
            .Courses.Select(x => new Course
            {
                Id = x.Id,
                Title = x.Title,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                Price = x.Price,
                IsBought = x.Purchases.Any(x => x.UserId == user.Id),
                TasksCount = x.Tasks.Count,
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        return Ok(courses);
    }
}
