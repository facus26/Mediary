using Mediary.Core;
using Mediary.Dispatcher;
using Mediary.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Mediary.Tests.Dispatcher;

public class RequestDispatcherTests
{
    [Fact]
    public async Task DispatchAsync_CallsHandlerWithoutBehaviors()
    {
        // Arrange
        var request = new SampleRequest();
        var handlerMock = new Mock<IRequestHandler<SampleRequest>>();
        handlerMock.Setup(h => h.HandleAsync(request)).Returns(Task.CompletedTask);

        var serviceProvider = new ServiceCollection()
            .AddSingleton(handlerMock.Object)
            .BuildServiceProvider();

        var dispatcher = new RequestDispatcher(serviceProvider);

        // Act
        await dispatcher.DispatchAsync(request);

        // Assert
        handlerMock.Verify(h => h.HandleAsync(request), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_CallsHandlerWithResponse()
    {
        // Arrange
        var request = new SampleRequestWithResponse();
        var expectedResponse = new SampleResponse();
        var handlerMock = new Mock<IRequestHandler<SampleResponse, SampleRequestWithResponse>>();
        handlerMock.Setup(h => h.HandleAsync(request)).ReturnsAsync(expectedResponse);

        var serviceProvider = new ServiceCollection()
            .AddSingleton(handlerMock.Object)
            .BuildServiceProvider();

        var dispatcher = new RequestDispatcher(serviceProvider);

        // Act
        var response = await dispatcher.ExecuteAsync<SampleResponse, SampleRequestWithResponse>(request);

        // Assert
        Assert.Equal(expectedResponse, response);
        handlerMock.Verify(h => h.HandleAsync(request), Times.Once);
    }

    [Fact]
    public async Task DispatchAsync_ThrowsIfHandlerNotRegistered()
    {
        // Arrange
        var request = new SampleRequest();
        var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var dispatcher = new RequestDispatcher(serviceProvider);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            dispatcher.DispatchAsync(request));

        Assert.Contains("No service for", ex.Message);
        Assert.Contains(request.GetType().Name, ex.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ThrowsIfHandlerNotRegistered()
    {
        // Arrange
        var request = new SampleRequestWithResponse();
        var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var dispatcher = new RequestDispatcher(serviceProvider);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            dispatcher.ExecuteAsync<SampleResponse, SampleRequestWithResponse>(request));

        Assert.Contains("No service for", ex.Message);
        Assert.Contains(request.GetType().Name, ex.Message);
    }
}
