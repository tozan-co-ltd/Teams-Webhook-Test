using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamsNotifier.Extensions;
using TeamsNotifier.Models;
using TeamsNotifier.Services;

// ── Config ────────────────────────────────────────────────────────────────────
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

// ── DI ────────────────────────────────────────────────────────────────────────
var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(config);
services.AddTeamsNotifier();

var provider = services.BuildServiceProvider();
var teams = provider.GetRequiredService<ITeamsNotifierService>();

// ── Tự động gửi message khi khởi động ────────────────────────────────────────
Console.WriteLine("Sending...");

try
{
    // 2. Giả lập exception và gửi error alert
    // try { 
        // ThrowSampleError(); 

        // 1. Gửi custom message
        await teams.SendMessageAsync(new CustomMessage
        {
            Title    = "アプリが正常に起動しました 🚀",
            Body     = $"TeamNotificationAppが{DateTime.Now:yyyy-MM-dd HH:mm:ss}に起動しました",
            Severity = MessageSeverity.Info
        });
        Console.WriteLine("✅ アプリが正常に起動しました!");
        // ThrowSampleError();
    // }
    // catch (Exception ex)
    // {

    //     Console.WriteLine("✅ エラーアラートを送信しました。");
    // }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
    Console.WriteLine("👉 appsettings.json の WebhookUrl を再確認してください");
    await teams.SendErrorAsync(new ErrorMessage
        {
            Exception = ex,
            Context   = "Program.ThrowSampleError",
            UserId    = "son-01",
            // ExtraData = new() { { "Environment", "Development" }, { "Version", "1.0.0" } }
        });
}

static void ThrowSampleError() => throw new InvalidOperationException("3回試行しましたが、データベースに接続できませんでした。");
