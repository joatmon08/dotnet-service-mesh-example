FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

RUN mkdir expense report

# Copy csproj and restore as distinct layers
COPY expense/*.csproj expense/.
COPY report/*.csproj report/.
RUN cd /app/report && dotnet restore

# Copy everything else and build
WORKDIR /app
COPY expense/ expense/
COPY report/ report/

WORKDIR /app/report
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/report/out .
EXPOSE 5002
ENTRYPOINT ["dotnet", "report.dll"]