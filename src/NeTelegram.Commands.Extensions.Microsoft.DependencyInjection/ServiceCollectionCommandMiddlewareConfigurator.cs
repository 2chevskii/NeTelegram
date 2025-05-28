using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace NeTelegram.Commands;

public class ServiceCollectionCommandMiddlewareConfigurator(IServiceCollection serviceCollection)
    : CommandMiddlewareConfigurator
{
    public override CommandMiddlewareConfigurator WithCommands(
        IEnumerable<CommandDefinition> commands
    )
    {
        foreach (var commandDefinition in commands)
        {
            serviceCollection.TryAddScoped(commandDefinition.HandlerType);
        }

        return base.WithCommands(commands);
    }
}
