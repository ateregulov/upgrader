using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;
using OrisAppBack.Other.Settings;

namespace PsyNet.Features.PsynetApi;

public abstract class BaseClient
{
    protected readonly HttpClient HttpClient;
    private readonly ILogger _logger;

    protected BaseClient(HttpClient httpClient, IOptions<AppSettings> appSettingsOptions, ILogger logger)
    {
        HttpClient = httpClient;
        var psynetSettings = appSettingsOptions.Value.IntegrationSettings.Psynet;
        HttpClient.BaseAddress = new Uri(psynetSettings.BaseUrl);
        HttpClient.DefaultRequestHeaders.Add("X-PrivateKey", psynetSettings.PrivateKey);
        _logger = logger;
    }

    public async Task<CallResponse<T>> MakeCallAsync<T>(Func<Task<HttpResponseMessage>> call,
        CancellationToken cancellationToken = default) where T : class
    {
        string apiRoute = HttpClient.BaseAddress.ToString();
        try
        {
            var response = await call();
            apiRoute = response.RequestMessage.RequestUri.ToString();
            if (response.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.Created))
            {
                var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Http request error for route {Route} with status code {StatusCode} and error message {ErrorMessage}",
                    apiRoute, response.StatusCode, errorMessage);
                return new CallResponse<T>
                {
                    Error = new ErrorData
                    {
                        StatusCode = response.StatusCode,
                        Message = errorMessage
                    }
                };
            }

            var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken);
            return new CallResponse<T> { Data = result };
        }
        catch (JsonException jEx)
        {
            _logger.LogError(jEx, "Json deserialization error for route {Route}", apiRoute);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unknown error for route {Route}", apiRoute);
        }

        return new CallResponse<T>();
    }

    public async Task<ErrorData> MakeCallAsync(Func<Task<HttpResponseMessage>> call,
        CancellationToken cancellationToken = default)
    {
        var apiRoute = HttpClient.BaseAddress.ToString();
        try
        {
            var response = await call();
            apiRoute = response.RequestMessage.RequestUri.ToString();
            if (response.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.Created))
            {
                var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Http request error for route {Route} with status code {StatusCode} and error message {ErrorMessage}",
                    apiRoute, response.StatusCode, errorMessage);

                return new ErrorData
                {
                    StatusCode = response.StatusCode,
                    Message = errorMessage
                };
            }

            return null;
        }
        catch (JsonException jEx)
        {
            _logger.LogError(jEx, "Json deserialization error for route {Route}", apiRoute);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unknown error for route {Route}", apiRoute);
        }

        return null;
    }

    protected void ExtendBaseUrl(string route)
    {
        if (!route.EndsWith('/'))
            route += '/';

        HttpClient.BaseAddress = new Uri(HttpClient.BaseAddress.ToString() + route);
    }
}

public class CallResponse<T>
{
    public T Data { get; set; }
    public ErrorData Error { get; set; }
}

public class ErrorData
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
}