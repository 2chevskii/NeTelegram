using NeTelegram.Entities;
using NeTelegram.Middleware;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace NeTelegram.Pipelines;

public sealed class NeTelegramPipeline(
    INeTelegramPipelineConfiguration configuration,
    INeMiddlewareFactory middlewareFactory
) : INeTelegramPipeline
{
    private readonly List<ClientRegistration> _registrations = new();

    public void Start()
    {
        Stop();
        _registrations.AddRange(
            configuration.GetClients().Select(bot => new ClientRegistration(bot, this))
        );
        _registrations.ForEach(x => x.Start());
    }

    public void Stop()
    {
        _registrations.ForEach(x => x.Dispose());
        _registrations.Clear();
    }

    private async Task OnUpdate(
        ITelegramBotClient client,
        Update update,
        CancellationToken cancellationToken
    )
    {
        var context = new NeUpdateContext(this, client, update, cancellationToken);
        var middlewareQueue = configuration.GetUpdateMiddleware();
        using var middlewareFactoryScope = middlewareFactory.CreateScope(context);
        await InvokeNext(middlewareFactoryScope, middlewareQueue);
    }

    private async Task OnError(
        ITelegramBotClient client,
        Exception exception,
        HandleErrorSource source,
        CancellationToken cancellationToken
    )
    {
        var context = new NeErrorContext(this, client, exception, source, cancellationToken);
        var middlewareQueue = configuration.GetErrorMiddleware();
        using var middlewareFactoryScope = middlewareFactory.CreateScope(context);
        await InvokeNext(middlewareFactoryScope, middlewareQueue);
    }

    private async Task InvokeNext<TContext>(
        INeMiddlewareFactoryScope<TContext> neMiddlewareFactoryScope,
        Queue<INeMiddlewareDefinition<TContext>> queue
    ) where TContext : NeEventContext
    {
        if (!queue.TryDequeue(out var middlewareDefinition))
        {
            return;
        }

        var middleware = neMiddlewareFactoryScope.Create(middlewareDefinition);
        await middleware.Invoke(
            neMiddlewareFactoryScope.Context,
            () => InvokeNext(neMiddlewareFactoryScope, queue)
        );
    }

    private sealed class ClientRegistration(ITelegramBotClient client, NeTelegramPipeline pipeline)
        : IUpdateHandler, IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public void Start()
        {
            client.StartReceiving(this, cancellationToken: _cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public Task HandleUpdateAsync(
            ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken
        )
        {
            return pipeline.OnUpdate(botClient, update, cancellationToken);
        }

        public Task HandleErrorAsync(
            ITelegramBotClient botClient,
            Exception exception,
            HandleErrorSource source,
            CancellationToken cancellationToken
        )
        {
            return pipeline.OnError(botClient, exception, source, cancellationToken);
        }
    }
}
