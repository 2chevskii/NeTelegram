using NeTelegram.Commands;
using NeTelegram.Middleware;
using NeTelegram.Pipelines;
using NeTelegram.Sample.ConsoleApp;
using Telegram.Bot;

var builder = new NeTelegramPipelineBuilder();

builder.UseCommands(x => x.WithCommandsInCurrentAppDomain());
builder.Use<ConsoleLoggingNeMiddleware>();
builder.Use<WriteUnhandledErrorToConsoleNeMiddleware>();

var client = new TelegramBotClient("<insert your bot token here>");
builder.AddClient(client);

var pipeline = builder.Build();
pipeline.Start();
Console.WriteLine("Pipeline started");

while (true)
{
    Thread.Sleep(TimeSpan.FromMilliseconds(300));
}
