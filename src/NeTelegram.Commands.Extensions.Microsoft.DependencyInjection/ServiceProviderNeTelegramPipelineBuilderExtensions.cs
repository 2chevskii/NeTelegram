using Microsoft.Extensions.DependencyInjection;
using NeTelegram.Extensions.Microsoft.DependencyInjection;

namespace NeTelegram.Commands.Extensions.Microsoft.DependencyInjection;

public static class ServiceProviderNeTelegramPipelineBuilderExtensions
{
    public static ServiceProviderNeTelegramPipelineBuilder UseCommands(
        this ServiceProviderNeTelegramPipelineBuilder builder,
        Action<CommandMiddlewareConfigurator> configure
    )
    {
        builder.Use<CommandNeMiddleware>();

        var configurator =
            new ServiceCollectionCommandMiddlewareConfigurator(builder.ServiceCollection);
        configure(configurator);

        builder.ServiceCollection.AddScoped(_ => configurator.BuildCommandRegistry());
        builder.ServiceCollection
            .AddScoped<ICommandHandlerFactory, ServiceProviderCommandHandlerFactory>();

        return builder;
    }
}
