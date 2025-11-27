using Microsoft.EntityFrameworkCore;
using TLabs.DotnetHelpers;
using Upgrader.Features.Balance;
using Upgrader.Features.Transactions;

namespace Upgrader.Features.Courses;

public class CoursePurchaseService
{
    private readonly MyContext _dbContext;
    private readonly BalanceService _balanceService;
    private readonly TransactionService _transactionService;

    public CoursePurchaseService(
        MyContext dbContext,
        BalanceService balanceService,
        TransactionService transactionService
    )
    {
        _dbContext = dbContext;
        _balanceService = balanceService;
        _transactionService = transactionService;
    }

    public async Task<QueryResult> BuyAsync(Guid courseId, Guid userId, CancellationToken cancellationToken = default)
    {
        var course = await _dbContext
            .Courses.Where(x => x.Id == courseId)
            .FirstOrDefaultAsync(cancellationToken);
        if (course == null)
            return QueryResult.CreateFailed("Курс не найден");

        var isCourseAlreadyBought = await _dbContext.CoursePurchases.AnyAsync(x =>
            x.UserId == userId && x.CourseId == courseId,
            cancellationToken
        );
        if (isCourseAlreadyBought)
            return QueryResult.CreateFailed("Курс уже был куплен");

        var balance = await _balanceService.GetBalanceAsync(userId, cancellationToken);
        if (course.Price > balance)
            return QueryResult.CreateFailed("Недостаточно средств на счету");

        await _transactionService.CreateTransactionAsync(
            amount: course.Price,
            type: TransactionType.CoursePurchase,
            senderId: userId,
            uniqueKey: $"course-purchase-{userId}-{courseId}",
            cancellationToken: cancellationToken
        );

        await _dbContext.CoursePurchases.AddAsync(
            new CoursePurchase
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CourseId = courseId,
                PaidAmount = course.Price,
            }, cancellationToken
        );
        await _dbContext.SaveChangesAsync(cancellationToken);

        return QueryResult.CreateSucceeded();
    }
}
