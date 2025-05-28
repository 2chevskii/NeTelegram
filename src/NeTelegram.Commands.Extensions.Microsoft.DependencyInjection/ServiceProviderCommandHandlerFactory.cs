using Microsoft.Extensions.DependencyInjection;
using NeTelegram.Entities;

namespace NeTelegram.Commands.Extensions.Microsoft.DependencyInjection;

public class ServiceProviderCommandHandlerFactory(IServiceProvider serviceProvider)
    : CommandHandlerFactory
{
    protected override CommandBase CreateInstance(
        CommandDefinition commandDefinition,
        NeUpdateContext context,
        IEnumerable<string> args
    )
    {
        return (CommandBase)serviceProvider.GetRequiredService(commandDefinition.HandlerType);
    }
}