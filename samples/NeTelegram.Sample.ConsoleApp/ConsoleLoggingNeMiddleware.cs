using NeTelegram.Entities;
using NeTelegram.Middleware;

namespace NeTelegram.Sample.ConsoleApp;

internal class ConsoleLoggingNeMiddleware : PassthroughNeMiddleware<NeUpdateContext>
{
    protected override Task Invoke(NeUpdateContext context)
    {
        Console.WriteLine(format: "Update received: {0}", context.Update.Id);


        return Task.CompletedTask;
    }
}
