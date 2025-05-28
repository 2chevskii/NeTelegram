using Microsoft.Extensions.DependencyInjection;

namespace NeTelegram.Extensions.Microsoft.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static ServiceProviderNeTelegramPipelineBuilder AddNeTelegram(
        this IServiceCollection serviceCollection
    )
    {
        var builder = new ServiceProviderNeTelegramPipelineBuilder(serviceCollection);
        return builder;
    }

    public static ServiceProviderNeTelegramPipelineBuilder AddNeTelegram(
        this IServiceCollection serviceCollection,
        Action<ServiceProviderNeTelegramPipelineBuilder> configure
    )
    {
        var builder = new ServiceProviderNeTelegramPipelineBuilder(serviceCollection);
        configure(builder);
        return builder;
    }
}
