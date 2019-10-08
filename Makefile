build:
	docker build -t joatmon08/expense-db:mssql database/
	docker build -t joatmon08/expense:dotnet expense/
	docker build -t joatmon08/report:dotnet -f report/Dockerfile .

push:
	docker push joatmon08/expense-db:mssql
	docker push joatmon08/expense:dotnet
	docker push joatmon08/report:dotnet

jaeger-run:
	docker run --rm -d -e 'COLLECTOR_ZIPKIN_HTTP_PORT=9411' -p 14268:14268 -p 9411:9411 -p 6831:6831/udp -p 6832:6832/udp -p 16686:16686 --name jaeger jaegertracing/all-in-one:latest --log-level=debug

db-run:
	docker run --rm -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Testing!123' -p 1433:1433 -d --name expenses-db joatmon08/expense-db:mssql

expense-run:
	(cd expense && dotnet run)

report-run:
	(cd report && dotnet run)

circuit-break:
	ab -n 1000 -c 5 http://0.0.0.0:5001/api/expense
	
traffic-shaping:
	while true; do curl localhost:5002/api/report/expense/version; echo ""; sleep 2; done

clean:
	docker rm -f expenses-db
	docker rm -f jaeger
