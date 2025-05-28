using NeTelegram.Pipelines;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NeTelegram.Entities;

public sealed record NeUpdateContext(
    INeTelegramPipeline Pipeline,
    ITelegramBotClient Client,
    Update Update,
    CancellationToken CancellationToken
) : NeEventContext(Pipeline, Client, CancellationToken);
