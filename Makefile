build:
	docker build -t joatmon08/expense-db:mssql database/
	docker build -t joatmon08/expense:dotnet expense/
	docker build -t joatmon08/report:dotnet -f report/Dockerfile .

push:
	docker push joatmon08/expense-db:mssql
	docker push joatmon08/expense:dotnet
	docker push joatmon08/report:dotnet

zipkin-run:
	docker run --rm -d -p 9411:9411 --name zipkin openzipkin/zipkin

db-run:
	docker run --rm -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Testing!123' -p 1433:1433 -d --name expenses-db joatmon08/expense-db:mssql

expense-run:
	(cd expense && dotnet run)

report-run:
	(cd report && dotnet run)

circuit-break:
	while true; do curl localhost:5002/api/report/trip/d7fd4bf6-aeb9-45a0-b671-85dfc4d09544; echo ""; sleep 1; done
	
traffic-shaping:
	while true; do curl localhost:5002/api/report/expense/version; echo ""; sleep 2; done

clean:
	docker-compose -f docker-compose-circuit.yml down || true
	docker-compose -f docker-compose-circuit.yml rm || true
