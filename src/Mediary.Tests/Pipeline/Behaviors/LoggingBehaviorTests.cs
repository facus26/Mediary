using Mediary.Pipeline.Behaviors;
using Mediary.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Mediary.Tests.Pipeline.Behaviors;
    
public class LoggingBehaviorTests
{
    [Fact]
    public async Task HandleAsync_LogsBeforeAndAfterHandling()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehavior<SampleResponse, SampleRequestWithResponse>>>();
        var behavior = new LoggingBehavior<SampleResponse, SampleRequestWithResponse>(loggerMock.Object);

        var request = new SampleRequestWithResponse();
        var response = new SampleResponse();

        // Act
        var result = await behavior.HandleAsync(request, () => Task.FromResult(response));

        // Assert
        Assert.Equal(response, result);
        loggerMock.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Handling request")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        loggerMock.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Successfully handled request")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_LogsErrorOnException()
    {
        var loggerMock = new Mock<ILogger<LoggingBehavior<SampleResponse, SampleRequestWithResponse>>>();
        var behavior = new LoggingBehavior<SampleResponse, SampleRequestWithResponse>(loggerMock.Object);

        var request = new SampleRequestWithResponse();
        var exception = new InvalidOperationException("Test failure");

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            behavior.HandleAsync(request, () => throw exception));

        Assert.Equal(exception, ex);

        loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Exception while handling request")),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
