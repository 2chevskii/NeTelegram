using NeTelegram.Entities;

namespace NeTelegram.Middleware;

public record TypeNeMiddlewareDefinition<TContext>(Type Type) : INeMiddlewareDefinition<TContext>
    where TContext : NeEventContext;
