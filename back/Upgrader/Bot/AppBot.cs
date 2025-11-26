using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using OrisAppBack.Other.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TLabs.DotnetHelpers;

namespace OrisAppBack.Features.Bot;

public class AppBot
{
    private readonly ITelegramBotClient _botClient;
    private static readonly Regex TokenRegex = new(@"^[0-9]{8,10}:[a-zA-Z0-9_-]{35}$");
    private readonly ILogger<AppBot> _logger;
    private readonly bool _isTokenValid;
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

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

    public async Task<QueryResult<Message>> SendMessageAsync(
        string msg,
        long tgId,
        IReplyMarkup replyMarkup = null,
        CancellationToken cancellationToken = default
    )
    {
        if (!_isTokenValid)
        {
            _logger.LogInformation("Token is not valid, skipping message: {Message}", msg);
            return QueryResult<Message>.CreateFailed("token not valid");
        }

        try
        {
            await _semaphoreSlim.WaitAsync();
            var sentMessage = await _botClient.SendTextMessageAsync(
                tgId,
                msg,
                replyMarkup: replyMarkup,
                cancellationToken: cancellationToken
            );
            await Task.Delay(TimeSpan.FromSeconds(0.1), cancellationToken);

            return QueryResult<Message>.CreateSucceeded(sentMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Error on sending bot's message:{Msg} to user with tgId:{TgId} msg:{Msg}",
                msg,
                tgId,
                ex.Message
            );
            return QueryResult<Message>.CreateFailed("error");
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}
