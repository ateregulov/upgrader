using Microsoft.EntityFrameworkCore;
using Upgrader.Users;

namespace Upgrader.Features.ReferralSystem;

public class RefCodeService
{
    private readonly MyContext _dbContext;

    public RefCodeService(MyContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefCode> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        var refCode = await _dbContext
            .RefCodes.Where(x => x.IsActive)
            .FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);

        if (refCode == null)
        {
            refCode = new RefCode
            {
                UserId = user.Id,
                Code = RefCodeConverter.IntToBase60(user.TelegramId.Value),
            };

            await _dbContext.RefCodes.AddAsync(refCode, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return refCode;
    }
}
