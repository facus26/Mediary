﻿using System.Diagnostics;
using Mediary.Core;
using Mediary.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace Mediary.Pipeline.Behaviors;

/// <summary>
/// Pipeline behavior that measures and logs the execution time of the request handler.
/// Useful for performance monitoring and detecting slow requests.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
/// <typeparam name="TRequest">The type of the request being handled.</typeparam>
public sealed class PerformanceBehavior<TResponse, TRequest> : IRequestPipelineBehavior<TResponse, TRequest>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceBehavior<TResponse, TRequest>> _logger;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TResponse, TRequest>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Measures the elapsed time for request handling and logs a warning if it exceeds a threshold.
    /// </summary>
    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next)
    {
        var requestDescription = request.GetDescription();
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation("Handling {RequestName}{Description}",
                requestName,
                requestDescription is not null ? $" - {requestDescription}" : ""
            );

            var response = await next();

            stopwatch.Stop();

            _logger.LogInformation("✔ Handled {RequestName} in {ElapsedMilliseconds}ms",
                requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(ex, "✘ Error handling {RequestName} after {Elapsed}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}
