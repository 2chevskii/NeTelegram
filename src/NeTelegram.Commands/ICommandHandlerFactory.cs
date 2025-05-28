using NeTelegram.Entities;

namespace NeTelegram.Commands;

public interface ICommandHandlerFactory
{
    CommandBase Create(
        CommandDefinition commandDefinition,
        NeUpdateContext context,
        IEnumerable<string> args
    );
}
