FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY BattleShip.sln BattleShip.sln
COPY BattleShip.Application/BattleShip.Application.csproj BattleShip.Application/BattleShip.Application.csproj
COPY BattleShip.Domain/BattleShip.Domain.csproj BattleShip.Domain/BattleShip.Domain.csproj
COPY BattleShip.Infrastructure/BattleShip.Infrastructure.csproj BattleShip.Infrastructure/BattleShip.Infrastructure.csproj
COPY BattleShip.WebApi/BattleShip.WebApi.csproj BattleShip.WebApi/BattleShip.WebApi.csproj
COPY BattleShip.WebApi/appsettings.json Rapid.Sterling.WebApi/appsettings.json
RUN dotnet restore

COPY . .

RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/publish .

RUN groupadd -g 10000 dotnet && useradd -u 10000 -g dotnet -m -d /app dotnet && chown -R dotnet:dotnet /app
USER dotnet:dotnet

ENV ASPNETCORE_URLS http://*:30000
EXPOSE 30000

ENTRYPOINT ["dotnet", "BattleShip.WebApi.dll"]
