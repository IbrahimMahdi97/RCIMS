# Overview
This repository contains a .NET 9 based REST API.
The solution (`RCIMS.sln`) groups several projects:
```
RCIMS/                 – API host (Program.cs, middleware, migrations)
Presentation/          – API controllers
Service/               – Business logic implementations
Service.Interface/     – Service layer interfaces
Repository/            – Dapper repositories + raw SQL
Interfaces/            – Repository and logger interfaces
Entities/              – Domain models, enums, custom exceptions
Shared/                – DTOs, helper extensions, request parameter classes
LoggerService/         – NLog logger implementation
```
## API Host
`RCIMS/Program.cs` wires everything together. It sets up logging, DI, migrations, JWT auth, and registers controllers. The application uses a custom middleware to handle missing/expired tokens and an exception handler. Swagger is configured for API documentation.
```

## Layers
1. Entities
Holds POCO classes for database entities (e.g., `User`, `Contract`) and enumerations. It also defines custom exception types for error handling.

2. Shared
Contains Data Transfer Objects (DTOs), parameter objects for paging/filtering, and helper extensions (e.g., SHA-512 hashing and retrieving user ID from JWT claims).
```
public static class ClaimsPrincipleExtensions
{
    public static int RetrieveUserIdFromPrincipal(this ClaimsPrincipal user)
    {
        return Convert.ToInt32(
            user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
    ...
}
```
3. Interfaces
Defines repository interfaces (`IUserRepository`, `IContractRepository`, etc.) and other abstractions such as `ILoggerManager` or `IFileStorageService`.

4. Repository
Implements repositories using Dapper. SQL queries live in `Repository/Query/*Query.cs`. Example: `ContractRepository` retrieves posts, creates them inside transactions, and maps results to DTOs.

5. Service.Interface / Service
The service layer implements business logic. Each service depends on `IRepositoryManager` and optional helpers like `FileStorageService`. Example: `ContractService` create contract by admin.
```
public async Task<int> CreateByAdmin(ContractCreateByAdminDto contract, int userId)
    {
        var asset = await _repository.Asset.GetById(contract.AssetId) ?? throw new AssetNotFoundException();
        var plan = await _repository.Plan.GetById(contract.PlanId) ?? throw new PlanNotFoundException();
        var assetId = await _repository.Asset.GetIdByUUID(contract.AssetId);
        var planId = await _repository.Plan.GetIdByUUID(contract.PlanId);
        var customerId = await _repository.User.GetIdByUUID(contract.UserId);
        if (customerId <= 0) throw new UserNotFoundException(contract.UserId);
        var contractNumber = await GetLastContractNumber() + 1;
        int price = asset.ListPrice;
        int subtotal = price - contract.DownPayment;
        int discountamount = 0;
        decimal discount = 0;
        if (contract.Discount > 0)
        {
            discount = contract.Discount;
            discountamount = Convert.ToInt32((price - contract.DownPayment) * discount);
            subtotal = price - discountamount;
        }

        var contractCreate = new ContractForManipulationDto
        {
            ContractNumber = contractNumber.ToString(),
            ContractDate = DateTime.Now,
            Asset = assetId,
            Plan = planId,
            ContractStatus = 2,
            ContractFor = customerId,
            TotalAmount = price,
            DownPayment = contract.DownPayment,
            PaidAmount = contract.DownPayment,
            Subtotal = subtotal,
            GraceDays = plan.MaxAllow,
            Discount = discount,
            DiscountAmount = discountamount,
            Description = contract.Description
        };
        var result = await Create(contractCreate, userId);
        return result;
    }
```

`ServiceManager` and `RepositoryManager` lazily instantiate services and repositories to centralize dependency management.

6. Presentation
Houses ASP.NET controllers which are thin wrappers around the service layer. An example is `ContractsController`, providing CRUD operations with JWT authorization.

7. LoggerService
Implements `ILoggerManager` using NLog.

8. Migrations
Database schema is managed with FluentMigrator. `MigrationManager` runs migrations on startup. `Database.cs` creates the database if needed.

## Configuration and Helpers
- `nlog.config` configures file-based logging.
- `serviceAccountKey.json` stores Firebase credentials for sending push notifications via NotificationService.
- `global.json` pins the .NET SDK version (7.0).
- `launchSettings.json` contains local development profiles.

## What to Learn Next
1. Dapper basics – Understand how repositories build SQL queries and map them to DTOs.
2. FluentMigrator – Review migration classes in `RCIMS/Migrations` to learn the database schema and seeding process.
3. JWT authentication – Examine `UserService` for token creation/refresh logic and the custom middleware handling expired/missing tokens.
4. Dependency injection – Study `ServiceExtensions.cs` for how services, repositories, and logging are registered.
5. Controller to service flow – Observe how controllers call service methods and how responses are shaped using DTOs.
6. File storage utilities – See `FileStorageService` for saving/retrieving images.
7. Firebase push notifications – Explore `NotificationService` for sending notifications and retrieving unread counts.

Understanding these pieces will make it easier to contribute new endpoints, modify database queries, or adjust business rules within this codebase.
