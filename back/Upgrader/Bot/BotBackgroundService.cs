using Microsoft.EntityFrameworkCore;
using OrisAppBack.Features.Bot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Upgrader.Features.ReferralSystem;

namespace Upgrader.Bot;

public class BotBackgroundService : BackgroundService
{
    private readonly ILogger<BotBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly AppBot _appBot;

    public BotBackgroundService(
        ILogger<BotBackgroundService> logger,
        IServiceProvider serviceProvider,
        AppBot appBot)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _appBot = appBot;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("App bot background service is starting");

                if (_appBot.BotInValidState())
                {
                    await _appBot
                        .GetBotClientInstance()
                        .ReceiveAsync(
                            updateHandler: UpdateHandler,
                            pollingErrorHandler: PollingErrorHandler,
                            cancellationToken: stoppingToken
                        );
                }
                else
                {
                    _logger.LogInformation(
                        "bot is not in a valid state, listening messages are not working"
                    );
                    break;
                }

                _logger.LogInformation("App bot background service is stopping");
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Task was canceled");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Error in bot background service msg:{Msg} stack:{Stack}",
                    ex.Message,
                    ex.StackTrace
                );
                await Task.Delay(10_000, stoppingToken);
            }
        }
    }

    private async Task UpdateHandler(
        ITelegramBotClient client,
        Update update,
        CancellationToken cancellationToken
    )
    {
        if (update.Type == UpdateType.Message && update.Message.Text.StartsWith("/start"))
        {
            var parts = update.Message.Text.Split(' ');
            if (parts.Length > 1)
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<MyContext>();

                var refCode = parts[1];

                var dbRefCodeWithReferral = await dbContext
                    .RefCodes.Include(x => x.User)
                    .Where(x => x.Code == refCode)
                    .Select(x => new
                    {
                        RefUser = x.User,
                        ReferralExists = x.User.TelegramId == update.Message.Chat.Id
                            || dbContext.Referrals.Any(r =>
                                r.UserTelegramId == update.Message.Chat.Id
                                && r.ParentTelegramId == x.User.TelegramId
                            ),
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (dbRefCodeWithReferral == null || dbRefCodeWithReferral.ReferralExists)
                    return;

                var referral = new Referral
                {
                    UserTelegramId = update.Message.Chat.Id,
                    ParentTelegramId = dbRefCodeWithReferral.RefUser.TelegramId.Value,
                };

                await dbContext.Referrals.AddAsync(referral, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }

    private async Task PollingErrorHandler(
        ITelegramBotClient client,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(
            "Polling error:{Error} stack:{Stack}",
            exception.Message,
            exception.StackTrace
        );

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }
}
