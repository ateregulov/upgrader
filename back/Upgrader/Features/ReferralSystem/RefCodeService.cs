using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrisAppBack.Other.Settings;
using Upgrader.Users;

namespace Upgrader.Features.ReferralSystem;

public class RefCodeService
{
    private readonly MyContext _dbContext;
    private readonly string _botName;

    public RefCodeService(MyContext dbContext, IOptions<AppSettings> appSettingsOpt)
    {
        _dbContext = dbContext;
        _botName = appSettingsOpt.Value.TelegramBotName;
    }

    public string GetLinkByCode(string code)
    {
        return $"https://t.me/{_botName}?start={code}";
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
