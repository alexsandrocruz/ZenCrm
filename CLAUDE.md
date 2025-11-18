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

## CRM Entity Development Guidelines

### When Creating New CRM Entities

#### 1. Domain Layer (Entity Design)
```csharp
// Always create entities with parameterized constructors for required fields
public class SalesOpportunity : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }
    public Guid SalesLeadId { get; private set; }
    public Guid OwnerUserId { get; private set; }
    public decimal EstimatedValue { get; private set; }
    public DateTime ExpectedCloseDate { get; private set; }

    // Constructor with REQUIRED parameters only
    public SalesOpportunity(
        Guid id,
        string name,
        Guid salesLeadId,
        Guid ownerUserId,
        decimal estimatedValue,
        DateTime expectedCloseDate) : base(id)
    {
        SetName(name);
        SalesLeadId = salesLeadId;
        OwnerUserId = ownerUserId;
        SetEstimatedValue(estimatedValue);
        ExpectedCloseDate = expectedCloseDate;
        // Initialize defaults
        Priority = Priority.Normal;
        Stage = PipelineStage.Qualification;
        IsActive = true;
    }

    // Use setter methods for business logic validation
    public void SetEstimatedValue(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Estimated value must be greater than 0");
        EstimatedValue = value;
    }
}
```

#### 2. Application Layer (AutoMapper Configuration)
```csharp
// ALWAYS add mappings to ZenCrmApplicationAutoMapperProfile.cs
public class ZenCrmApplicationAutoMapperProfile : Profile
{
    public ZenCrmApplicationAutoMapperProfile()
    {
        CreateMap<YourEntity, YourEntityDto>();
        CreateMap<CreateUpdateYourEntityDto, YourEntity>();
        // Add ALL entity mappings here
    }
}
```

#### 3. Application Service (Entity Creation Pattern)
```csharp
[Authorize(ZenCrmPermissions.YourEntity.Create)]
public async Task<YourEntityDto> CreateAsync(CreateUpdateYourEntityDto input)
{
    // NEVER use ObjectMapper.Map() for entities with parameterized constructors
    // ALWAYS construct manually using the entity's constructor
    var entity = new YourEntity(
        GuidGenerator.Create(),
        input.RequiredField1,
        input.RequiredField2,
        input.RequiredField3
    );

    // Use setter methods for optional properties with validation
    entity.SetOptionalProperty(input.OptionalField);
    entity.AssociateWithRelatedEntity(input.RelatedEntityId);

    await _repository.InsertAsync(entity);

    // Use ObjectMapper for Entity -> DTO mapping (this works)
    return ObjectMapper.Map<YourEntity, YourEntityDto>(entity);
}
```

#### 4. Frontend Angular Form Validation
```typescript
// Form validation with proper min/max values
buildForm(): void {
  this.form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(256)]],
    estimatedValue: [
      null, // Start with null instead of 0
      [Validators.required, Validators.min(0.01)] // Enforce > 0
    ],
    expectedCloseDate: [null, Validators.required]
  });
}

// Helper methods for validation feedback
isFieldInvalid(fieldName: string): boolean {
  const field = this.form.get(fieldName);
  return field ? field.invalid && (field.dirty || field.touched) : false;
}

getErrorMessage(fieldName: string): string {
  const field = this.form.get(fieldName);
  if (!field || !field.errors) return '';

  if (field.errors['required']) return 'This field is required.';
  if (field.errors['min']) {
    if (fieldName === 'estimatedValue') return 'Value must be greater than 0.';
    return `Minimum value is ${field.errors['min'].min}.`;
  }
  return 'Invalid value.';
}
```

#### 5. Frontend Template HTML
```html
<!-- Always include validation feedback -->
<input
  type="number"
  min="0.01"
  step="0.01"
  class="form-control"
  formControlName="estimatedValue"
  [class.is-invalid]="isFieldInvalid('estimatedValue')"
/>
@if (isFieldInvalid('estimatedValue')) {
  <div class="invalid-feedback d-block">
    {{ getErrorMessage('estimatedValue') }}
  </div>
}
```

#### 6. Frontend Payload Cleanup
```typescript
// Always remove undefined fields from payload
const payload = { /* ... form values ... */ };

Object.keys(payload).forEach(key => {
  if (payload[key] === undefined) {
    delete payload[key];
  }
});
```

### Critical Rules to Follow

1. **NEVER use ObjectMapper.Map()** to create entities with parameterized constructors
2. **ALWAYS add AutoMapper mappings** for every new entity in `ZenCrmApplicationAutoMapperProfile.cs`
3. **Frontend forms must validate** business rules (e.g., values > 0)
4. **Remove undefined fields** from API payloads to prevent serialization errors
5. **Use parameterized constructors** to enforce required fields and business logic
6. **Test both API and frontend** when creating new entities

### Common Pitfalls to Avoid

- ❌ `ObjectMapper.Map<Dto, Entity>(input)` - Fails with parameterized constructors
- ❌ Sending `undefined` values in API payloads
- ❌ Starting numeric fields with 0 when validation requires > 0
- ❌ Forgetting to add AutoMapper profiles for new entities
- ❌ Not providing visual validation feedback in forms

### Testing New Entities

1. Test API directly with curl/PostMan first
2. Test frontend form validation
3. Test end-to-end creation flow
4. Verify all CRUD operations work correctly