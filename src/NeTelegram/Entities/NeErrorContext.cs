using NeTelegram.Pipelines;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace NeTelegram.Entities;

public sealed record NeErrorContext(
    INeTelegramPipeline Pipeline,
    ITelegramBotClient Client,
    Exception Exception,
    HandleErrorSource Source,
    CancellationToken CancellationToken
) : NeEventContext(Pipeline, Client, CancellationToken);
