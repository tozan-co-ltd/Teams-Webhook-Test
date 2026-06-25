namespace TeamsNotifier.Web;

public record CustomMessageRequest(string Title, string Body, string Severity, string? Url);
public record ErrorRequest(string Message, string? Context, string? UserId, Dictionary<string, string>? ExtraData);
