using Microsoft.AspNetCore.Mvc;
using Upgrader.Features.Courses;
using Upgrader.Features.Tasks;
using static Upgrader.Features.Courses.CoursePurchasesController;
using static Upgrader.Features.Tasks.TaskResultService;

namespace Upgrader.Features.PublicApi;

[ApiController]
[Route("api/public")]
[PublicApi]
public class PublicApiController : ControllerBase
{
    private readonly CoursesService _coursesService;
    private readonly TaskService _taskService;
    private readonly CoursePurchaseService _coursePurchaseService;
    private readonly TaskResultService _taskResultService;
    private readonly CourseAnalyzeResultService _courseAnalyzeResultService;
    private readonly CourseAnalyzeService _courseAnalyzeService;

    public PublicApiController(CoursesService coursesService, TaskService taskService,
        CoursePurchaseService coursePurchaseService, TaskResultService taskResultService,
        CourseAnalyzeResultService courseAnalyzeResultService, CourseAnalyzeService courseAnalyzeService)
    {
        _coursesService = coursesService;
        _taskService = taskService;
        _coursePurchaseService = coursePurchaseService;
        _taskResultService = taskResultService;
        _courseAnalyzeResultService = courseAnalyzeResultService;
        _courseAnalyzeService = courseAnalyzeService;
    }

    [HttpGet("courses")]
    public async Task<IActionResult> GetCourses(Guid userId)
    {
        var courses = await _coursesService
            .GetAsync(userId, false);

        return Ok(courses);
    }

    [HttpGet("courses/{id}")]
    public async Task<IActionResult> GetCourse(Guid userId, Guid id)
    {
        var course = await _coursesService
            .GetByIdAsync(userId, id, false);

        return Ok(course);
    }

    [HttpGet("tasks")]
    public async Task<IActionResult> GetTasks(Guid userId, Guid courseId)
    {
        var tasks = await _taskService
            .GetTasksAsync(courseId, userId, false);

        return Ok(tasks);
    }

    [HttpGet("tasks/{id}")]
    public async Task<IActionResult> GetTask(Guid userId, Guid id, bool includeResult)
    {
        var task = await _taskService
            .GetByIdAsync(id, userId, includeResult, false);

        return Ok(task);
    }

    [HttpGet("analyze-result")]
    public async Task<IActionResult> GetCourseAnalyzeResult(Guid userId, Guid courseId)
    {
        var result = await _courseAnalyzeResultService.GetAsync(courseId, userId, false);

        return Ok(result);
    }

    [HttpGet("analyze-request")]
    public async Task<IActionResult> GetCourseAnalyzeRequest(Guid userId, Guid courseId)
    {
        var result = await _courseAnalyzeService.GetAsync(courseId, userId, false);

        return Ok(result);
    }

    [HttpGet("analyze-request-price")]
    public async Task<IActionResult> GetCourseAnalyzeRequestPrice()
    {
        return Ok(_courseAnalyzeService.GetAnalyzePrice());
    }

    [HttpPost("analyze-request")]
    public async Task<IActionResult> CreateAnalyzeRequest(Guid courseId, Guid userId)
    {
        var result = await _courseAnalyzeService.CreateRequestAsync(courseId, userId, false);

        if (!result.Succeeded)
        {
            if (result.ErrorsString.Contains("не найден"))
                return NotFound(result.ErrorsString);
            return BadRequest(result.ErrorsString);
        }

        return Ok();
    }

    [HttpPost("course-purchases")]
    public async Task<IActionResult> PurchaseCourse(PurchaseCourseDto dto)
    {
        var result = await _coursePurchaseService.BuyAsync(dto.CourseId, dto.UserId, false);

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

    [HttpPost("task-result")]
    public async Task<IActionResult> CreateTaskResultAsync(CreateTaskResultDto dto)
    {
        var result = await _taskResultService.CreateResultAsync(dto, dto.UserId, false);

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
