using TeamsNotifier.Models;

namespace TeamsNotifier.Services;

public interface ITeamsNotifierService
{
    Task SendMessageAsync(CustomMessage message, CancellationToken ct = default);
    Task SendErrorAsync(ErrorMessage error, CancellationToken ct = default);
}
