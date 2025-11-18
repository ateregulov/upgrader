namespace OrisAppBack.Other.Settings
{
    public class AppSettings
    {
        public string TelegramBotToken { get; set; }
        public string TelegramBotName { get; set; }
        public BonusSettings BonusSettings { get; set; }
    }

    public class BonusSettings
    {
        public decimal RegisterBonus { get; set; }
        public decimal ReferrerBonus { get; set; }
    }
}
