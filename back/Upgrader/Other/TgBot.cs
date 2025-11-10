using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Upgrader.Collocutors;

public class TgBot : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly bool _isEnabled;

    public TgBot(IServiceScopeFactory scopeFactory,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _isEnabled = _configuration.GetSection("CollocutorBot:Enabled").Get<bool>();

        if (_isEnabled)
        {
            var token = _configuration.GetSection("CollocutorBot:TelegramToken").Value;
            _botClient = new TelegramBotClient(token!);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
            _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);
        }
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();

            Console.WriteLine(JsonConvert.SerializeObject(update));
            if (update.Type == UpdateType.Message)
            {
            }
            if (update.Type == UpdateType.CallbackQuery && update?.CallbackQuery != null)
            {
                return;
            }
        }
        catch
        {
        }
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(JsonConvert.SerializeObject(exception));
        await Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}