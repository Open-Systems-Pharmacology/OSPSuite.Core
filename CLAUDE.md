# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

OSPSuite.Core is the core library for the Open Systems Pharmacology Suite - a C#/.NET framework for model-based systems pharmacology providing domain modeling, simulation, and analysis capabilities.

## Build Commands

```bash
# Restore and build
dotnet restore
dotnet build OSPSuite.Core.sln

# Run all tests
dotnet test

# Run specific test project
dotnet test tests/OSPSuite.Core.Tests
dotnet test tests/OSPSuite.Core.IntegrationTests

# Run tests with filter
dotnet test tests/OSPSuite.Core.Tests --filter "FullyQualifiedName~ClassName"

# Code coverage (requires Ruby)
rake cover

# Create local NuGet packages
rake create_local_nuget

# Update local MoBi/PK-Sim with new packages
rake create_local_nuget[m]   # update MoBi
rake create_local_nuget[p]   # update PK-Sim
```

## Architecture

**Layered architecture with clear separation:**

- **OSPSuite.Core** - Domain model, services, serialization (netstandard2.0)
- **OSPSuite.Infrastructure** - Cross-cutting concerns: logging, IO, serialization implementations
- **OSPSuite.Presentation** - MVP pattern presenters, DTOs, view interfaces (DevExpress)
- **OSPSuite.UI** - WinForms view implementations
- **OSPSuite.R** - R language integration for scripting support

**Key patterns:**
- MVP (Model-View-Presenter) in Presentation/UI layers
- Command pattern for undo/redo via `Commands` namespace
- Constructor injection with IoC container (Castle Windsor or Autofac)
- Rich domain model with `IEntity`/`IContainer` hierarchy

## Testing Conventions

Uses BDD-style testing with OSPSuite.BDDHelper:

```csharp
public abstract class concern_for_PopulationRunner : ContextForIntegration<IPopulationRunner>
{
    public override void GlobalContext()
    {
        base.GlobalContext();
        // Setup code
    }
}

public class When_running_population : concern_for_PopulationRunner
{
    [Observation]
    public void should_return_expected_results()
    {
        _results.Count.ShouldBeEqualTo(2);
    }
}
```

- Test classes inherit from `concern_for_<SUT>` or `ContextForIntegration<T>`
- Use `[Observation]` attribute instead of `[Test]`
- Assertions use `.ShouldBeEqualTo()`, `.ShouldBeTrue()`, etc.
- Mocking with FakeItEasy

## Coding Standards

**Naming:**
- Private fields: `_camelCase` (underscore prefix)
- Constants: `ALL_CAPS`
- Private methods: `camelCase`
- Public methods/properties: `PascalCase`
- Interfaces: `I` prefix

**Style:**
- 3 spaces indentation (no tabs)
- Always use braces for loops/conditionals
- No Hungarian notation
- English comments only
- Use constants for magic numbers/strings
- Return `IReadOnlyList`/`IReadOnlyCollection` from public APIs

## Git Workflow

- Default branch: `develop`
- Create feature branches off `develop`
- Reference issues: `Fixes #<issue-number>`
- Submodules: `src/OSPSuite.Dimensions`, `src/OSPSuite.PKParameters`

## External Documentation

- [Coding Standards](https://github.com/Open-Systems-Pharmacology/developer-docs/blob/main/setup/coding_standards.md)
- [Contributing Guide](https://github.com/Open-Systems-Pharmacology/Suite/blob/master/CONTRIBUTING.md)
