using Upgrader.Users;

namespace Upgrader.Features.ReferralSystem;

public class RefCode
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Code { get; set; }
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public bool IsActive { get; set; } = true;
}