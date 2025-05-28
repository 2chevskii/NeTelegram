using NeTelegram.Entities;

namespace NeTelegram.Middleware;

public interface INeMiddleware<in TContext> where TContext : NeEventContext
{
    Task Invoke(TContext context, Func<Task> next);
}
