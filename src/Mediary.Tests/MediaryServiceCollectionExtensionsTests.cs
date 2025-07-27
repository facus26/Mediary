using Mediary.Core;
using Mediary.Dispatcher;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Mediary.Tests;

public class MediaryServiceCollectionExtensionsTests
{
    [Fact]
    public void AddMediary_RegistersDefaultDispatcher()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var builder = services.AddMediary();

        // Assert
        Assert.NotNull(builder);
        Assert.IsType<MediaryBuilder>(builder);

        var provider = services.BuildServiceProvider();
        var dispatcher = provider.GetService<IRequestDispatcher>();

        Assert.NotNull(dispatcher);
        Assert.IsType<RequestDispatcher>(dispatcher);
    }

    [Fact]
    public void AddMediary_WithCustomDispatcher_RegistersCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var builder = services.AddMediary<FakeDispatcher>();

        // Assert
        Assert.NotNull(builder);
        Assert.IsType<MediaryBuilder>(builder);

        var provider = services.BuildServiceProvider();
        var dispatcher = provider.GetService<IRequestDispatcher>();

        Assert.NotNull(dispatcher);
        Assert.IsType<FakeDispatcher>(dispatcher);
    }

    private sealed class FakeDispatcher : IRequestDispatcher
    {
        public Task<TResponse> DispatchAsync<TResponse, TRequest>(TRequest request)
            where TRequest : IRequest<TResponse> =>
            Task.FromResult(default(TResponse)!);
    }
}
