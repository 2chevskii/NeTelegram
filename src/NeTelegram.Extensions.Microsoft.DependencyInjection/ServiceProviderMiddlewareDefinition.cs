using NeTelegram.Entities;
using NeTelegram.Middleware;

namespace NeTelegram.Extensions.Microsoft.DependencyInjection;

public record ServiceProviderMiddlewareDefinition<TContext>(Type Type)
    : INeMiddlewareDefinition<TContext> where TContext : NeEventContext;