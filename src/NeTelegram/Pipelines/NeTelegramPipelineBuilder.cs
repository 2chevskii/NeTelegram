using NeTelegram.Entities;
using NeTelegram.Middleware;
using Telegram.Bot;

namespace NeTelegram.Pipelines;

public class NeTelegramPipelineBuilder
{
    private readonly List<Func<ITelegramBotClient>> _clientFactories = new();
    private readonly List<INeMiddlewareDefinition<NeUpdateContext>> _updateMiddleware = new();
    private readonly List<INeMiddlewareDefinition<NeErrorContext>> _errorMiddleware = new();

    public INeTelegramPipeline Build()
    {
        return new NeTelegramPipeline(
            new NeTelegramPipelineConfiguration(
                _clientFactories,
                _updateMiddleware,
                _errorMiddleware
            ),
            new NeMiddlewareFactory()
        );
    }

    public NeTelegramPipelineBuilder AddClient(Func<ITelegramBotClient> factory)
    {
        _clientFactories.Add(factory);
        return this;
    }

    public NeTelegramPipelineBuilder AddClient(ITelegramBotClient client)
    {
        return AddClient(() => client);
    }

    public NeTelegramPipelineBuilder Use(Type middlewareType)
    {
        if (middlewareType.IsAssignableTo(typeof(INeMiddleware<NeUpdateContext>)))
        {
            _updateMiddleware.Add(new TypeNeMiddlewareDefinition<NeUpdateContext>(middlewareType));

        }
        else if (middlewareType.IsAssignableTo(typeof(INeMiddleware<NeErrorContext>)))
        {
            _errorMiddleware.Add(new TypeNeMiddlewareDefinition<NeErrorContext>(middlewareType));
        }
        else throw new ArgumentException();

        return this;
    }

    public NeTelegramPipelineBuilder Use<T>()
    {
        return Use(typeof(T));
    }

    public NeTelegramPipelineBuilder Use(
        Func<NeUpdateContext, INeMiddleware<NeUpdateContext>> factory
    )
    {
        _updateMiddleware.Add(new FactoryNeMiddlewareDefinition<NeUpdateContext>(factory));
        return this;
    }

    public NeTelegramPipelineBuilder Use(Func<INeMiddleware<NeUpdateContext>> factory)
    {
        return Use(_ => factory());
    }

    public NeTelegramPipelineBuilder Use(
        Func<NeErrorContext, INeMiddleware<NeErrorContext>> factory
    )
    {
        _errorMiddleware.Add(new FactoryNeMiddlewareDefinition<NeErrorContext>(factory));
        return this;
    }

    public NeTelegramPipelineBuilder Use(Func<INeMiddleware<NeErrorContext>> factory)
    {
        return Use(_ => factory());
    }
}
