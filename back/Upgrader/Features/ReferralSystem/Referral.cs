using Microsoft.EntityFrameworkCore;

namespace Upgrader.Features.ReferralSystem;

[Index(nameof(ParentId), nameof(UserId), IsUnique = true)]
public class Referral
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
}
