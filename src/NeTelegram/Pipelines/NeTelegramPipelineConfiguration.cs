using NeTelegram.Entities;
using NeTelegram.Middleware;
using Telegram.Bot;

namespace NeTelegram.Pipelines;

public sealed class NeTelegramPipelineConfiguration(
    IEnumerable<Func<ITelegramBotClient>> clientFactories,
    IEnumerable<INeMiddlewareDefinition<NeUpdateContext>> updateMiddleware,
    IEnumerable<INeMiddlewareDefinition<NeErrorContext>> errorMiddleware
) : INeTelegramPipelineConfiguration
{
    public IEnumerable<ITelegramBotClient> GetClients()
    {
        return clientFactories.Select(x => x());
    }

    public Queue<INeMiddlewareDefinition<NeUpdateContext>> GetUpdateMiddleware()
    {
        return new Queue<INeMiddlewareDefinition<NeUpdateContext>>(updateMiddleware);
    }

    public Queue<INeMiddlewareDefinition<NeErrorContext>> GetErrorMiddleware()
    {
        return new Queue<INeMiddlewareDefinition<NeErrorContext>>(errorMiddleware);
    }
}
