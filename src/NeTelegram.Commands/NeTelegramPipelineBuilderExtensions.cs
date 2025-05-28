using NeTelegram.Pipelines;

namespace NeTelegram.Commands;

public static class NeTelegramPipelineBuilderExtensions
{
    public static NeTelegramPipelineBuilder UseCommands(
        this NeTelegramPipelineBuilder builder,
        Action<CommandMiddlewareConfigurator> configure
    )
    {
        var configurator = new CommandMiddlewareConfigurator();
        configure(configurator);
        builder.Use(() => new CommandNeMiddleware(
                configurator.BuildCommandRegistry(),
                new CommandHandlerFactory()
            )
        );
        return builder;
    }
}
