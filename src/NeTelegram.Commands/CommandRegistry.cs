using System.Diagnostics.CodeAnalysis;

namespace NeTelegram.Commands;

public class CommandRegistry(IEnumerable<CommandDefinition> commands)
{
    public readonly Dictionary<string, CommandDefinition> CommandIndex =
        commands.ToDictionary(x => x.Name);

    public bool TryGetCommand(string name, [NotNullWhen(true)] out CommandDefinition? command)
    {
        return CommandIndex.TryGetValue(name, out command);
    }
}
