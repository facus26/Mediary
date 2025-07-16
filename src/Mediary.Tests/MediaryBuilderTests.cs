using Mediary.Core;
using Mediary.Pipeline;
using Mediary.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Mediary.Tests;

public class MediaryBuilderTests
{
    [Fact]
    public void AddRequestHandler_Registers_Handler_Correctly()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new MediaryBuilder(services);

        builder.AddRequestHandler<SampleRequest, SampleRequestHandler>();

        // Act
        var provider = services.BuildServiceProvider();
        var handler = provider.GetService<IRequestHandler<SampleRequest>>();

        // Assert
        Assert.NotNull(handler);
        Assert.IsType<SampleRequestHandler>(handler);
    }

    [Fact]
    public void AddRequestHandler_WithResponse_Registers_Handler_Correctly()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new MediaryBuilder(services);

        builder.AddRequestHandler<SampleResponse, SampleRequestWithResponse, SampleRequestWithResponseHandler>();

        // Act
        var provider = services.BuildServiceProvider();
        var handler = provider.GetService<IRequestHandler<SampleResponse, SampleRequestWithResponse>>();

        // Assert
        Assert.NotNull(handler);
        Assert.IsType<SampleRequestWithResponseHandler>(handler);
    }

    [Fact]
    public void AddPipelineBehavior_Registers_NonGeneric_Correctly()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new MediaryBuilder(services);

        builder.AddPipelineBehaviors<SampleRequest, SampleLoggingBehavior>();

        // Act
        var provider = services.BuildServiceProvider();
        var behavior = provider.GetService<IRequestPipelineBehavior<SampleRequest>>();

        // Assert
        Assert.NotNull(behavior);
        Assert.IsType<SampleLoggingBehavior>(behavior);
    }

    [Fact]
    public void AddPipelineBehavior_WithResponse_Registers_Correctly()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new MediaryBuilder(services);

        builder.AddPipelineBehaviors<SampleResponse, SampleRequestWithResponse, SampleLoggingBehaviorWithResponse>();

        // Act
        var provider = services.BuildServiceProvider();
        var behavior = provider.GetService<IRequestPipelineBehavior<SampleResponse, SampleRequestWithResponse>>();

        // Assert
        Assert.NotNull(behavior);
        Assert.IsType<SampleLoggingBehaviorWithResponse>(behavior);
    }

    [Fact]
    public void AddOpenPipelineBehaviors_Registers_OpenGeneric_Correctly()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new MediaryBuilder(services);

        builder.AddOpenPipelineBehaviors(typeof(SampleOpenLoggingBehavior<,>));

        // Act
        var provider = services.BuildServiceProvider();
        var behavior = provider.GetService<IRequestPipelineBehavior<SampleResponse, SampleRequestWithResponse>>();

        // Assert
        Assert.NotNull(behavior);
        Assert.IsType<SampleOpenLoggingBehavior<SampleResponse, SampleRequestWithResponse>>(behavior);
    }

    [Fact]
    public void AddOpenPipelineBehaviors_InvalidType_Throws()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new MediaryBuilder(services);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            builder.AddOpenPipelineBehaviors(typeof(object)));

        Assert.Contains("does not implement any valid", ex.Message);
    }

    [Fact]
    public void AddRequestHandlersFromAssembly_Registers_AllHandlers()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new MediaryBuilder(services);

        builder.AddRequestHandlersFromAssembly(typeof(SampleRequestHandler).Assembly);

        // Act
        var provider = services.BuildServiceProvider();
        var handler = provider.GetService<IRequestHandler<SampleRequest>>();

        // Assert
        Assert.NotNull(handler);
        Assert.IsType<SampleRequestHandler>(handler);
    }

    [Fact]
    public void AddPipelineBehaviorsFromAssembly_Registers_AllBehaviors()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new MediaryBuilder(services);

        builder.AddPipelineBehaviorsFromAssembly(typeof(SampleLoggingBehavior).Assembly);

        // Act
        var provider = services.BuildServiceProvider();
        var behavior = provider.GetService<IRequestPipelineBehavior<SampleRequest>>();

        // Assert
        Assert.NotNull(behavior);
        Assert.IsType<SampleLoggingBehavior>(behavior);
    }
}
