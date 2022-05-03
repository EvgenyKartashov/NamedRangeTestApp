namespace Core.Options
{
    public class QueueConfig
    {
        public static string ConfigSection => "QueueConfig";

        public string? Host { get; set; }
        public string? VirtualHost { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}