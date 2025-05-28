using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NeTelegram.Commands;
using NeTelegram.Commands.Extensions.Microsoft.DependencyInjection;
using NeTelegram.Extensions.Microsoft.DependencyInjection;
using NeTelegram.Extensions.Microsoft.Hosting;
using NeTelegram.Middleware;
using NeTelegram.Sample.HostedApp;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<TelegramOptions>().BindConfiguration("Telegram");

builder
    .Services
    .AddNeTelegram()
    .UseCommands(x =>
        {
            x.WithCommandsInAssemblyOf<Program>();
        }
    )
    .AddClient(services =>
        {
            var options = services.GetRequiredService<IOptions<TelegramOptions>>();
            return new TelegramBotClient(options.Value.Token);
        }
    )
    .Use<WriteUnhandledErrorToConsoleNeMiddleware>()
    .UseHostLifetime();

var app = builder.Build();

app.Run();


[Command("test")]
class TestCommand : CommandBase
{
    public override Task Invoke()
    {
        Console.WriteLine("Test command invoked with args: [{0}]", string.Join(", ", Args));
        return Task.CompletedTask;
    }
}
