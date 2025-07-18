using Mediary.Core;
using Mediary.Core.Attributes;
using Mediary.Pipeline;

namespace Mediary.Tests.Helpers;

[RequestInfo("Sample request")]
public class SampleRequestOnlyWithDescription : IRequest { }
public class SampleRequestWithoutInfo : IRequest { }
public class SampleRequestWithResponseWithoutInfo : IRequest<SampleResponse> { }
public class SampleClosedRequest : IRequest { }

[RequestInfo("Sample request", "sample")]
public class SampleRequest : IRequest { }

[RequestInfo("Sample request with response", "sample")]
public class SampleRequestWithResponse : IRequest<SampleResponse> { }

public class SampleResponse { }

public class SampleRequestHandler : IRequestHandler<SampleRequest>
{
    public Task HandleAsync(SampleRequest request) => Task.CompletedTask;
}

public class SampleRequestWithResponseHandler : IRequestHandler<SampleResponse, SampleRequestWithResponse>
{
    public Task<SampleResponse> HandleAsync(SampleRequestWithResponse request) =>
        Task.FromResult(new SampleResponse());
}

public class SampleLoggingBehavior : IRequestPipelineBehavior<SampleRequest>
{
    public Task HandleAsync(SampleRequest request, Func<Task> next) => next();
}

public class SampleLoggingBehaviorWithResponse : IRequestPipelineBehavior<SampleResponse, SampleRequestWithResponse>
{
    public Task<SampleResponse> HandleAsync(SampleRequestWithResponse request, Func<Task<SampleResponse>> next) => next();
}

public class SampleOpenLoggingBehavior<TResponse, TRequest> : IRequestPipelineBehavior<TResponse, TRequest>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next) => next();
}

public class ClosedLoggingBehavior : IRequestPipelineBehavior<SampleClosedRequest>
{
    public Task HandleAsync(SampleClosedRequest request, Func<Task> next) => next();
}
