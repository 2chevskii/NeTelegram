using NeTelegram.Entities;
using Telegram.Bot;

namespace NeTelegram.Commands;

public abstract class CommandBase
{
    protected NeUpdateContext Context { get; }
    protected IReadOnlyList<string> Args { get; }

    protected ITelegramBotClient Client => Context.Client;

    public abstract Task Invoke();
}
