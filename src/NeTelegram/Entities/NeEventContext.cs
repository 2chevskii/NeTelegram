using NeTelegram.Pipelines;
using Telegram.Bot;

namespace NeTelegram.Entities;

public abstract record NeEventContext(
    INeTelegramPipeline Pipeline,
    ITelegramBotClient Client,
    CancellationToken CancellationToken
);
