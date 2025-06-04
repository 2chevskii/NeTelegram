using System.Reflection;

namespace NeTelegram.Commands;

public static class CommandLocator
{
    public static CommandDefinition FromHandlerType(Type handlerType)
    {
        if (!handlerType.IsDefined(typeof(CommandAttribute)))
        {
            throw new ArgumentException(
                $"Attribute {nameof(CommandAttribute)} must be defined on handler type"
            );
        }

        if (!handlerType.IsAssignableTo(typeof(CommandBase)))
        {
            throw new ArgumentException(
                $"Handler type must be inherited from {nameof(CommandBase)}"
            );
        }

        var attribute = handlerType.GetCustomAttribute<CommandAttribute>()!;
        return new CommandDefinition(attribute.Name, attribute.Description, handlerType);
    }

    public static CommandDefinition FromHandlerType<T>() where T : CommandBase
        => FromHandlerType(typeof(T));

    public static IEnumerable<CommandDefinition> GetCommandsInAssembly(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(x => x.IsDefined(typeof(CommandAttribute)))
            .Where(x => x.IsAssignableTo(typeof(CommandBase)))
            .Select(FromHandlerType);
    }

    public static IEnumerable<CommandDefinition> GetCommandsInAssemblyOf<T>()
    {
        return GetCommandsInAssembly(typeof(T).Assembly);
    }

    public static IEnumerable<CommandDefinition> GetCommandsInAssemblies(
        IEnumerable<Assembly> assemblies
    )
    {
        return assemblies.SelectMany(GetCommandsInAssembly);
    }

    public static IEnumerable<CommandDefinition> GetCommandsInAssemblies(
        params Assembly[] assemblies
    )
    {
        return assemblies.SelectMany(GetCommandsInAssembly);
    }

    public static IEnumerable<CommandDefinition> GetCommandsInAppDomain(AppDomain domain)
    {
        return GetCommandsInAssemblies(domain.GetAssemblies());
    }

    public static IEnumerable<CommandDefinition> GetCommandsInCurrentAppDomain()
    {
        return GetCommandsInAppDomain(AppDomain.CurrentDomain);
    }
}
