# Mediary — Lightweight Request Dispatcher for .NET

**Mediary** is a minimal, open-source request/response dispatcher for .NET —  
inspired by MediatR, but built from scratch with **no external dependencies**.

It provides clean request handling, extensible pipeline behaviors, and a flexible DI-friendly architecture — all without relying on third-party libraries.

[![Build](https://github.com/facus26/Mediary/actions/workflows/build-test-coverage.yml/badge.svg)](https://github.com/facus26/Mediary/actions/workflows/build-test-coverage.yml)
[![codecov](https://codecov.io/gh/facus26/Mediary/branch/main/graph/badge.svg)](https://codecov.io/gh/facus26/Mediary)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue?logo=dotnet)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/Mediary.svg)](https://www.nuget.org/packages/Mediary)
---

## 🚨 Why Mediary?

**Mediary** is a lightweight request dispatcher for .NET designed to offer a clean, extensible, and dependency-free alternative to more complex mediators.

It focuses on **performance**, **clarity**, and **developer control**, while maintaining compatibility with the .NET dependency injection ecosystem.

### ✅ Key benefits

- ⚡ **Lightweight and fast** — no unnecessary overhead or runtime reflection
- 🧩 **Extensible pipeline behaviors** — clean middleware-style request handling
- 🧼 **Minimalist design** — no external dependencies, no magic
- 🧪 **Test-friendly** — everything is composable and DI-compatible
- 📦 **NuGet-ready** — simple to install and integrate

---

## 📦 Installation

You can install **Mediary** via NuGet:

```bash
dotnet add package Mediary
```

Or via the NuGet UI in Visual Studio by searching for **Mediary**.

📦 Available at: [https://www.nuget.org/packages/Mediary](https://www.nuget.org/packages/Mediary)

---

## 🚀 Features

* ✅ Async request handling via `IRequest<TResponse>` / `IRequest`
* ✅ Built-in dispatcher (`IRequestDispatcher`)
* ✅ Middleware support (`IRequestPipelineBehavior`)
* ✅ Generic and specific pipeline registration
* ✅ Optional `[RequestInfo]` metadata for descriptive logging and tooling


---

## 🔧 Usage

### 1. Define a Request

```csharp
public class GetAllPlansQuery : IQuery<List<PlanDto>> { }
```

### 2. Create a Handler

```csharp
public class GetAllPlansHandler : IRequestHandler<List<PlanDto>, GetAllPlansQuery>
{
    public Task<List<PlanDto>> HandleAsync(GetAllPlansQuery request)
    {
        // Your logic here
    }
}
```

### 3. Inject the Dispatcher

```csharp
public class PlanService
{
    private readonly IRequestDispatcher _dispatcher;

    public PlanService(IRequestDispatcher dispatcher) =>
        _dispatcher = dispatcher;

    public Task<List<PlanDto>> GetPlansAsync() =>
        _dispatcher.ExecuteAsync<GetAllPlansQuery, List<PlanDto>>(new GetAllPlansQuery());
}
```

---

## 🛠 Dependency Injection

Mediary supports flexible registration depending on your needs:


### ✅ Option 1 — Auto-registration (Recommended)

```csharp
services.AddMediary()
    .AddRequestHandlersFromAssembly(typeof(Program).Assembly)
    .AddPipelineBehaviorsFromAssembly(typeof(Program).Assembly);
```

This will:

* Register the `IRequestDispatcher`
* Scan and register all `IRequestHandler<>` and `IRequestHandler<,>`
* Scan and register all `IRequestPipelineBehavior<>` and `IRequestPipelineBehavior<,>`

### 🔧 Option 2 — Semi-manual (Builder API)

If you prefer full control but want to use the builder pattern:

```csharp
services.AddMediary()
    .AddRequestHandler<GetAllPlansQuery, GetAllPlansHandler>()
    .AddRequestHandler<List<PlanDto>, GetAllPlansQuery, GetAllPlansHandler>()
    .AddPipelineBehaviors<GetAllPlansQuery, LoggingBehavior<GetAllPlansQuery>>()
    .AddPipelineBehaviors<List<PlanDto>, GetAllPlansQuery, LoggingBehavior<List<PlanDto>, GetAllPlansQuery>>();
```

You can also override the dispatcher:

```csharp
services.AddMediary<CustomDispatcher>();
```

### 🧩 Option 3 — Fully manual

If you want **zero coupling** to Mediary’s builder or extension methods, you can register everything manually:

```csharp
services.AddScoped<IRequestDispatcher, RequestDispatcher>();
services.AddScoped<IRequestHandler<List<PlanDto>, GetAllPlansQuery>, GetAllPlansHandler>();
services.AddScoped<IRequestPipelineBehavior<List<PlanDto>, GetAllPlansQuery>, LoggingBehavior<List<PlanDto>, GetAllPlansQuery>>();
```

This gives you **maximum flexibility** and full control over dependency injection.

---

## 🔍 Built-in Behaviors

- [`LoggingBehavior<TResponse, TRequest>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Pipeline/Behaviors/LoggingBehavior.cs)  
    Logs the start and end of a request using `ILogger`.
- [`PerformanceBehavior<TResponse, TRequest>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Pipeline/Behaviors/PerformanceBehavior.cs)  
    Measures and logs execution time of each request.

### ✅ Register pipeline behaviors globally

You can register the logging and performance behaviors globally in two ways:

#### 1. Using the MediaryBuilder

```csharp
services.AddMediary()
    .AddOpenPipelineBehaviors(typeof(LoggingBehavior<,>))
    .AddOpenPipelineBehaviors(typeof(PerformanceBehavior<,>));
```

#### 2. Manual registration

```csharp
services.AddScoped(typeof(IRequestPipelineBehavior<,>), typeof(LoggingBehavior<,>));
services.AddScoped(typeof(IRequestPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
```

This will ensure that both behaviors are applied to all requests automatically.

---

## 🔖 Request Metadata

You can optionally annotate your request types with descriptive metadata using the `[RequestInfo]` attribute:

```csharp
[RequestInfo("Creates a new user", "Command", "Users")]
public class CreateUserCommand : ICommand<Guid> { }
```

This helps document the purpose of the request and can be consumed by logging, debugging, or inspection tools.

Both built-in behaviors (`LoggingBehavior` and `PerformanceBehavior`) will use this description if available.

This metadata is also accessible from within handlers or behaviors via extension methods.

👉 See [`RequestInfoAttribute`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/RequestInfoAttribute.cs) in the source.

---

## ✅ Core Interfaces

```csharp
public interface IRequest { }

public interface IRequest<TResponse> { }

public interface IRequestHandler<TRequest>
    where TRequest : IRequest
{
    Task HandleAsync(TRequest request);
}

public interface IRequestHandler<TResponse, TRequest>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request);
}

public interface IRequestPipelineBehavior<TRequest>
    where TRequest : IRequest
{
    Task HandleAsync(TRequest request, Func<Task> next);
}

public interface IRequestPipelineBehavior<TResponse, TRequest>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next);
}

public interface IRequestDispatcher
{
    Task DispatchAsync<TRequest>(TRequest request)
        where TRequest : IRequest;

    Task<TResponse> ExecuteAsync<TResponse, TRequest>(TRequest request)
        where TRequest : IRequest<TResponse>;
}
```

---

## 🪪 Aliases

For semantic clarity, Mediary exposes aliases for intent-based request types:

| Alias                 | Inherits              | Purpose                           |
| --------------------- | --------------------- | --------------------------------- |
| `IQuery`              | `IRequest`            | Read-only request, no response    |
| `IQuery<TResponse>`   | `IRequest<TResponse>` | Read-only request with a response |
| `ICommand`            | `IRequest`            | Write command, no response        |
| `ICommand<TResponse>` | `IRequest<TResponse>` | Write command with a response     |

Aliases are optional and meant to improve readability:

```csharp
public class GetUserByIdQuery : IQuery<UserDto> { }
public class CreateUserCommand : ICommand<UserDto> { }
```

---

## 📄 License

**MIT License** — Free for personal and commercial use.

---

## 🙌 Contributing

Open to feedback, ideas, PRs and improvements — feel free to fork and collaborate.

---

## 👨‍💻 Author

Built with ❤️ by **Facundo Juarez** — [GitHub](https://github.com/facus26)
