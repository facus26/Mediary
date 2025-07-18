# 📦 Changelog — Mediary

## [v0.2.0] - 2025-07-17

### ✨ Features

* **Request Metadata (`[RequestInfo]`)**

  * Added optional `[RequestInfo(string description, params string[] tags)]` attribute for request types.
  * Enables descriptive logging, debugging, and tooling support.
  * Request metadata is accessible at runtime via new extensions:

    * `request.GetDescription()` and `request.GetTags()`
  * 📄 See [RequestInfoAttribute.cs](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/RequestInfoAttribute.cs)

### 🔧 Enhancements

* **LoggingBehavior**
  Now includes the request description (if available via `[RequestInfo]`) in logs for improved traceability.

* **PerformanceBehavior**
  Also logs the request description from `[RequestInfo]` to provide better profiling context.

## [v0.1.0] - 2025-07-15

### ✨ Features

- **Request Dispatching**
  - Added `IRequestDispatcher` with:
    - `DispatchAsync<TRequest>()` for fire-and-forget commands
    - `ExecuteAsync<TRequest, TResponse>()` for response-based requests
  - Added extension method for simplified syntax:  
    `ExecuteAsync<TResponse>(IRequest<TResponse>)`

- **Request & Handler Interfaces**
  - `IRequest` and `IRequest<TResponse>` as the base contracts
  - `IRequestHandler<TRequest>` and `IRequestHandler<TResponse, TRequest>` for handlers
  - Semantic aliases:
    - `ICommand`, `ICommand<TResponse>`
    - `IQuery`, `IQuery<TResponse>`

- **Pipeline Behaviors**
  - Support for middleware behaviors using:
    - `IRequestPipelineBehavior<TRequest>`
    - `IRequestPipelineBehavior<TResponse, TRequest>`
  - Behaviors are executed in reverse registration order for proper chaining

- **Built-in Utility Behaviors**
  - `LoggingBehavior<TResponse, TRequest>`: Logs request start, end, and errors using `ILogger`
  - `PerformanceBehavior<TResponse, TRequest>`: Measures and logs execution time in milliseconds

- **Dependency Injection Support**
  - `AddMediary()` extension to register dispatcher
  - `MediaryBuilder` fluent API to register:
    - Request handlers (individually or from assemblies)
    - Pipeline behaviors (individually or from assemblies)
    - Open generic behaviors globally using:
      `AddOpenPipelineBehaviors(typeof(LoggingBehavior<,>))`