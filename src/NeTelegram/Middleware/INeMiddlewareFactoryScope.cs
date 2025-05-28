using NeTelegram.Entities;

namespace NeTelegram.Middleware;

public interface INeMiddlewareFactoryScope<TContext> : IDisposable
    where TContext : NeEventContext
{
    TContext Context { get; }

    INeMiddleware<TContext> Create(INeMiddlewareDefinition<TContext> neMiddlewareDefinition);
}
