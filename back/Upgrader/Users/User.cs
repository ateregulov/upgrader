namespace Upgrader.Users;

public class User
{
    public Guid Id { get; set; }
    public long? TelegramId { get; set; }
    public DateTimeOffset Created { get; set; }
    public TgProfile TgProfile { get; set; }
}