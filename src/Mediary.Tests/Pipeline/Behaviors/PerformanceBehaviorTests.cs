using Mediary.Pipeline.Behaviors;
using Mediary.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Mediary.Tests.Pipeline.Behaviors;

public class PerformanceBehaviorTests
{
    [Fact]
    public async Task HandleAsync_LogsExecutionTime()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<PerformanceBehavior<SampleResponse, SampleRequestWithResponse>>>();
        var behavior = new PerformanceBehavior<SampleResponse, SampleRequestWithResponse>(loggerMock.Object);

        var request = new SampleRequestWithResponse();
        var response = new SampleResponse();

        // Act
        var result = await behavior.HandleAsync(request, () => Task.FromResult(response));

        // Assert
        Assert.Equal(response, result);

        loggerMock.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) =>
                o.ToString()!.Contains("Handling")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        loggerMock.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) =>
                o.ToString()!.Contains("Handled")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_LogsErrorOnException()
    {
        var loggerMock = new Mock<ILogger<PerformanceBehavior<SampleResponse, SampleRequestWithResponse>>>();
        var behavior = new PerformanceBehavior<SampleResponse, SampleRequestWithResponse>(loggerMock.Object);

        var request = new SampleRequestWithResponse();
        var exception = new InvalidOperationException("Test failure");

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            behavior.HandleAsync(request, () => throw exception));

        Assert.Equal(exception, ex);

        loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Error handling")),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

