namespace Infrastructure.Logging;

public class LoggerSettings
{
    public string AppName { get; set; } = "RestApiTemplate.Api";
    public bool WriteToFile { get; set; } = false;
    public bool StructuredConsoleLogging { get; set; } = false;
    public string MinimumLogLevel { get; set; } = "Information";
}