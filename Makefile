build:
	docker build -t joatmon08/expense-db:mssql database/
	docker build -t joatmon08/expense:dotnet expense/
	docker build -t joatmon08/report:dotnet report/

jaeger-run:
	docker run --rm -d -p 6831:6831/udp -p 6832:6832/udp -p 16686:16686 jaegertracing/all-in-one:latest --log-level=debug

db-run:
	docker run --rm -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Testing!123' -p 1433:1433 -d --name expenses-db joatmon08/expense-db:mssql

expense-run:
	(cd expense && dotnet run)

report-run:
	(cd report && dotnet run)

clean:
	docker rm -f expenses-db
