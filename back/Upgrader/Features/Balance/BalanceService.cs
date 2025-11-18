using Microsoft.EntityFrameworkCore;

namespace Upgrader.Features.Balance;

public class BalanceService
{
    private readonly MyContext _dbContext;

    public BalanceService(MyContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<decimal> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var income = await _dbContext
            .Transactions.Where(x => x.ReceiverId == userId)
            .SumAsync(x => x.Amount, cancellationToken);

        var outcome = await _dbContext
            .Transactions.Where(x => x.SenderId == userId)
            .SumAsync(x => x.Amount, cancellationToken);

        return income - outcome;
    }
}
