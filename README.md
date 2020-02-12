# Service Mesh Examples for .NET

This repository has been consolidated to
[joatmon08/expense-report](https://github.com/joatmon08/expense-report).

Examples that use Consul Service Mesh for .NET services.

There are three services:

1. Report - for a given trip ID, return a list of expenses
1. Expense - create, read, update, and delete expenses
1. Database - tracks the expenses

Between each section, be sure to clean up everything before trying
the next section!

```shell
make clean
```

## Tracing

This example uses Consul Service Mesh to automatically propagate
span information when passing through proxies.

1. Start by creating the stack with Docker Compose.
   ```shell
   docker-compose up -d
   ```

1. Once it is up, you can go to the Consul and Jaeger UI to examine
   the services. You should see 3 services and 3 sidecars.
   ```shell
   open http://localhost:8500
   open http://localhost:16686
   ```

1. Make a request to the report service.
   ```shell
   curl -X GET http://localhost:5002/api/report/trip/d7fd4bf6-aeb9-45a0-b671-85dfc4d09544
   ```
   The request should return an empty report, with $0.00.

1. Go to Jaeger UI (localhost:16686) and you should see the trace.

## Traffic Management

To split traffic for a canary deployment or route traffic for an A/B test, you will need
to create version 2 of the expense service.

1. Start by creating the stack with Docker Compose.
   ```shell
   docker-compose up -d
   ```

1. Deploy version 2 by spinning up a second Docker Compose stack.
   ```shell
   docker-compose -f docker-compose-v2.yml up -d
   ```

1. Once it is up, you can go to the Consul and Jaeger UI to examine
   the services. You should see 3 services and 3 sidecars. The expense
   service should have 2 tags, `v1` and `v2`.
   ```shell
   open http://localhost:8500
   open http://localhost:16686
   ```

1. Apply the traffic management configurations.
   ```shell
   make traffic-config
   ```

1. Make requests to the report service. You should see half the traffic go to
   version 1 and the other half go to version 2.
   ```shell
   make traffic-test
   ```

## Circuit Breaker

We're going to put a circuit breaker from calls originating from report to the expense
service.

1. Start by creating the stack with Docker Compose.
   ```shell
   docker-compose -f docker-compose-circuit.yml up -d
   ```

1. You'll need to grab the Consul Cluster FQDN and populate it into
   the report service.
   ```shell
   make circuit-breaker
   ```

1. Then deploy the report service.
   ```shell
   docker-compose -f docker-compose-circuit-report.yml up -d
   ```

1. You can test it by deleting the database service container.

1. Then run an API call.
   ```shell
   make circuit-breaker-test
   ```
    
