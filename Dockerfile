#Img config
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as build

WORKDIR /src

#Restoring

COPY cis-api-legacy-integration-phase-2/cis-api-legacy-integration-phase-2.csproj ./
RUN dotnet restore

#Building

COPY . /src
WORKDIR /src
run dotnet build cis-api-legacy-integration-phase-2/cis-api-legacy-integration-phase-2.csproj -c Release -o /app/build

#Publish
FROM build as publish
run dotnet publish cis-api-legacy-integration-phase-2/cis-api-legacy-integration-phase-2.csproj -c 

#Running
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "cis-api-legacy-integration-phase-2.dll"]
EXPOSE 5141