using TeamsNotifier.Models;

namespace TeamsNotifier.Helpers;

public static class AdaptiveCardBuilder
{
    public static object BuildCustomCard(CustomMessage msg)
    {
        var (color, icon) = msg.Severity switch
        {
            MessageSeverity.Warning => ("Warning",   "⚠️"),
            MessageSeverity.Error   => ("Attention", "🔴"),
            _                       => ("Good",      "ℹ️")
        };

        var body = new List<object>
        {
            new { type = "TextBlock", text = $"{icon} {msg.Title}", weight = "Bolder", size = "Large", color },
            new { type = "TextBlock", text = msg.Body, wrap = true },
            new { type = "TextBlock", text = $"🕐 {DateTime.Now:yyyy-MM-dd HH:mm:ss}", isSubtle = true }
        };

        if (!string.IsNullOrWhiteSpace(msg.Url))
            return new { type = "AdaptiveCard", version = "1.4", body, actions = new[] { new { type = "Action.OpenUrl", title = "🔗 Xem chi tiết", url = msg.Url } } };

        return new { type = "AdaptiveCard", version = "1.4", body };
    }

    public static object BuildErrorCard(ErrorMessage err, string appName, string env)
    {
        var ex = err.Exception;

        var body = new List<object>
        {
            new { type = "TextBlock", text = "🔴 Exception Alert", weight = "Bolder", size = "Large", color = "Attention" },
            new { type = "FactSet", facts = new[]
                {
                    new { title = "App",         value = appName },
                    new { title = "Environment", value = env },
                    new { title = "Time",        value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                    new { title = "Exception",   value = ex.GetType().Name },
                    new { title = "Context",     value = err.Context ?? "—" },
                    new { title = "User",        value = err.UserId  ?? "—" }
                }
            },
            new { type = "TextBlock", text = "**Message:**", weight = "Bolder" },
            new { type = "TextBlock", text = ex.Message,     wrap = true, color = "Attention" }
        };

        if (err.ExtraData.Count > 0)
        {
            body.Add(new { type = "TextBlock", text = "**Extra Data:**", weight = "Bolder" });
            body.Add(new { type = "FactSet", facts = err.ExtraData.Select(kv => new { title = kv.Key, value = kv.Value }).ToArray() });
        }

        return new { type = "AdaptiveCard", version = "1.4", body };
    }
}
