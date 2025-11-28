using Microsoft.EntityFrameworkCore;
using OrisAppBack.Features.Bot;
using TLabs.DotnetHelpers;
using Upgrader.Features.PsynetApi;

namespace Upgrader.Features.Courses;

public class CourseAnalyzeResultService
{
    private readonly MyContext _dbContext;
    private readonly AppBot _appBot;
    private readonly BotClient _botClient;

    public CourseAnalyzeResultService(MyContext dbContext, AppBot appBot, BotClient botClient)
    {
        _dbContext = dbContext;
        _appBot = appBot;
        _botClient = botClient;
    }

    public async Task<CourseAnalyzeResult> GetAsync(Guid courseId, Guid userId, bool isLocal = true,
        CancellationToken cancellationToken = default)
    {
        var result = await _dbContext
            .CourseAnalyzeResults.Where(x =>
                x.Request.CourseId == courseId && (isLocal
                    ? x.Request.UserId == userId
                    : x.Request.ExternalUserId == userId)
            )
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<QueryResult> CreateAsync(CourseAnalyzeResultDto dto,
        CancellationToken cancellationToken = default)
    {
        var request = await _dbContext
            .CourseAnalyzeRequests.Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.Id == dto.RequestId, cancellationToken);
        if (request == null)
            return QueryResult.CreateFailed("заявка не найдена");

        var result = new CourseAnalyzeResult { RequestId = dto.RequestId, Message = dto.Message };

        await _dbContext.CourseAnalyzeResults.AddAsync(result, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        if (request.UserId.HasValue)
        {
            await _appBot.SendMessageAsync(
                $"Анализ ваших ответов в рамках курса: {request.Course.Title} завершен.",
                dto.TgId.Value,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            await _botClient.SendMessageAsync(
                new BotMessageDto
                {
                    Message = $"Анализ ответов в рамках курса: {request.Course.Title} завершен.",
                    UserId = request.ExternalUserId.Value
                },
                cancellationToken
            );
        }

        return QueryResult.CreateSucceeded();
    }

    public class CourseAnalyzeResultDto
    {
        public Guid RequestId { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public long? TgId { get; set; }
    }
}