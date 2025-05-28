using NeTelegram.Entities;

namespace NeTelegram.Middleware;

public abstract class PassthroughNeMiddleware<TContext> : INeMiddleware<TContext>
    where TContext : NeEventContext
{
    public virtual async Task Invoke(TContext context, Func<Task> next)
    {
        await Invoke(context);
        await next();
    }

    protected virtual Task Invoke(TContext context)
    {
        return Task.CompletedTask;
    }
}
