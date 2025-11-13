using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OrisAppBack.Other.Settings;
using Upgrader.Db;
using Upgrader.Features.Transactions;
using Upgrader.Users;

namespace Upgrader.Auth;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public AuthMiddleware(RequestDelegate next,
        ILogger<AuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Method == HttpMethod.Options.Method ||
            context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/api/healthcheck"))
        {
            await _next(context);
            return;
        }

        string authHeader = "";

        if (!string.IsNullOrWhiteSpace(context.Request.Query["token"]))
        {
            authHeader = context.Request.Query["token"];
        }
        else if (context.Request.Headers.ContainsKey("Authorization"))
        {
            authHeader = context.Request.Headers["Authorization"].ToString();
        }
        else
        {
            await _next(context);
            return;
        }

        if (authHeader.StartsWith("tma ", StringComparison.OrdinalIgnoreCase))
        {
            var settings = context.RequestServices.GetService<IOptions<AppSettings>>()!.Value;

            var botToken = settings.TelegramBotToken;

            if (string.IsNullOrEmpty(botToken))
            {
                _logger.LogError("TelegramBotToken config error");
                context.Response.StatusCode = 500;
                return;
            }

            var initData = authHeader.Substring(4);
            var validator = new TgValidator();

            if (validator.ValidateInitData(botToken, initData))
            {
                var data = validator.ParseInitData(initData);

                if (!data.TryGetValue("auth_date", out var authDateStr) ||
                    !long.TryParse(authDateStr, out var authDate))
                {
                    context.Response.StatusCode = 401;
                    return;
                }

                var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (currentTimestamp - authDate > 2 * 60 * 60) // 2 hours
                {
                    context.Response.StatusCode = 401;
                    return;
                }

                data.TryGetValue("user", out var userData);
                var obj = JObject.Parse(userData);

                var telegramId = obj["id"]?.Value<long>() ?? 0;
                var isPremium = obj["is_premium"]?.Value<bool>() ?? false;
                var firstName = obj["first_name"]?.Value<string>() ?? string.Empty;
                var lastName = obj["last_name"]?.Value<string>() ?? string.Empty;
                var photoUrl = obj["photo_url"]?.Value<string>() ?? string.Empty;
                var login = obj["username"]?.Value<string>() ?? string.Empty;

                var headersData = new HeadersData
                {
                    TelegramId = telegramId,
                    TgUserName = login,
                    FirstName = firstName,
                    IsPremium = isPremium,
                    LastName = lastName,
                    PhotoUrl = photoUrl,
                    Login = login,
                };

                var _dbContext = context.RequestServices.GetService<MyContext>();
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.TelegramId == telegramId);

                if (user == null)
                {
                    var transactionService = context.RequestServices.GetService<TransactionService>();

                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        TelegramId = telegramId,
                        Created = DateTimeOffset.UtcNow,
                    };

                    var tgProfile = new TgProfile
                    {
                        TelegramId = telegramId,
                        Login = login,
                        FirstName = firstName,
                        LastName = lastName,
                        IsPremium = isPremium,
                        PhotoUrl = photoUrl
                    };

                    _dbContext.Users.Add(user);
                    _dbContext.TgProfiles.Add(tgProfile);
                    await _dbContext.SaveChangesAsync();
                    await transactionService.CreateTransactionAsync(
                        settings.BonusSettings.RegisterBonus,
                        TransactionType.RegisterBonus,
                        receiverId: user.Id,
                        uniqueKey: $"registerBonus-{user.Id}"
                    );

                    var refData = await _dbContext
                        .Referrals.Where(r => r.UserTelegramId == telegramId)
                        .Select(r => new
                        {
                            ReferrerUserId = _dbContext
                                .Users.Where(u => u.TelegramId == r.ParentTelegramId)
                                .Select(u => u.Id)
                                .FirstOrDefault(),
                        })
                        .FirstOrDefaultAsync();

                    if (refData?.ReferrerUserId != null)
                    {
                        await transactionService.CreateTransactionAsync(
                            settings.BonusSettings.ReferrerBonus,
                            TransactionType.ReferrerBonus,
                            receiverId: refData.ReferrerUserId,
                            uniqueKey: $"referrerBonus-from-{user.Id}"
                        );
                    }
                }

                headersData.UserId = user.Id;

                context.Items["HeadersData"] = headersData;
            }
            else
            {
                _logger.LogWarning("Token validation error");
                context.Response.StatusCode = 401;
                return;
            }
        }

        await _next(context);
    }
}