using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using OrisAppBack.Other.Settings;
using Telegram.Bot;

namespace OrisAppBack.Features.Bot;

public class AppBot
{
    private readonly ITelegramBotClient _botClient;
    private static readonly Regex TokenRegex = new(@"^[0-9]{8,10}:[a-zA-Z0-9_-]{35}$");
    private readonly ILogger<AppBot> _logger;
    private readonly bool _isTokenValid;

    public AppBot(IOptions<AppSettings> appSettings, ILogger<AppBot> logger)
    {
        _logger = logger;
        var token = appSettings.Value.TelegramBotToken;

        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Telegram bot token cannot be empty");
        }

        if (!TokenRegex.IsMatch(token))
        {
            _logger.LogWarning("Invalid Telegram bot token format: {Token}", token);
            return;
        }

        _isTokenValid = true;
        _botClient = new TelegramBotClient(token);
    }

    public ITelegramBotClient GetBotClientInstance()
    {
        return _botClient;
    }

    public bool BotInValidState() => _isTokenValid;
}
