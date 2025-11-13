using Microsoft.EntityFrameworkCore;

namespace Upgrader.Features.ReferralSystem;

[Index(nameof(UserTelegramId), IsUnique = true)]
public class Referral
{
    public Guid Id { get; set; }
    public long ParentTelegramId { get; set; }
    public long UserTelegramId { get; set; }
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
}
