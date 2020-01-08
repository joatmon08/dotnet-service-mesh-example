CONSUL_DOMAIN := $(shell curl -s http://localhost:8500/v1/connect/ca/roots | jq -r .TrustDomain)

build:
	docker build -t joatmon08/expense-db:mssql database/
	docker build -t joatmon08/expense:dotnet expense/
	docker build -t joatmon08/report:dotnet -f report/Dockerfile .

push:
	docker push joatmon08/expense-db:mssql
	docker push joatmon08/expense:dotnet
	docker push joatmon08/report:dotnet

tracing-run:
	docker run --rm -d -p 9411:9411 -p 16686:16686 -e 'COLLECTOR_ZIPKIN_HTTP_PORT=9411' --name tracing jaegertracing/all-in-one:1.13

db-run:
	docker run --rm -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Testing!123' -p 1433:1433 -d --name expenses-db joatmon08/expense-db:mssql

expense-run:
	(cd expense && dotnet run)

report-run:
	(cd report && dotnet run)

circuit-break:
	sed 's/CONSUL_FQDN/${CONSUL_DOMAIN}/g' circuit_breaking/template.hcl > circuit_breaking/consul_config/report.hcl

circuit-break-test:
	while true; do curl localhost:5002/api/report/trip/d7fd4bf6-aeb9-45a0-b671-85dfc4d09544; echo ""; sleep 1; done
	
traffic-test:
	while true; do curl localhost:5002/api/report/expense/version; echo ""; sleep 2; done

traffic-config:
  CONSUL_HTTP_ADDR=http://localhost:8500 consul config write traffic_config/expense_service_resolver.hcl
  CONSUL_HTTP_ADDR=http://localhost:8500 consul config write traffic_config/expense_service_router.hcl
  CONSUL_HTTP_ADDR=http://localhost:8500 consul config write traffic_config/expense_service_splitter.hcl

report-test:
	CONSUL_HTTP_ADDR=http://localhost:8500 consul config write traffic_config/report_service_resolver.hcl
	CONSUL_HTTP_ADDR=http://localhost:8500 consul config write traffic_config/report_service_router.hcl

toggle-on:
	CONSUL_HTTP_ADDR=http://localhost:8500 consul kv put toggles/enable-number-of-items true

toggle-off:
	CONSUL_HTTP_ADDR=http://localhost:8500 consul kv put toggles/enable-number-of-items false

toggle-datacenter:
	CONSUL_HTTP_ADDR=http://localhost:8500 consul kv put toggles/datacenters dc2

clean:
	docker-compose -f docker-compose-circuit-report.yml down || true
	docker-compose -f docker-compose-circuit.yml down || true
	docker-compose -f docker-compose-v2.yml down || true
	docker-compose -f docker-compose.yml down || true