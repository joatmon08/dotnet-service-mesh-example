db:
	docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Testing!123' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-CU8-ubuntu
