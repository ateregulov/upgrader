using Microsoft.EntityFrameworkCore;

namespace Upgrader.Features.Transactions;

[Index(nameof(UniqueKey), IsUnique = true)]
public class Transaction
{
    public Guid Id { get; set; }
    public Guid? SenderId { get; set; }
    public Guid? ReceiverId { get; set; }
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string UniqueKey { get; set; }
}

public enum TransactionType
{
    RegisterBonus = 100,
    ReferrerBonus = 101,

    CoursePurchase = 200,
}
