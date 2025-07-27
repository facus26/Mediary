# Mediary — Lightweight Request Dispatcher for .NET

**Mediary** is a minimal, open-source request/response dispatcher for .NET —  
inspired by MediatR, but built from scratch with **no external dependencies**.

Clean request handling, extensible pipeline behaviors, and a DI-friendly architecture — all with zero external dependencies.

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

## 🧭 Table of Contents

- [Installation](#-installation)
- [Features](#-features)
- [Usage](#-usage)
- [Dependency Injection](#-dependency-injection)
  - [Option 1 — Auto-registration (Recommended)](#-option-1--auto-registration-recommended)
  - [Option 2 — Semi-manual (Builder API)](#-option-2--semi-manual-builder-api)
  - [Option 3 — Fully manual](#-option-3--fully-manual)
- [Built-in Behaviors](#-built-in-behaviors)
  - [Register pipeline behaviors globally](#-register-pipeline-behaviors-globally)
- [Handling Commands Without Return Values](#-handling-commands-without-return-values)
    - [Built-in Result Types](#-built-in-result-types)
- [Core Interfaces](#-core-interfaces)
- [Aliases](#-aliases) 
- [Request Metadata](#-request-metadata)

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

* ✅ Async request handling via [`IRequest<TResponse>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/IRequest.cs)
* ✅ Handler support via [`IRequestHandler<TResponse, TRequest>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/IRequestHandler.cs)
* ✅ Semantic result types: [`Unit`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Unit.cs), [`Success`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Success.cs), [`Created`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Created.cs), [`Updated`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Updated.cs), [`Deleted`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Deleted.cs)
* ✅ Dispatcher support via [`IRequestDispatcher`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Dispatcher/IRequestDispatcher.cs)
* ✅ Middleware support [`IRequestPipelineBehavior<TResponse, TRequest>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Pipeline/IRequestPipelineBehavior.cs)
* ✅ Generic and specific pipeline registration
* ✅ Optional [`[RequestInfo]`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Attributes/RequestInfoAttribute.cs) metadata for descriptive logging and tooling
* ✅ Aliases for semantic intent: [`ICommand<TResponse>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/ICommand.cs), [`IQuery<TResponse>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/IQuery.cs)

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
        _dispatcher.DispatchAsync<GetAllPlansQuery, List<PlanDto>>(new GetAllPlansQuery());
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

* Register the [`IRequestDispatcher`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Dispatcher/IRequestDispatcher.cs)
* Scan and register all [`IRequestHandler<,>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/IRequestHandler.cs)
* Scan and register all [`IRequestPipelineBehavior<,>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Pipeline/IRequestPipelineBehavior.cs)

### 🔧 Option 2 — Semi-manual (Builder API)

If you prefer full control but want to use the builder pattern:

```csharp
services.AddMediary()
    .AddRequestHandler<List<PlanDto>, GetAllPlansQuery, GetAllPlansHandler>()
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

| Interface | Description |
|----------|-------------|
| [`LoggingBehavior<TResponse, TRequest>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Pipeline/Behaviors/LoggingBehavior.cs) | Logs the start and end of a request using `ILogger`. |
| [`PerformanceBehavior<TResponse, TRequest>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Pipeline/Behaviors/PerformanceBehavior.cs) | Measures and logs execution time of each request. |

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

## ✅ Core Interfaces

| Interface | Description |
|----------|-------------|
| [`IRequest<TResponse>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/IRequest.cs) | Base request contract used for dispatching |
| [`IRequestHandler<TResponse, TRequest>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/IRequestHandler.cs) | Handles a specific request and returns a response |
| [`IRequestPipelineBehavior<TResponse, TRequest>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Pipeline/IRequestPipelineBehavior.cs) | Defines pipeline logic before/after the handler |
| [`IRequestDispatcher`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Dispatcher/IRequestDispatcher.cs) | Responsible for dispatching requests through the pipeline |

---

## ⚙️ Handling Commands Without Return Values

Mediary uses a single interface: [`IRequest<TResponse>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/IRequest.cs).  
When you don’t need to return data from a command, you can use semantic result types to indicate intent:

### ✅ Built-in Result Types

| Type | Purpose |
|------|---------|
| [`Unit`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Unit.cs) | Used when no response is needed (replaces `void`) |
| [`Success`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Success.cs) | Indicates a generic successful result |
| [`Created`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Created.cs) | Indicates a resource was created |
| [`Updated`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Updated.cs) | Indicates a resource was updated |
| [`Deleted`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results/Deleted.cs) | Indicates a resource was deleted |

These types are lightweight `readonly struct`s defined in [`Mediary.Core.Results`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Results).

#### Example: Command with no output
```csharp
public class ClearCacheCommand : IRequest<Unit> { }

public class ClearCacheHandler : IRequestHandler<Unit, ClearCacheCommand>
{
    public Task<Unit> HandleAsync(ClearCacheCommand request) =>
        Task.FromResult(Unit.Value);
}
```

#### Example: Command with semantic return

```csharp
public class CreateUserCommand : IRequest<Created> { }

public class CreateUserHandler : IRequestHandler<Created, CreateUserCommand>
{
    public Task<Created> HandleAsync(CreateUserCommand request) =>
        Task.FromResult(Created.Value);
}
```

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

👉 See [`RequestInfoAttribute`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/Attributes/RequestInfoAttribute.cs) in the source.

---

## 🪪 Aliases

For semantic clarity, Mediary exposes aliases for intent-based request types:

| Alias                 | Inherits              | Purpose                           |
| --------------------- | --------------------- | --------------------------------- |
| [`IQuery<TResponse>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/IQuery.cs) | [`IRequest<TResponse>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/IRequest.cs) | Read-only request                 |
| [`ICommand<TResponse>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/ICommand.cs) | [`IRequest<TResponse>`](https://github.com/facus26/Mediary/blob/main/src/Mediary/Core/IRequest.cs) | Write command                     |

Aliases are optional and meant to improve readability:

```csharp
// Represents: GET /users/{id}
public class GetUserByIdQuery : IQuery<UserDto> { }

// Represents: POST /users
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
