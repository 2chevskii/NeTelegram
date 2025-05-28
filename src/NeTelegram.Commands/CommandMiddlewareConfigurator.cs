using System.Reflection;

namespace NeTelegram.Commands;

public class CommandMiddlewareConfigurator
{
    private readonly List<CommandDefinition> _commands = new();

    public CommandRegistry BuildCommandRegistry()
    {
        return new CommandRegistry(_commands);
    }

    public virtual CommandMiddlewareConfigurator WithCommands(
        IEnumerable<CommandDefinition> commands
    )
    {
        _commands.AddRange(commands);
        return this;
    }

    public CommandMiddlewareConfigurator WithCommands(params CommandDefinition[] commands)
    {
        return WithCommands((IEnumerable<CommandDefinition>)commands);
    }

    public CommandMiddlewareConfigurator WithCommandsInAssembly(Assembly assembly)
    {
        return WithCommands(CommandLocator.GetCommandsInAssembly(assembly));
    }

    public CommandMiddlewareConfigurator WithCommandsInAssemblyOf<T>()
    {
        return WithCommands(CommandLocator.GetCommandsInAssemblyOf<T>());
    }

    public CommandMiddlewareConfigurator WithCommandsInAssemblies(IEnumerable<Assembly> assemblies)
    {
        return WithCommands(CommandLocator.GetCommandsInAssemblies(assemblies));
    }

    public CommandMiddlewareConfigurator WithCommandsInAssemblies(params Assembly[] assemblies)
    {
        return WithCommands(CommandLocator.GetCommandsInAssemblies(assemblies));
    }

    public CommandMiddlewareConfigurator WithCommandsInAppDomain(AppDomain domain)
    {
        return WithCommands(CommandLocator.GetCommandsInAppDomain(domain));
    }

    public CommandMiddlewareConfigurator WithCommandsInCurrentAppDomain()
    {
        return WithCommands(CommandLocator.GetCommandsInCurrentAppDomain());
    }
}
