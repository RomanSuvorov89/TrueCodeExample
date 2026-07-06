FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/TrueCodeExample.Common/TrueCodeExample.Common.csproj", "src/TrueCodeExample.Common/"]
COPY ["src/Services/Users/TrueCodeExample.Users.Domain/TrueCodeExample.Users.Domain.csproj", "src/Services/Users/TrueCodeExample.Users.Domain/"]
COPY ["src/Services/Users/TrueCodeExample.Users.Application/TrueCodeExample.Users.Application.csproj", "src/Services/Users/TrueCodeExample.Users.Application/"]
COPY ["src/Services/Users/TrueCodeExample.Users.DataAccess/TrueCodeExample.Users.DataAccess.csproj", "src/Services/Users/TrueCodeExample.Users.DataAccess/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.Domain/TrueCodeExample.Finance.Domain.csproj", "src/Services/Finance/TrueCodeExample.Finance.Domain/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.Application/TrueCodeExample.Finance.Application.csproj", "src/Services/Finance/TrueCodeExample.Finance.Application/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.DataAccess/TrueCodeExample.Finance.DataAccess.csproj", "src/Services/Finance/TrueCodeExample.Finance.DataAccess/"]
COPY ["src/Services/MigrationService/TrueCodeExample.MigrationService/TrueCodeExample.MigrationService.csproj", "src/Services/MigrationService/TrueCodeExample.MigrationService/"]

RUN dotnet restore "src/Services/MigrationService/TrueCodeExample.MigrationService/TrueCodeExample.MigrationService.csproj"

COPY . .
WORKDIR "/src/src/Services/MigrationService/TrueCodeExample.MigrationService"
RUN dotnet publish "TrueCodeExample.MigrationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrueCodeExample.MigrationService.dll"]
