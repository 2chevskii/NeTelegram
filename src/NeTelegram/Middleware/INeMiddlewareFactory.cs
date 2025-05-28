using NeTelegram.Entities;

namespace NeTelegram.Middleware;

public interface INeMiddlewareFactory
{
    INeMiddlewareFactoryScope<TContext> CreateScope<TContext>(TContext context)
        where TContext : NeEventContext;
}
