using Microsoft.Extensions.Options;
using OrisAppBack.Other.Settings;
using PsyNet.Features.PsynetApi;

namespace Upgrader.Features.PsynetApi;

public class BotClient : BaseClient
{
    private const string BotRoute = "api/upgrader/bot";

    public BotClient(
        HttpClient httpClient,
        IOptions<AppSettings> appSettingsOptions,
        ILogger<BotClient> logger
    )
        : base(httpClient, appSettingsOptions, logger)
    {
        ExtendBaseUrl(BotRoute);
    }

    public async Task SendMessageAsync(
        BotMessageDto botMessage,
        CancellationToken cancellationToken = default
    )
    {
        await MakeCallAsync(
            () => HttpClient.PostAsJsonAsync("message", botMessage, cancellationToken),
            cancellationToken
        );
    }
}

public class BotMessageDto
{
    public string Message { get; set; }
    public Guid UserId { get; set; }
}
