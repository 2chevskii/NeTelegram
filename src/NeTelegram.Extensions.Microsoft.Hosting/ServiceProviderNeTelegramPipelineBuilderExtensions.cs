using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NeTelegram.Extensions.Microsoft.DependencyInjection;
using NeTelegram.Pipelines;

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

internal class NeTelegramPipelineLifetimeController(INeTelegramPipeline pipeline)
    : IHostedLifecycleService
{
    public Task StartingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        pipeline.Start();
        return Task.CompletedTask;
    }

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        pipeline.Stop();
        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
