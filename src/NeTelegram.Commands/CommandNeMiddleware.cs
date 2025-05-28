using NeTelegram.Entities;
using NeTelegram.Middleware;
using Telegram.Bot;

namespace NeTelegram.Commands;

public class CommandNeMiddleware(CommandRegistry registry, ICommandHandlerFactory handlerFactory)
    : PassthroughNeMiddleware<NeUpdateContext>
{
    protected override async Task Invoke(NeUpdateContext context)
    {
        var update = context.Update;

        if (update.Message is not { Text: not null })
        {
            return;
        }

        if (!CommandParser.TryParse(
                update.Message.Text,
                out var command,
                out var parsedUsername,
                out var args
            ))
        {
            return;
        }

        Console.WriteLine(
            format: "Parsed command: {0}, {1}, {2}",
            command,
            parsedUsername,
            $"[{string.Join(separator: ", ", args)}]"
        );

        if (!registry.TryGetCommand(command, out var commandDefinition))
        {
            Console.WriteLine(format: "Cannot find command definition for command: {0}", command);
            return;
        }

        Console.WriteLine(format: "Command handler type: {0}", commandDefinition.HandlerType);

        var handler = handlerFactory.Create(commandDefinition, context, args);

        await handler.Invoke();
    }

    private async ValueTask<string> GetBotUsername(NeUpdateContext context)
    {
        var botInfo = await context.Client.GetMe();
        return botInfo.Username ?? string.Empty;
    }
}
