namespace Upgrader.Users;

public class TgProfile
{
    public long TelegramId { get; set; }
    public string Login { get; set; }
    public string PhotoUrl { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public bool IsPremium { get; set; }
}