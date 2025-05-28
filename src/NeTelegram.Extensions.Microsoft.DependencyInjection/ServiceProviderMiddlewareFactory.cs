using Microsoft.Extensions.DependencyInjection;
using NeTelegram.Entities;
using NeTelegram.Middleware;

namespace NeTelegram.Extensions.Microsoft.DependencyInjection;

public class ServiceProviderMiddlewareFactory(IServiceProvider serviceProvider)
    : INeMiddlewareFactory
{
    public INeMiddlewareFactoryScope<TContext> CreateScope<TContext>(TContext context)
        where TContext : NeEventContext
    {
        var serviceScope = serviceProvider.CreateScope();
        return new Scope<TContext>(serviceScope, context, this);
    }

    private INeMiddleware<NeUpdateContext> Create(
        Scope<NeUpdateContext> scope,
        INeMiddlewareDefinition<NeUpdateContext> definition
    )
    {
        switch (definition)
        {
            case ServiceProviderMiddlewareDefinition<NeUpdateContext> def:
                return scope.ServiceScope.ServiceProvider.GetRequiredService(def.Type) as
                    INeMiddleware<NeUpdateContext>;

            default: throw new NotSupportedException();
        }
    }

    private INeMiddleware<NeErrorContext> Create(
        Scope<NeErrorContext> scope,
        INeMiddlewareDefinition<NeErrorContext> definition
    )
    {
        switch (definition)
        {
            case ServiceProviderMiddlewareDefinition<NeErrorContext> def:
                return scope.ServiceScope.ServiceProvider.GetRequiredService(def.Type) as
                    INeMiddleware<NeErrorContext>;

            default: throw new NotSupportedException();
        }
    }

    private class Scope<TContext>(
        IServiceScope serviceScope,
        TContext context,
        ServiceProviderMiddlewareFactory factory
    ) : IDisposable, INeMiddlewareFactoryScope<TContext> where TContext : NeEventContext
    {
        public TContext Context => context;
        public IServiceScope ServiceScope => serviceScope;

        public void Dispose()
        {
            serviceScope.Dispose();
        }

        public INeMiddleware<TContext> Create(INeMiddlewareDefinition<TContext> definition)
        {
            if (typeof(TContext) == typeof(NeUpdateContext))
            {
                return (INeMiddleware<TContext>)factory.Create(
                    this as Scope<NeUpdateContext>,
                    definition as INeMiddlewareDefinition<NeUpdateContext>
                );
            }

            if (typeof(TContext) == typeof(NeErrorContext))
            {
                return (INeMiddleware<TContext>)factory.Create(
                    this as Scope<NeErrorContext>,
                    definition as INeMiddlewareDefinition<NeErrorContext>
                );
            }

            throw new NotSupportedException();
        }
    }
}
