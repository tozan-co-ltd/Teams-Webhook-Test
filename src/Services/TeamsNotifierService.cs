using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using TeamsNotifier.Helpers;
using TeamsNotifier.Models;

namespace TeamsNotifier.Services;

public class TeamsNotifierService : ITeamsNotifierService
{
    private readonly HttpClient _http;
    private readonly string     _webhookUrl;
    private readonly string     _appName;
    private readonly string     _environment;

    public TeamsNotifierService(HttpClient http, IConfiguration config)
    {
        _http        = http;
        _webhookUrl  = config["Teams:WebhookUrl"]
                       ?? throw new InvalidOperationException("Teams:WebhookUrl chưa được cấu hình trong appsettings.json");
        _appName     = config["App:Name"]        ?? "UnknownApp";
        _environment = config["App:Environment"] ?? "Unknown";
    }

    public async Task SendMessageAsync(CustomMessage message, CancellationToken ct = default)
    {
        var payload = Wrap(AdaptiveCardBuilder.BuildCustomCard(message));
        await PostAsync(payload, ct);
    }

    public async Task SendErrorAsync(ErrorMessage error, CancellationToken ct = default)
    {
        var payload = Wrap(AdaptiveCardBuilder.BuildErrorCard(error, _appName, _environment));
        await PostAsync(payload, ct);
    }

    private static object Wrap(object card) => new
    {
        type        = "message",
        attachments = new[] { new { contentType = "application/vnd.microsoft.card.adaptive", content = card } }
    };

    private async Task PostAsync(object payload, CancellationToken ct)
    {
        var response = await _http.PostAsJsonAsync(_webhookUrl, payload, ct);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"Teams trả về {(int)response.StatusCode}: {body}");
        }
    }
}
