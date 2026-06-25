namespace TeamsNotifier.Models;

public class CustomMessage
{
    public required string Title    { get; set; }
    public required string Body     { get; set; }
    public string?         Url      { get; set; }
    public MessageSeverity Severity { get; set; } = MessageSeverity.Info;
}

public class ErrorMessage
{
    public required Exception              Exception { get; set; }
    public string?                         Context   { get; set; }
    public string?                         UserId    { get; set; }
    public Dictionary<string, string>      ExtraData { get; set; } = [];
}

public enum MessageSeverity { Info, Warning, Error }
