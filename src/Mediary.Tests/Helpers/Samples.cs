using Mediary.Core;
using Mediary.Core.Attributes;
using Mediary.Pipeline;

namespace Mediary.Tests.Helpers;

public class SampleResponse { }
public class SampleClosedRequest : IRequest<SampleResponse> { }

[RequestInfo("Sample request", "sample")]
public class SampleRequest : IRequest<SampleResponse> { }

[RequestInfo("Sample request")]
public class SampleRequestWithoutTags : IRequest<SampleResponse> { }
public class SampleRequestWithoutInfo : IRequest<SampleResponse> { }


public class SampleRequestHandler : IRequestHandler<SampleResponse, SampleRequest>
{
    public Task<SampleResponse> HandleAsync(SampleRequest request) =>
        Task.FromResult(new SampleResponse());
}

public class SampleOpenLoggingBehavior<TResponse, TRequest> : IRequestPipelineBehavior<TResponse, TRequest>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next) => next();
}

public class SampleLoggingBehavior : IRequestPipelineBehavior<SampleResponse, SampleRequest>
{
    public Task<SampleResponse> HandleAsync(SampleRequest request, Func<Task<SampleResponse>> next) => next();
}

public class ClosedLoggingBehavior : IRequestPipelineBehavior<SampleResponse, SampleClosedRequest>
{
    public Task<SampleResponse> HandleAsync(SampleClosedRequest request, Func<Task<SampleResponse>> next) => next();
}
