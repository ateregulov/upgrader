using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrisAppBack.Other.Settings;
using Upgrader.Auth;
using Upgrader.Features.Transactions;

namespace Upgrader.Features.ReferralSystem;

[ApiController]
[Route("api/referrals")]
public class ReferralsController : ControllerBase
{
    private readonly MyContext _dbContext;
    private readonly AppSettings _appSettings;

    public ReferralsController(MyContext dbContext, IOptions<AppSettings> appSettingsOpt)
    {
        _dbContext = dbContext;
        _appSettings = appSettingsOpt.Value;
    }

    [HttpPost("info")]
    public async Task<IActionResult> GetOrCreateRefCode()
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.TelegramId == headersData.TelegramId);

        var refCode = await _dbContext.RefCodes.Where(x => x.IsActive).FirstOrDefaultAsync(x => x.UserId == user.Id);

        var startOfLink = $"https://t.me/{_appSettings.TelegramBotName}?start=";

        if (refCode == null)
        {
            refCode = new RefCode
            {
                UserId = user.Id,
                Code = RefCodeConverter.IntToBase60(user.TelegramId.Value),
            };

            await _dbContext.RefCodes.AddAsync(refCode);
            await _dbContext.SaveChangesAsync();

            var info = new RefInfo
            {
                Link = $"{startOfLink}{refCode.Code}",
            };

            return Ok(info);
        }

        var earned = await _dbContext
            .Transactions.Where(x =>
                x.ReceiverId == user.Id && x.Type == TransactionType.ReferrerBonus
            )
            .SumAsync(x => x.Amount);

        var referralsCount = await _dbContext
            .Referrals.Where(x => x.ParentTelegramId == user.TelegramId)
            .CountAsync();

        var resultInfo = new RefInfo
        {
            Link = $"{startOfLink}{refCode.Code}",
            ReferralsCount = referralsCount,
            RefBonusAmount = _appSettings.BonusSettings.ReferrerBonus,
            EarnedFromReferrals = earned,
        };

        return Ok(resultInfo);
    }

    public class RefInfo
    {
        public string Link { get; set; }
        public int ReferralsCount { get; set; }
        public decimal RefBonusAmount { get; set; }
        public decimal EarnedFromReferrals { get; set; }
    }
}
