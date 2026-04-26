# CarRental — README

## Architecture overview

This solution is modular and service-oriented. It includes a Razor Pages web front-end (`Applications/CarRentalPlatform`) and multiple independent WebAPI services (for example, `Services/Maintenance.WebAPI`, `Services/VehicleInventory`). Projects are split to separate concerns and enable independent development, testing, and deployment:

- `Applications/*` — Presentation (Razor Pages, controllers, views).
- `Services/*` — Service APIs, application services, DTOs, controllers.
- `*.Infrastructure` — Persistence, EF Core DbContext, repositories, migrations.
- `*.Domain` — Core business entities, enums, value objects, and domain exceptions.

Services expose HTTP APIs (most include Swagger) and communicate with their own domain and infrastructure layers. Dependencies follow an inward direction: Presentation -> Application -> Domain. Infrastructure implements details and depends on Domain.

## Clean Architecture layers

The solution follows Clean Architecture principles and separates responsibilities into layers:

- Presentation
  - Handles HTTP, routing, model binding, and views. Located in `Applications/CarRentalPlatform`.
- Application
  - Contains use cases, application services, DTOs, and orchestration logic. Located in `*/*.Application` projects.
- Domain
  - Core business rules, entities, value objects, and domain exceptions. Located in `*/*.Domain` projects.
- Infrastructure
  - Persistence (EF Core), repositories, external integrations, and DI wiring. Located in `*/*.Infrastructure` projects.

Layers depend inward. Business rules and invariants belong to Domain; use cases in Application orchestrate them; Presentation handles HTTP concerns.

## Domain model and business rules

Key domain concepts (high level):

- Vehicle
  - Attributes: `Id`, `Make`, `Model`, `Type`, `Status` (Available, Rented, Maintenance, Retired), etc.
  - Rules: Valid status transitions (e.g., cannot mark `Available` without return processing). Moving to `Retired` is permanent.
- Customer
  - Attributes: contact details, profile, active rentals.
  - Rules: Required contact fields validated; rental creation enforces availability and single active rental per vehicle.
- RepairHistory / Maintenance
  - Attributes: vehicle id, date, description, cost.
  - Rules: Maintenance records are append-only; maintenance can change vehicle status to/from `Maintenance`.
- Rentals (workflow)
  - Rules: Check availability, create rental atomically, update vehicle status, enforce domain invariants.

Place validation and invariants in Domain entities or domain services; Application layer enforces orchestration and transactional boundaries.

## Run instructions

Prerequisites
- Visual Studio 2022 (or later) with .NET 8 SDK, or .NET 8 SDK and a terminal.

From Visual Studio
1. Open the solution.
2. Set startup project(s) (for example `Applications/CarRentalPlatform` and any WebAPI projects you need to run). Use Solution Properties to configure multiple startup projects.
3. Restore NuGet packages, build, and run. Check each project's `launchSettings.json` for ports.

From command line
1. At solution root: `dotnet restore && dotnet build`.
2. Run a project:
   - `cd Applications/CarRentalPlatform` && `dotnet run`
   - or `cd Services/Maintenance.WebAPI` && `dotnet run`
3. Open browser to the project's configured URL. Many WebAPI projects expose Swagger at `/swagger`.

Notes
- `Services/Maintenance.WebAPI` requires an API key header `X-Api-Key` with the development value `MY_SECRET_KEY_123` (see `Program.cs`). Swagger UI is enabled; use the Authorization control to provide the API key.

## Known limitations

- Several services use in-memory or fake implementations (for example, `FakeRepairHistoryService`) instead of persistent storage.
- API key authentication is hard-coded and unsuitable for production.
- Limited global authentication/authorization (no full identity/roles implemented).
- Minimal logging and observability; mainly console output in places.
- Some endpoints lack comprehensive validation and edge-case handling.
- Not all domain scenarios have unit tests yet.

