using NeTelegram.Entities;

namespace NeTelegram.Middleware;

public interface INeMiddlewareDefinition<TContext> where TContext : NeEventContext;
