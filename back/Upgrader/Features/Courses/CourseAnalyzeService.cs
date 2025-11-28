using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrisAppBack.Other.Settings;
using TLabs.DotnetHelpers;
using Upgrader.Features.Balance;
using Upgrader.Features.Tasks;
using Upgrader.Features.Transactions;

namespace Upgrader.Features.Courses;

public class CourseAnalyzeService
{
    private readonly MyContext _dbContext;
    private readonly TaskService _taskService;
    private readonly BalanceService _balanceService;
    private readonly TransactionService _transactionService;
    private readonly decimal _analyzePrice;

    public CourseAnalyzeService(
        MyContext dbContext,
        TaskService taskService,
        BalanceService balanceService,
        IOptions<AppSettings> appSettingsOpt,
        TransactionService transactionService
    )
    {
        _dbContext = dbContext;
        _taskService = taskService;
        _balanceService = balanceService;
        _analyzePrice = appSettingsOpt.Value.CourseSettings.AnalysisPrice;
        _transactionService = transactionService;
    }

    public decimal GetAnalyzePrice() => _analyzePrice;

    public async Task<QueryResult<CourseAnalyzeRequest>> GetAsync(
        Guid courseId,
        Guid userId,
        bool includeResult = false,
        bool isLocal = true,
        CancellationToken cancellationToken = default
    )
    {
        var isCourseExists = await _dbContext.Courses.AnyAsync(x => x.Id == courseId, cancellationToken);
        if (!isCourseExists)
            return QueryResult<CourseAnalyzeRequest>.CreateFailed("Курс не найден");

        IQueryable<CourseAnalyzeRequest> requestQuery = _dbContext.CourseAnalyzeRequests;
        if (includeResult)
            requestQuery = requestQuery.Include(x => x.Result);

        var request = await requestQuery.FirstOrDefaultAsync(
            x => x.CourseId == courseId && (isLocal ? x.UserId == userId : x.ExternalUserId == userId),
            cancellationToken
        );

        return QueryResult<CourseAnalyzeRequest>.CreateSucceeded(request ?? new CourseAnalyzeRequest());
    }

    public async Task<QueryResult> CreateRequestAsync(
        CreateAnalyzeRequestDto dto,
        bool isLocal = true,
        CancellationToken cancellationToken = default
    )
    {
        var isCourseExists = await _dbContext.Courses.AnyAsync(
            x => x.Id == dto.CourseId,
            cancellationToken
        );
        if (!isCourseExists)
            return QueryResult.CreateFailed("Курс не найден");

        var tasks = await _taskService.GetTasksAsync(
            dto.CourseId,
            dto.UserId,
            isLocal: isLocal,
            cancellationToken: cancellationToken
        );
        if (!tasks.All(x => x.IsCompleted))
            return QueryResult.CreateFailed(
                "Нельзя создать анализ курса не ответив на все задания"
            );

        if (isLocal)
        {
            var balance = await _balanceService.GetBalanceAsync(
                dto.UserId,
                cancellationToken: cancellationToken
            );
            if (balance < _analyzePrice)
            {
                return QueryResult.CreateFailed("INFLUENT_BALANCE");
            }

            await _transactionService.CreateTransactionAsync(
                _analyzePrice,
                TransactionType.CourseAnalyze,
                senderId: dto.UserId,
                uniqueKey: $"CourseAnalyze-{dto.CourseId}-{dto.UserId}",
                cancellationToken: cancellationToken
            );
        }

        var courseAnalyzeRequest = new CourseAnalyzeRequest
        {
            CourseId = dto.CourseId,
            UserId = isLocal ? dto.UserId : null,
            ExternalUserId = isLocal ? null : dto.UserId,
        };
        await _dbContext.CourseAnalyzeRequests.AddAsync(courseAnalyzeRequest, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return QueryResult.CreateSucceeded();
    }

}

public class CreateAnalyzeRequestDto
{
    public Guid CourseId { get; set; }
    public Guid UserId { get; set; }
}
