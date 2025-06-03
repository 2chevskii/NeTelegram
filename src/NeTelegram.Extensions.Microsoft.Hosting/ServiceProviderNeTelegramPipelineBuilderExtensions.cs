using Microsoft.Extensions.DependencyInjection;
using NeTelegram.Extensions.Microsoft.DependencyInjection;

namespace NeTelegram.Extensions.Microsoft.Hosting;

public static class ServiceProviderNeTelegramPipelineBuilderExtensions
{
    public static ServiceProviderNeTelegramPipelineBuilder UseHostLifetime(
        this ServiceProviderNeTelegramPipelineBuilder builder
    )
    {
        builder.ServiceCollection.AddHostedService<NeTelegramPipelineLifetimeController>();
        return builder;
    }
}
