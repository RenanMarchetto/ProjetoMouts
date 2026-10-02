# DeveloperStore Sales API

REST API developed as part of the Developer Evaluation challenge.

The project implements sales management using .NET, ASP.NET Core, Entity Framework Core, PostgreSQL and principles inspired by Domain-Driven Design (DDD) and Clean Architecture.

The API supports the complete lifecycle of a sale, including creation, retrieval, update, deletion and cancellation of sales and individual sale items.

---

## Technologies

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- FluentValidation
- Swagger / OpenAPI
- xUnit
- Entity Framework Core InMemory for integration tests
- Docker
- Docker Compose

---

## Architecture

The solution is divided into the following projects:

text
.
├── src/
│   ├── DeveloperStore.Api/
│   ├── DeveloperStore.Application/
│   ├── DeveloperStore.Domain/
│   └── DeveloperStore.Infrastructure/
│
├── tests/
│  ├── DeveloperStore.UnitTests/
│  └── DeveloperStore.IntegrationTests/
├── DeveloperStore.slnx
├── Dockerfile
├── docker-compose.yml
├── .dockerignore
├── .gitignore
└── README.md

### DeveloperStore.Domain

Contains the core business model and business rules.

Responsibilities include:

- Sale aggregate
- Sale items
- Discount calculation
- Quantity restrictions
- Sale and item cancellation rules
- Domain events
- Domain exceptions

The domain layer does not depend on infrastructure or ASP.NET Core.

### DeveloperStore.Application

Coordinates application use cases.

Responsibilities include:

- Sale services
- Requests and DTOs
- Validation
- Mapping between domain entities and API/application models
- Repository abstractions
- Domain event dispatching

### DeveloperStore.Infrastructure

Contains infrastructure-specific implementations.

Responsibilities include:

- Entity Framework Core
- PostgreSQL persistence
- DbContext
- Entity configurations
- Repository implementations
- Database migrations

### DeveloperStore.Api

Application entry point and HTTP interface.

Responsibilities include:

- REST controllers
- Dependency injection
- Swagger/OpenAPI
- Exception handling middleware
- HTTP request/response handling

---

## Domain Model

Sale is the aggregate root.

A sale controls its collection of SaleItem entities and protects the business invariants associated with the sale.

Simplified model:

text
Sale
 ├── Id
 ├── SaleNumber
 ├── SaleDate
 ├── CustomerId
 ├── CustomerName
 ├── BranchId
 ├── BranchName
 ├── IsCancelled
 ├── TotalAmount
 └── Items
      └── SaleItem
           ├── ProductId
           ├── ProductName
           ├── Quantity
           ├── UnitPrice
           ├── Discount
           ├── TotalAmount
           └── IsCancelled


Business logic is kept in the domain instead of controllers or persistence classes.

---

## Business Rules

Discounts are calculated according to the quantity of identical products in a sale.

| Quantity | Discount |
|---:|---:|
| 1 - 3 | 0% |
| 4 - 9 | 10% |
| 10 - 20 | 20% |
| More than 20 | Not allowed |

For example:

text
Product: Keyboard
Quantity: 10
Unit price: 100.00

Gross amount = 10 × 100.00 = 1000.00
Discount = 20%
Total = 800.00


Discounts and totals are calculated by the domain and are not trusted as values supplied by the API client.

### Quantity limit

A sale cannot contain more than 20 units of the same product.

Attempting to create or update an item with a quantity greater than 20 results in a domain validation error.

### Cancellation

Sales can be cancelled without being physically removed from the database.

Individual sale items can also be cancelled.

Cancellation and deletion therefore represent different operations:

- *Cancellation* preserves the sale and its historical information.
- *Deletion* removes the sale through the CRUD operation.

---

## External Identities

Customer, branch and product information belongs conceptually to external domains.

Instead of modelling complete Customer, Branch and Product aggregates inside the Sales domain, the sale stores their identifiers together with denormalized descriptions:

text
CustomerId + CustomerName
BranchId   + BranchName
ProductId  + ProductName


This follows the challenge requirement for external identities with denormalized entity descriptions.

It avoids coupling the Sales bounded context to external domain models while preserving the descriptive information associated with the sale.

---

## Domain Events

The application represents important changes in the sale lifecycle through domain events.

Examples include:

text
SaleCreated
SaleModified
SaleCancelled
ItemCancelled


A message broker is intentionally not required for this challenge.

Domain events are dispatched internally, allowing side effects such as logging while keeping the domain model decoupled from infrastructure concerns.

This design also makes it possible to replace the current handler with an external messaging mechanism in the future without moving business rules into the API layer.

---

## REST API

Main endpoints:

