using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Upgrader.Auth;

public static class HeadersDataHelper
{
    public static async Task<HeadersData> GetHeadersData(this ControllerBase controller)
    {
        if (IsDevelopment())
        {
            return GetDevHeadersData();
        }

        controller.HttpContext.Items.TryGetValue("HeadersData", out object? headersDataObj);
        var headersData = headersDataObj as HeadersData;

        return headersData ?? new HeadersData();
    }

    public static async Task<HeadersData> GetHeadersData(this HubCallerContext context)
    {
        if (IsDevelopment())
        {
            return GetDevHeadersData();
        }

        context.GetHttpContext().Items.TryGetValue("HeadersData", out object? headersDataObj);
        var headersData = headersDataObj as HeadersData;

        return headersData ?? new HeadersData();
    }

    private static HeadersData GetDevHeadersData()
    {
        return new HeadersData()
        {
            UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Dev user ID
            TelegramId = 1,
            FirstName = "Dev User",
            IsPremium = false,
            LastName = string.Empty,
            PhotoUrl = string.Empty,
            TgUserName = "dev_user",
            Login = "dev_user",
        }; // only for development
    }

    private static bool IsDevelopment()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    }
}