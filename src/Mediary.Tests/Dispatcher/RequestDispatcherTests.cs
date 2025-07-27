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
    public async Task DispatchAsync_CallsHandler()
    {
        // Arrange
        var request = new SampleRequest();
        var expectedResponse = new SampleResponse();
        var handlerMock = new Mock<IRequestHandler<SampleResponse, SampleRequest>>();
        handlerMock.Setup(h => h.HandleAsync(request)).ReturnsAsync(expectedResponse);

        var serviceProvider = new ServiceCollection()
            .AddSingleton(handlerMock.Object)
            .BuildServiceProvider();

        var dispatcher = new RequestDispatcher(serviceProvider);

        // Act
        var response = await dispatcher.DispatchAsync<SampleResponse, SampleRequest>(request);

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
            dispatcher.DispatchAsync<SampleResponse, SampleRequest>(request));

        Assert.Contains("No service for", ex.Message);
        Assert.Contains(request.GetType().Name, ex.Message);
    }
}
