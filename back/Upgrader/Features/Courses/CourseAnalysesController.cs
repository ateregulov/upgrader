using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrisAppBack.Other.Settings;
using Upgrader.Auth;
using Upgrader.Features.Balance;
using Upgrader.Features.Tasks;
using Upgrader.Features.Transactions;

namespace Upgrader.Features.Courses;

[ApiController]
[Route("api/course-analyses")]
public class CourseAnalysesController : ControllerBase
{
    private readonly MyContext _dbContext;
    private readonly TaskService _taskService;
    private readonly BalanceService _balanceService;
    private readonly TransactionService _transactionService;
    private readonly decimal _analyzePrice;

    public CourseAnalysesController(MyContext dbContext, BalanceService balanceService,
        IOptions<AppSettings> appSettingsOpt, TransactionService transactionService, TaskService taskService)
    {
        _dbContext = dbContext;
        _balanceService = balanceService;
        _analyzePrice = appSettingsOpt.Value.CourseSettings.AnalysisPrice;
        _transactionService = transactionService;
        _taskService = taskService;
    }

    [HttpGet("price")]
    public async Task<IActionResult> GetPrice()
    {
        return Ok(_analyzePrice);
    }

    [HttpGet]
    public async Task<IActionResult> GetAnalyze(Guid courseId, bool includeResult = false)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var isCourseExists = await _dbContext.Courses.AnyAsync(x => x.Id == courseId);
        if (!isCourseExists)
        {
            return NotFound("Курс не найден");
        }

        IQueryable<CourseAnalyzeRequest> requestQuery = _dbContext.CourseAnalyzeRequests;
        if (includeResult)
            requestQuery = requestQuery.Include(x => x.Result);

        var request = await requestQuery
            .FirstOrDefaultAsync(x => x.CourseId == courseId && x.UserId == headersData.UserId);

        return Ok(request);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest(CreateRequestDto dto)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var isCourseExists = await _dbContext.Courses.AnyAsync(x => x.Id == dto.CourseId);
        if (!isCourseExists)
        {
            return NotFound("Курс не найден");
        }

        var tasks = await _taskService.GetTasksAsync(dto.CourseId, headersData.UserId);
        if (!tasks.All(x => x.IsCompleted))
        {
            return BadRequest("Нельзя создать анализ курса не ответив на все задания");
        }

        var balance = await _balanceService.GetBalanceAsync(headersData.UserId);
        if (balance < _analyzePrice)
        {
            return BadRequest("INFLUENT_BALANCE");
        }

        await _transactionService.CreateTransactionAsync(
            _analyzePrice,
            TransactionType.CourseAnalyze,
            senderId: headersData.UserId,
            uniqueKey: $"CourseAnalyze-{dto.CourseId}-{headersData.UserId}"
        );

        var courseAnalyzeRequest = new CourseAnalyzeRequest
        {
            CourseId = dto.CourseId,
            UserId = headersData.UserId,
        };
        await _dbContext.CourseAnalyzeRequests.AddAsync(courseAnalyzeRequest);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    public class CreateRequestDto
    {
        public Guid CourseId { get; set; }
    }
}
