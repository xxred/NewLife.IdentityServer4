namespace IdentityServer4.Admin.Api.Models
{
    public class LoggingSettings
    {
        public string LoggingProvider { get; set; } = "Console";

        public string LoggingMinimumLevel { get; set; } = "Warning";

        public string LoggingOutputTemplate { get; set; } = "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level}] {Message}{NewLine}{Exception}";
    }
}