| Method | Endpoint | Description |
|---|---|---|
| POST | /api/sales | Create a sale |
| GET | /api/sales/{id} | Get a sale by id |
| GET | /api/sales | List sales |
| PUT | /api/sales/{id} | Update a sale |
| DELETE | /api/sales/{id} | Delete a sale |
| PATCH | /api/sales/{id}/cancel | Cancel a sale |
| PATCH | /api/sales/{saleId}/items/{itemId}/cancel | Cancel an item |

---

## Example - Create Sale

Request:

json
{
  "saleNumber": "SALE-001",
  "saleDate": "2026-10-01T12:00:00Z",
  "customerId": "0c0dbca5-8241-43b6-908b-d60ad14adf41",
  "customerName": "Customer Example",
  "branchId": "1279e642-dc69-46c1-8f1e-7e109d540a20",
  "branchName": "Main Branch",
  "items": [
    {
      "productId": "92de81bd-d850-4f31-9413-b43cb444653c",
      "productName": "Product Example",
      "quantity": 10,
      "unitPrice": 100.00
    }
  ]
}


For 10 units, the domain applies the 20% discount automatically.

The client does not need to calculate the discount or total amount.

---

## Running with Docker

### Requirements

- Docker
- Docker Compose

From the DeveloperStore directory:

bash
docker compose up --build


Docker Compose starts:

text
DeveloperStore API
        |
        v
PostgreSQL


The PostgreSQL container includes a health check and the API waits for the database service to become healthy.

The API is available at:

text
http://localhost:8081


Swagger:

text
http://localhost:8081/swagger


To stop the containers:

bash
docker compose down


To also remove the PostgreSQL volume:

bash
docker compose down -v


> Removing the volume deletes the local Docker database data.

---

## Database

The application uses PostgreSQL through Entity Framework Core.

The Docker environment creates the database using the configuration defined in docker-compose.yml.

EF Core migrations are applied when the application starts.

The database contains the sales and sale items required by the Sales domain.

---

## Running Locally

### Requirements

- .NET 8 SDK
- PostgreSQL

Configure the PostgreSQL connection string in the appropriate application settings or environment variable.

Restore dependencies:

bash
dotnet restore


Build the solution:

bash
dotnet build


Run the API:

bash
dotnet run --project src/DeveloperStore.Api


---

## Tests

The solution contains both unit and integration tests.

### Unit Tests

Unit tests focus primarily on domain behavior and business-rule boundaries.

Important scenarios include:

text
3 units  -> 0% discount
4 units  -> 10% discount
9 units  -> 10% discount
10 units -> 20% discount
20 units -> 20% discount
21 units -> rejected


They also cover sale behavior such as cancellation and item operations.

### Integration Tests

Integration tests exercise the API through HTTP and verify the interaction between:

text
HTTP request
    ↓
Controller
    ↓
Application
    ↓
Domain
    ↓
Repository / EF Core
    ↓
HTTP response


The integration test environment replaces the production PostgreSQL configuration with an isolated test database provider.

Run all tests with:

bash
dotnet test


---

## Error Handling

The API uses centralized exception handling middleware.

Domain and application exceptions are translated into appropriate HTTP responses instead of exposing internal implementation details.

Typical responses include:

text
200 OK
201 Created
204 No Content
400 Bad Request
404 Not Found
500 Internal Server Error


---

## Design Decisions

### Business rules belong to the domain

Discount and quantity rules are implemented in the domain model rather than in controllers or database code.

This ensures that the same rules are enforced regardless of how the domain is invoked.

### Calculated values are controlled by the application

Clients provide quantities and unit prices.

Discounts and totals are calculated internally, preventing clients from bypassing business rules by submitting their own calculated values.

### External entities are not duplicated as aggregates

Customer, branch and product belong to external domains.

Only their identifiers and required denormalized descriptions are stored in the Sales context.

### Cancellation is different from deletion

Cancellation represents a business operation and preserves historical information.

Deletion exists separately as part of the requested CRUD functionality.

### Infrastructure remains replaceable

The domain and application layers do not depend directly on PostgreSQL or ASP.NET Core infrastructure.

This keeps business rules isolated from technical implementation details.

---

## Assumptions

The explicit discount tiers defined by the challenge were interpreted as:

- fewer than 4 items: no discount;
- 4 through 9 identical items: 10%;
- 10 through 20 identical items: 20%;
- more than 20 identical items: not allowed.

Therefore, a quantity of exactly *4* receives the 10% discount.

Customer, branch and product information is considered to originate from external domains. Their identifiers and descriptions are received by the Sales API for the purposes of this challenge.

---

## API Documentation

When the application is running, the complete interactive API documentation is available through Swagger:

text
http://localhost:8081/swagger


Swagger can also be used to manually execute and inspect the available endpoints.