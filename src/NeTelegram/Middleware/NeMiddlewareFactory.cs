using NeTelegram.Entities;

namespace NeTelegram.Middleware;

public sealed class NeMiddlewareFactory : INeMiddlewareFactory
{
    public INeMiddlewareFactoryScope<TContext> CreateScope<TContext>(TContext context)
        where TContext : NeEventContext
    {
        return new Scope<TContext>(this, context);
    }

    private INeMiddleware<TContext> Create<TContext>(
        INeMiddlewareDefinition<TContext> neMiddlewareDefinition,
        TContext context
    ) where TContext : NeEventContext
    {
        return neMiddlewareDefinition switch
        {
            TypeNeMiddlewareDefinition<TContext> typeMiddlewareDefinition =>
                (INeMiddleware<TContext>?)Activator.CreateInstance(
                    typeMiddlewareDefinition.Type) ?? throw new InvalidOperationException(),
            FactoryNeMiddlewareDefinition<TContext> factoryMiddlewareDefinition =>
                factoryMiddlewareDefinition.Factory(context),
            _ => throw new NotSupportedException(),
        };
    }

    private class Scope<TContext>(NeMiddlewareFactory factory, TContext context)
        : INeMiddlewareFactoryScope<TContext> where TContext : NeEventContext
    {
        public TContext Context { get; } = context;

        public void Dispose() { }

        public INeMiddleware<TContext> Create(
            INeMiddlewareDefinition<TContext> neMiddlewareDefinition
        )
        {
            return factory.Create(neMiddlewareDefinition, Context);
        }
    }
}
