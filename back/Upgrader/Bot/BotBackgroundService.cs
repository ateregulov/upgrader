using OrisAppBack.Features.Bot;
using Telegram.Bot;
using Telegram.Bot.Types;

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
