using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NeTelegram.Entities;
using NeTelegram.Middleware;
using NeTelegram.Pipelines;
using Telegram.Bot;

namespace NeTelegram.Extensions.Microsoft.DependencyInjection;

public class ServiceProviderNeTelegramPipelineBuilder
{
    private readonly IServiceCollection _serviceCollection;

    private readonly List<Func<IServiceProvider, ITelegramBotClient>> _clientFactories = new();
    private readonly List<INeMiddlewareDefinition<NeUpdateContext>> _updateMiddleware = new();
    private readonly List<INeMiddlewareDefinition<NeErrorContext>> _errorMiddleware = new();

    public IServiceCollection ServiceCollection => _serviceCollection;

    public ServiceProviderNeTelegramPipelineBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
        _serviceCollection.AddSingleton(Build);
    }

    public ServiceProviderNeTelegramPipelineBuilder AddClient(
        Func<IServiceProvider, ITelegramBotClient> factory
    )
    {
        _clientFactories.Add(factory);
        return this;
    }

    public ServiceProviderNeTelegramPipelineBuilder Use(Type middlewareType)
    {
        if (middlewareType.IsAssignableTo(typeof(INeMiddleware<NeUpdateContext>)))
        {
            _updateMiddleware.Add(
                new ServiceProviderMiddlewareDefinition<NeUpdateContext>(middlewareType)
            );
        }
        else if (middlewareType.IsAssignableTo(typeof(INeMiddleware<NeErrorContext>)))
        {
            _errorMiddleware.Add(
                new ServiceProviderMiddlewareDefinition<NeErrorContext>(middlewareType)
            );
        }
        else throw new ArgumentException();

        _serviceCollection.TryAddScoped(middlewareType);

        return this;
    }

    public ServiceProviderNeTelegramPipelineBuilder Use<T>() => Use(typeof(T));

    private INeTelegramPipeline Build(IServiceProvider serviceProvider)
    {

        var configuration = BuildConfiguration(serviceProvider);
        var middlewareFactory = new ServiceProviderMiddlewareFactory(serviceProvider);

        return new NeTelegramPipeline(configuration, middlewareFactory);
    }

    private INeTelegramPipelineConfiguration BuildConfiguration(IServiceProvider serviceProvider)
    {
        return new NeTelegramPipelineConfiguration(
            _clientFactories.Select(x => TransformClientFactory(x, serviceProvider)),
            _updateMiddleware,
            _errorMiddleware
        );
    }

    private Func<ITelegramBotClient> TransformClientFactory(
        Func<IServiceProvider, ITelegramBotClient> factory,
        IServiceProvider serviceProvider
    )
        => () => factory(serviceProvider);
}
