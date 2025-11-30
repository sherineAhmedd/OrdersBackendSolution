# Task 5 — Short Answers

## 1. Redis vs SQL – key differences
...Data model:
Redis → in-memory key-value store.
SQL → relational tables with schemas and relationships.

Performance:
Redis → extremely fast (memory-based).
SQL → slower than Redis (disk-based), optimized for structured queries.

Persistence:
Redis → optional persistence.
SQL → fully persistent and ACID by default.

Use cases:
Redis → caching, sessions, rate-limiting.
SQL → long-term storage, relational queries, reporting.


Transactions & consistency:
Redis → simple transactions, no relational constraints.
SQL → strong consistency, complex joins, constraints, and transactions.


Scaling:
Redis → easy horizontal scaling from clustering.
SQL → scaling is harder.



## 2. When not to use caching?
...
– If data updates constantly, cache will become stale fast and offer no real benefit.
- Data is small , If the query is already very fast, caching adds unnecessary complexity.
- Memory is limited or expensive
- Sensitive data that shouldn't be stored in memory like Security-sensitive data.
- when You need strict real-time consistency like bank balances.

## 3. What if Redis is down?
...
If Redis goes down, my API should fall back to the database. We lose performance but not functionality. 
Redis is a cache, so no critical data is lost. I would log the issue, use retry/circuit-breaker patterns,
and let the application continue with degraded performance until Redis is back.

## 4. Optimistic vs pessimistic locking?
...
Pessimistic locking: lock the row immediately; others wait.
             Example:Two people edit the same order.
                     The first user locks it → second user must wait.
             Used when:High conflict scenarios , Banking and financial transactions.

Optimistic locking: no lock; check for conflict when saving using a version field like RowVersion or Timestamp.
             Used when:Low chance of conflict
                       High-read, low-write systems
                       Better performance, no blocking

## 5. Ways to scale a .NET backend?
...
Vertical scaling: upgrade server resources (CPU, RAM).

Horizontal scaling: run multiple API instances behind a load balancer.

Caching: use Redis to reduce database load and speed up reads.

Database scaling: read replicas, sharding, indexing, stored procedures, CQRS for separating read/write workloads.

Background processing: offload heavy tasks to workers or queues (Hangfire, Azure Queue, RabbitMQ).

Containerization: use Docker/Kubernetes for easier deployment and autoscaling.