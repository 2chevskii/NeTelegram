using System.Reflection;
using NeTelegram.Entities;

namespace NeTelegram.Commands;

public class CommandHandlerFactory : ICommandHandlerFactory
{
    private static readonly FieldInfo _contextBackingField = typeof(CommandBase).GetField(
        GetBackingFieldName("Context"),
        BindingFlags.Instance | BindingFlags.NonPublic
    )!;

    private static readonly FieldInfo _argsBackingField = typeof(CommandBase).GetField(
        GetBackingFieldName("Args"),
        BindingFlags.Instance | BindingFlags.NonPublic
    )!;

    public virtual CommandBase Create(
        CommandDefinition commandDefinition,
        NeUpdateContext context,
        IEnumerable<string> args
    )
    {
        var command = CreateInstance(commandDefinition, context, args);
        BindParameters(command, context, args);
        return command;
    }

    protected virtual CommandBase CreateInstance(
        CommandDefinition commandDefinition,
        NeUpdateContext context,
        IEnumerable<string> args
    )
    {
        return (CommandBase)Activator.CreateInstance(commandDefinition.HandlerType)!;
    }

    protected virtual void BindParameters(
        CommandBase command,
        NeUpdateContext context,
        IEnumerable<string> args
    )
    {
        _contextBackingField.SetValue(command, context);
        _argsBackingField.SetValue(command, args.ToArray());
    }

    private static string GetBackingFieldName(string propertyName)
        => $"<{propertyName}>k__BackingField";
}
