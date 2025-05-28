using NeTelegram.Entities;

namespace NeTelegram.Middleware;

public sealed record FactoryNeMiddlewareDefinition<TContext>(
    Func<TContext, INeMiddleware<TContext>> Factory
) : INeMiddlewareDefinition<TContext> where TContext : NeEventContext;
