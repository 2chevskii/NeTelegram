using System.Reflection;

namespace NeTelegram.Commands;

public static class CommandLocator
{
    public static IEnumerable<CommandDefinition> GetCommandsInAssembly(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(x => x.IsDefined(typeof(CommandAttribute)))
            .Where(x => x.IsAssignableTo(typeof(CommandBase)))
            .Select(x => new CommandDefinition(x.GetCustomAttribute<CommandAttribute>()!.Name, x));
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
