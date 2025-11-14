# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Application Overview

ZenCrm is a multi-layered CRM application built on the ABP Framework v9.3.6 with .NET 9.0. It follows Domain-Driven Design (DDD) principles with a modular architecture. The application supports multi-tenancy, uses Entity Framework Core with SQLite, and includes custom business modules for catalog, event flow, legal jurisdiction, and financial management.

## Development Commands

### First-time Setup
```bash
# Install ABP client-side libraries and initialize database
./etc/scripts/initialize-solution.ps1

# Or run manually:
abp install-libs
cd src/ZenCrm.DbMigrator && dotnet run && cd -
cd src/ZenCrm.Web && dotnet dev-certs https -v -ep openiddict.pfx -p config.auth_server_default_pass_phrase
```

### Running the Application
```bash
# Migrate database (if needed)
./etc/scripts/migrate-database.ps1

# Run the web application
cd src/ZenCrm.Web
dotnet run
# Application runs on https://localhost:44340
```

### Testing
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test test/ZenCrm.Domain.Tests/ZenCrm.Domain.Tests.csproj
dotnet test test/ZenCrm.Application.Tests/ZenCrm.Application.Tests.csproj
dotnet test test/ZenCrm.EntityFrameworkCore.Tests/ZenCrm.EntityFrameworkCore.Tests.csproj
dotnet test test/ZenCrm.Web.Tests/ZenCrm.Web.Tests.csproj
```

## Architecture

### Layer Structure
- **ZenCrm.Domain** - Business entities, domain services, repositories
- **ZenCrm.Application** - Application services, business use cases
- **ZenCrm.EntityFrameworkCore** - EF Core data access and migrations
- **ZenCrm.Web** - ASP.NET Core MVC/Razor Pages web application
- **ZenCrm.HttpApi** - REST API controllers
- **ZenCrm.DbMigrator** - Console app for database setup and seeding

### Custom Business Modules
Located in `modules/` directory, each follows ABP's module structure:
- **zencrm.catalog** - Product/service catalog management
- **zencrm.eventflow** - Event flow management system
- **zencrm.juris** - Legal and jurisdiction management
- **zencrm.finance** - Financial operations and management

### Database Configuration
- **Connection String**: `Data Source=../../ZenCrm.db;` (SQLite)
- **Multi-tenancy**: Enabled with tenant-scoped data isolation
- **Migrations**: Applied via `ZenCrm.DbMigrator` console application
- **Seeding**: Initial data populated by `ZenCrm.Domain.Seed` configuration

### Authentication & Security
- **OpenIddict**: OAuth2/OIDC server for authentication
- **Certificate**: Uses `openiddict.pfx` for token signing (password: `6060c88d-6ae3-49ea-897f-7df66bd2732d`)
- **Authority**: `https://localhost:44340`
- **Multi-tenant**: Tenant isolation in authentication and authorization

### Development Workflow
1. Run database migrations when schema changes: `./etc/scripts/migrate-database.ps1`
2. Install client-side dependencies after cloning: `abp install-libs`
3. Use `ZenCrm.TestBase` for common test setup and utilities
4. Follow ABP conventions for new modules and features
5. All modules should inherit from ABP's base module types

### Key Configuration Files
- `src/ZenCrm.Web/appsettings.json` - Main application configuration
- `src/ZenCrm.Web/Modules/` - ABP module configuration
- `common.props` - Common MSBuild properties for all projects
- `NuGet.Config` - NuGet package sources configuration

### ABP Framework Features Used
- Dependency Injection (Autofac)
- Permission Management (role-based)
- Audit Logging
- Background Jobs
- Caching
- Localization
- Feature Management
- Blob Storage
- LeptonXLite UI Theme