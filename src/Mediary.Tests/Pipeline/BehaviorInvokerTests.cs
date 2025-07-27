using Mediary.Core;
using Mediary.Pipeline;
using Mediary.Tests.Helpers;
using Moq;
using Xunit;

namespace Mediary.Tests.Pipeline;

public class BehaviorInvokerTests
{
    [Fact]
    public async Task ExecuteWithPipeline_NoBehaviors_CallsHandlerAndReturns()
    {
        // Arrange
        var request = new SampleRequest();
        var expectedResponse = new SampleResponse();

        var handlerMock = new Mock<IRequestHandler<SampleResponse, SampleRequest>>();
        handlerMock.Setup(h => h.HandleAsync(request)).ReturnsAsync(expectedResponse);

        // Act
        var result = await BehaviorInvoker.ExecuteWithPipeline(
            request,
            handlerMock.Object,
            []
        );

        // Assert
        Assert.Equal(expectedResponse, result);
        handlerMock.Verify(h => h.HandleAsync(request), Times.Once);
    }

    [Fact]
    public async Task ExecuteWithPipeline_BehaviorsCalledInOrder()
    {
        // Arrange
        var request = new SampleRequest();
        var callOrder = new List<string>();
        var expectedResponse = new SampleResponse();

        var behavior1 = new Mock<IRequestPipelineBehavior<SampleResponse, SampleRequest>>();
        behavior1
            .Setup(b => b.HandleAsync(request, It.IsAny<Func<Task<SampleResponse>>>()))
            .Returns(async (SampleRequest _, Func<Task<SampleResponse>> next) =>
            {
                callOrder.Add("behavior1-before");
                var result = await next();
                callOrder.Add("behavior1-after");
                return result;
            });

        var behavior2 = new Mock<IRequestPipelineBehavior<SampleResponse, SampleRequest>>();
        behavior2
            .Setup(b => b.HandleAsync(request, It.IsAny<Func<Task<SampleResponse>>>()))
            .Returns(async (SampleRequest _, Func<Task<SampleResponse>> next) =>
            {
                callOrder.Add("behavior2-before");
                var result = await next();
                callOrder.Add("behavior2-after");
                return result;
            });

        var handlerMock = new Mock<IRequestHandler<SampleResponse, SampleRequest>>();
        handlerMock
            .Setup(h => h.HandleAsync(request))
            .Returns(() =>
            {
                callOrder.Add("handler");
                return Task.FromResult(expectedResponse);
            });

        // Act
        var result = await BehaviorInvoker.ExecuteWithPipeline(
            request,
            handlerMock.Object,
            [behavior1.Object, behavior2.Object]
        );

        // Assert
        Assert.Equal(expectedResponse, result);
        var expectedOrder = new[]
        {
            "behavior1-before",
            "behavior2-before",
            "handler",
            "behavior2-after",
            "behavior1-after"
        };

        Assert.Equal(expectedOrder, callOrder);
    }    

    [Fact]
    public async Task ExecuteWithPipeline_ShortCircuits_ReturnsEarly()
    {
        // Arrange
        var request = new SampleRequest();
        var expectedResponse = new SampleResponse();

        var behavior = new Mock<IRequestPipelineBehavior<SampleResponse, SampleRequest>>();
        behavior
            .Setup(b => b.HandleAsync(request, It.IsAny<Func<Task<SampleResponse>>>()))
            .Returns((SampleRequest _, Func<Task<SampleResponse>> next) => Task.FromResult(expectedResponse));

        var handlerMock = new Mock<IRequestHandler<SampleResponse, SampleRequest>>();

        // Act
        var result = await BehaviorInvoker.ExecuteWithPipeline(
            request,
            handlerMock.Object,
            [behavior.Object]
        );

        // Assert
        Assert.Equal(expectedResponse, result);
        handlerMock.Verify(h => h.HandleAsync(It.IsAny<SampleRequest>()), Times.Never);
    }
}
