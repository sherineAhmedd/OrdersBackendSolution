# OrdersBackendSolution
# Orders API (Anyware Technical Task)

## Overview
This is a simple Orders API implementing the following endpoints:

- **POST /orders** — create a new order.
- **GET /orders/{id}** — fetch a single order (uses Redis cache, TTL 5 minutes).
- **GET /orders** — list all orders.
- **DELETE /orders/{id}** — delete an order from both DB and Redis cache.

### Order Model
- `OrderId` (Guid)
- `CustomerName` (string)
- `Product` (string)
- `Amount` (decimal)
- `CreatedAt` (DateTime)

---

## Repo Structure
OrdersBackendSolution/
├── Orders.API/
├── Orders.BLL/
├── Orders.DAL/
├── README.md
├── Task5.md
├── docker-compose.yml


---

## Prerequisites
- .NET 10 SDK 
- SQL Server (LocalDB or full version)
- Redis Server (via Docker)
You can run Redis using Docker with the following command:

      docker run --name redis -p 6379:6379 -d redis

---

## Configuration
- Update connection strings and Redis settings in `appsettings.Development.json` or environment variables.
- **Do not commit secrets**.

---

## Run Locally
1. Apply EF Core migrations: add-migration <message> , update-database

2.Add running the API and default URL:
   Default URL used in examples: http://localhost:5227 to get swagger

# Create an order
curl -X POST http://localhost:5227/orders \
-H "Content-Type: application/json" \
-d '{"customerName":"Jane","product":"Book","amount":12.5}'

# Get order by ID
curl http://localhost:5227/orders/{orderId}

# Delete order
curl -X DELETE http://localhost:5227/orders/{orderId}

## Task 5 – Theoretical Answers
See (./Task5.md) for short answers on Redis vs SQL, caching, locking, and scaling.

## Notes
- Dependency injection, async/await, logging, and error handling are implemented according to task requirements.
- Redis is optional for performance; DB is the source of truth.

        



