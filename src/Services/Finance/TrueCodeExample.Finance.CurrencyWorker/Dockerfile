FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/TrueCodeExample.Common/TrueCodeExample.Common.csproj", "src/TrueCodeExample.Common/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.Domain/TrueCodeExample.Finance.Domain.csproj", "src/Services/Finance/TrueCodeExample.Finance.Domain/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.Application/TrueCodeExample.Finance.Application.csproj", "src/Services/Finance/TrueCodeExample.Finance.Application/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.DataAccess/TrueCodeExample.Finance.DataAccess.csproj", "src/Services/Finance/TrueCodeExample.Finance.DataAccess/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.Infrastructure/TrueCodeExample.Finance.Infrastructure.csproj", "src/Services/Finance/TrueCodeExample.Finance.Infrastructure/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.CurrencyWorker/TrueCodeExample.Finance.CurrencyWorker.csproj", "src/Services/Finance/TrueCodeExample.Finance.CurrencyWorker/"]

RUN dotnet restore "src/Services/Finance/TrueCodeExample.Finance.CurrencyWorker/TrueCodeExample.Finance.CurrencyWorker.csproj"

COPY . .
WORKDIR "/src/src/Services/Finance/TrueCodeExample.Finance.CurrencyWorker"
RUN dotnet publish "TrueCodeExample.Finance.CurrencyWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrueCodeExample.Finance.CurrencyWorker.dll"]
