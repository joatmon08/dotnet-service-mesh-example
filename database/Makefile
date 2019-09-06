build:
	docker build -t demoexpenses-db .

run:
	docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Testing!123' -p 1433:1433 -d --name demoexpenses-db demoexpenses-db
	
clean:
	docker rm -f demoexpenses-db