namespace Upgrader.Features.Transactions;

public class TransactionService
{
    private readonly MyContext _dbContext;

    public TransactionService(MyContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateTransactionAsync(
        decimal amount,
        TransactionType type,
        Guid? senderId = null,
        Guid? receiverId = null,
        string uniqueKey = null,
        CancellationToken cancellationToken = default
    )
    {
        var tx = new Transaction
        {
            Amount = amount,
            Type = type,
            SenderId = senderId,
            ReceiverId = receiverId,
            UniqueKey = uniqueKey,
        };

        await _dbContext.Transactions.AddAsync(tx, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
