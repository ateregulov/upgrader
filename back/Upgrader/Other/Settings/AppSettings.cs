namespace OrisAppBack.Other.Settings
{
    public class AppSettings
    {
        public string TelegramBotToken { get; set; }
        public string TelegramBotName { get; set; }
        public BonusSettings BonusSettings { get; set; }
        public SecuritySettings SecuritySettings { get; set; }
        public CourseSettings CourseSettings { get; set; }
        public IntegrationsSettings IntegrationSettings { get; set; }
    }

    public class BonusSettings
    {
        public decimal RegisterBonus { get; set; }
        public decimal ReferrerBonus { get; set; }
    }

    public class SecuritySettings
    {
        public string PrivateKey { get; set; }
    }

    public class CourseSettings
    {
        public decimal AnalysisPrice { get; set; }
    }

    public class IntegrationsSettings
    {
        public PsynetSettings Psynet { get; set; }
    }

    public class PsynetSettings
    {
        public string BaseUrl { get; set; }
        public string PrivateKey { get; set; }
    }
}
