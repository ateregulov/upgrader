namespace Upgrader.Auth;

public class HeadersData
{
    public Guid UserId { get; set; }
    public long TelegramId { get; set; }
    public string Login { get; set; } = string.Empty;
    public string TgUserName { get; set; } = string.Empty;
    public bool IsPremium { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
}