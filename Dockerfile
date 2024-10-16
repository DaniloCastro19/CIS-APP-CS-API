#Img config
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

#Restoring
WORKDIR /src
COPY cis-api-legacy-integration-phase-2.csproj ./ 
RUN dotnet restore

#Building
COPY . ./
RUN dotnet build cis-api-legacy-integration-phase-2.csproj -c Release -o /app/build

#Publish
FROM build as publish
run dotnet publish 'cis-api-legacy-integration-phase-2.csproj' -c Release -o /app/publish

#Running
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "cis-api-legacy-integration-phase-2.dll"]
