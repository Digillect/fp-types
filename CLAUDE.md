# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a C# functional programming library called `Digillect.FP.Types` that provides `Result<T>` and `Option<T>` discriminated unions, along with utility types like `Unit` and `Error`. The library follows functional programming principles to avoid exception throwing and handle potential null reference exceptions through type-safe abstractions.

## Commands

### Build and Test
- Build: `dotnet build`
- Test: `dotnet test`
- Restore dependencies: `dotnet restore`
- Pack for NuGet: `dotnet pack -c Release -o artifacts`
- Build with the specific version: `dotnet build -c Release /p:Version="X.Y.Z"`

### Running Single Tests
The project uses the TUnit testing framework. To run a specific test:
- `dotnet test --filter "TestMethodName"`
- `dotnet test --filter "ClassName"`

## Architecture

### Core Types
- **`Result<T>`**: Represents either a successful value of type `T` or an `Error`. Supports monadic operations like `Map`, `Bind`, `Match`, and async variants.
- **`Option<T>`**: Represents an optional value that can be either `Some(T)` or `None`. Constraint: `T` must be non-nullable (`where T : notnull`).
- **`Error`**: Abstract base class for all error types. Includes `GenericError` for simple string messages and `Exceptional` for wrapping exceptions.
- **`Unit`**: Singleton type representing no meaningful value, used where `void` would be used in imperative code.

### Extension Methods
- **`ResultExtensions.cs`**: Extension methods for `Result<T>` including LINQ support and chaining operations.
- **`OptionExtensions.cs`**: Extension methods for `Option<T>` with functional operations.
- **`NewResultExtensions.cs`**: Recently added extensions for error handling patterns (`When`, `Else`).

### Functional Utilities
- **`Prelude.cs`**: Static utility class providing factory methods (`Success`, `Err`, `Some`, `Optional`) and exception handling wrappers (`WithExceptionHandling`).

### Code Patterns
- Heavy use of implicit operators for seamless type conversion
- Async variants for all major operations (`MapAsync`, `BindAsync`, `SwitchAsync`)
- Method chaining for fluent APIs
- Error-first design with early returns
- LINQ query syntax support through `Select` and `SelectMany`

## Development Guidelines

### Target Framework
- Library: `.NET Standard 2.1` for maximum compatibility
- Tests: `.NET 9.0` with TUnit framework

### Code Style
- Uses implicit usings and nullable reference types enabled
- Language version: default (latest stable)
- Follows C# naming conventions with proper XML documentation

### Testing Strategy
- Tests use the TUnit framework (not NUnit or xUnit)
- Global usings configured in `GlobalUsings.cs`
- Test reports generated in TRX format
- Test files follow the pattern: `*Tests.cs`

### CI/CD
- GitHub Actions workflow builds on Ubuntu
- Automatic versioning with GitVersion
- Publishes to MyGet (pre-release) and NuGet (release)
- Requires .NET 9.0 SDK
