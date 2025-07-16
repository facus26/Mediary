using Mediary.Core;
using Mediary.Pipeline;
using Mediary.Tests.Helpers;
using Moq;
using Xunit;

namespace Mediary.Tests.Pipeline;

public class BehaviorInvokerTests
{
    [Fact]
    public async Task ExecuteWithPipeline_NoBehaviors_CallsHandler()
    {
        // Arrange
        var request = new SampleRequest();
        var handlerMock = new Mock<IRequestHandler<SampleRequest>>();
        handlerMock.Setup(h => h.HandleAsync(request)).Returns(Task.CompletedTask);

        // Act
        await BehaviorInvoker.ExecuteWithPipeline(request, handlerMock.Object, []);

        // Assert
        handlerMock.Verify(h => h.HandleAsync(request), Times.Once);
    }

    [Fact]
    public async Task ExecuteWithPipeline_BehaviorsCalledInOrder()
    {
        // Arrange
        var request = new SampleRequest();
        var callOrder = new List<string>();

        var behavior1 = new Mock<IRequestPipelineBehavior<SampleRequest>>();
        behavior1
            .Setup(b => b.HandleAsync(request, It.IsAny<Func<Task>>()))
            .Returns(async (SampleRequest _, Func<Task> next) =>
            {
                callOrder.Add("behavior1-before");
                await next();
                callOrder.Add("behavior1-after");
            });

        var behavior2 = new Mock<IRequestPipelineBehavior<SampleRequest>>();
        behavior2
            .Setup(b => b.HandleAsync(request, It.IsAny<Func<Task>>()))
            .Returns(async (SampleRequest _, Func<Task> next) =>
            {
                callOrder.Add("behavior2-before");
                await next();
                callOrder.Add("behavior2-after");
            });

        var handlerMock = new Mock<IRequestHandler<SampleRequest>>();
        handlerMock
            .Setup(h => h.HandleAsync(request))
            .Returns(() =>
            {
                callOrder.Add("handler");
                return Task.CompletedTask;
            });

        // Act
        await BehaviorInvoker.ExecuteWithPipeline(request, handlerMock.Object, [behavior1.Object, behavior2.Object]);

        // Assert
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
    public async Task ExecuteWithPipeline_BehaviorShortCircuits_DoesNotCallNext()
    {
        // Arrange
        var request = new SampleRequest();

        var behavior = new Mock<IRequestPipelineBehavior<SampleRequest>>();
        behavior
            .Setup(b => b.HandleAsync(request, It.IsAny<Func<Task>>()))
            .Returns((SampleRequest _, Func<Task> next) => Task.CompletedTask);

        var handlerMock = new Mock<IRequestHandler<SampleRequest>>();

        // Act
        await BehaviorInvoker.ExecuteWithPipeline(request, handlerMock.Object, [behavior.Object]);

        // Assert
        handlerMock.Verify(h => h.HandleAsync(It.IsAny<SampleRequest>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteWithPipeline_Response_NoBehaviors_CallsHandlerAndReturns()
    {
        // Arrange
        var request = new SampleRequestWithResponse();
        var expectedResponse = new SampleResponse();

        var handlerMock = new Mock<IRequestHandler<SampleResponse, SampleRequestWithResponse>>();
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
    public async Task ExecuteWithPipeline_Response_BehaviorsCalledInOrder()
    {
        // Arrange
        var request = new SampleRequestWithResponse();
        var callOrder = new List<string>();
        var expectedResponse = new SampleResponse();

        var behavior1 = new Mock<IRequestPipelineBehavior<SampleResponse, SampleRequestWithResponse>>();
        behavior1
            .Setup(b => b.HandleAsync(request, It.IsAny<Func<Task<SampleResponse>>>()))
            .Returns(async (SampleRequestWithResponse _, Func<Task<SampleResponse>> next) =>
            {
                callOrder.Add("behavior1-before");
                var result = await next();
                callOrder.Add("behavior1-after");
                return result;
            });

        var behavior2 = new Mock<IRequestPipelineBehavior<SampleResponse, SampleRequestWithResponse>>();
        behavior2
            .Setup(b => b.HandleAsync(request, It.IsAny<Func<Task<SampleResponse>>>()))
            .Returns(async (SampleRequestWithResponse _, Func<Task<SampleResponse>> next) =>
            {
                callOrder.Add("behavior2-before");
                var result = await next();
                callOrder.Add("behavior2-after");
                return result;
            });

        var handlerMock = new Mock<IRequestHandler<SampleResponse, SampleRequestWithResponse>>();
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
    public async Task ExecuteWithPipeline_Response_ShortCircuits_ReturnsEarly()
    {
        // Arrange
        var request = new SampleRequestWithResponse();
        var expectedResponse = new SampleResponse();

        var behavior = new Mock<IRequestPipelineBehavior<SampleResponse, SampleRequestWithResponse>>();
        behavior
            .Setup(b => b.HandleAsync(request, It.IsAny<Func<Task<SampleResponse>>>()))
            .Returns((SampleRequestWithResponse _, Func<Task<SampleResponse>> next) => Task.FromResult(expectedResponse));

        var handlerMock = new Mock<IRequestHandler<SampleResponse, SampleRequestWithResponse>>();

        // Act
        var result = await BehaviorInvoker.ExecuteWithPipeline(
            request,
            handlerMock.Object,
            [behavior.Object]
        );

        // Assert
        Assert.Equal(expectedResponse, result);
        handlerMock.Verify(h => h.HandleAsync(It.IsAny<SampleRequestWithResponse>()), Times.Never);
    }

}
