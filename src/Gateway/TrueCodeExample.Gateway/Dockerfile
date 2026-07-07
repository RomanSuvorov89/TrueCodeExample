FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/TrueCodeExample.Common/TrueCodeExample.Common.csproj", "src/TrueCodeExample.Common/"]
COPY ["src/Gateway/TrueCodeExample.Gateway/TrueCodeExample.Gateway.csproj", "src/Gateway/TrueCodeExample.Gateway/"]

RUN dotnet restore "src/Gateway/TrueCodeExample.Gateway/TrueCodeExample.Gateway.csproj"

COPY . .
WORKDIR "/src/src/Gateway/TrueCodeExample.Gateway"
RUN dotnet publish "TrueCodeExample.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
USER root
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrueCodeExample.Gateway.dll"]
