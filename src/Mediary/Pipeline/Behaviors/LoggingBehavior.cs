using Mediary.Core;
using Microsoft.Extensions.Logging;

namespace Mediary.Pipeline.Behaviors;

/// <summary>
/// Pipeline behavior that logs the start and end of request handling,
/// along with the execution duration and any exceptions thrown.
/// Useful for monitoring and troubleshooting.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
/// <typeparam name="TRequest">The type of the request being handled.</typeparam>
public sealed class LoggingBehavior<TResponse, TRequest> : IRequestPipelineBehavior<TResponse, TRequest>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TResponse, TRequest>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TResponse, TRequest>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logs before and after the request handling, including execution time and exceptions.
    /// </summary>
    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling request: {RequestName}", requestName);

        try
        {
            var response = await next();

            _logger.LogInformation("Successfully handled request: {RequestName}", requestName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while handling request: {RequestName}", requestName);
            throw;
        }
    }
}
