using NeTelegram.Entities;
using NeTelegram.Middleware;
using Telegram.Bot;

namespace NeTelegram.Pipelines;

public interface INeTelegramPipelineConfiguration
{
    IEnumerable<ITelegramBotClient> GetClients();

    Queue<INeMiddlewareDefinition<NeUpdateContext>> GetUpdateMiddleware();

    Queue<INeMiddlewareDefinition<NeErrorContext>> GetErrorMiddleware();
}
