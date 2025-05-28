using NeTelegram.Entities;

namespace NeTelegram.Middleware;

public sealed class WriteUnhandledErrorToConsoleNeMiddleware : INeMiddleware<NeErrorContext>
{
    public Task Invoke(NeErrorContext context, Func<Task> next)
    {
        Console.WriteLine(
            format: "Unhandled error encountered: {0},\n{1}",
            context.Source,
            context.Exception);
        return Task.CompletedTask;
    }
}
