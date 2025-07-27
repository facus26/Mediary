# 📦 Changelog — Mediary

## [v0.3.0] - 2025-07-27

### ⚠️ Breaking Changes

* ❌ **Removed `IRequest` (non-generic)**

  * Eliminated support for `IRequest`, `IRequestHandler<TRequest>`, and `IRequestPipelineBehavior<TRequest>`.
  * All requests must now implement `IRequest<TResponse>`.
  * Pipeline and handler contracts were updated to support only the generic form.

* 🧼 **Removed legacy extensions**

  * Extensions like `ExecuteAsync<TRequest>()` were removed along with support for non-generic requests.

* 🧹 **Simplified `IRequestDispatcher`**

  * Now only contains:

    ```csharp
    Task<TResponse> DispatchAsync<TResponse, TRequest>(TRequest request);
    ```
  * The previous `ExecuteAsync` overloads were removed to streamline the interface.

---

### ✨ New Features

* **Built-in Semantic Result Types** (under `Mediary.Core.Results`)

  * Introduced lightweight `readonly struct`s for non-data responses:

    * [`Unit`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Unit.cs)
    * [`Success`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Success.cs)
    * [`Created`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Created.cs)
    * [`Updated`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Updated.cs)
    * [`Deleted`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Deleted.cs)
  * These types can be used as `TResponse` for commands that don’t return data, improving semantic clarity (e.g. `IRequest<Deleted>`).

* **Static `Result` helper**

  * Added `Result.Unit`, `Result.Created`, etc. for more fluent return values.

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