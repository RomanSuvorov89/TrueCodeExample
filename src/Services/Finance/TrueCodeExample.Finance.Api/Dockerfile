FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/TrueCodeExample.Common/TrueCodeExample.Common.csproj", "src/TrueCodeExample.Common/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.Domain/TrueCodeExample.Finance.Domain.csproj", "src/Services/Finance/TrueCodeExample.Finance.Domain/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.Application/TrueCodeExample.Finance.Application.csproj", "src/Services/Finance/TrueCodeExample.Finance.Application/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.DataAccess/TrueCodeExample.Finance.DataAccess.csproj", "src/Services/Finance/TrueCodeExample.Finance.DataAccess/"]
COPY ["src/Services/Finance/TrueCodeExample.Finance.Api/TrueCodeExample.Finance.Api.csproj", "src/Services/Finance/TrueCodeExample.Finance.Api/"]

RUN dotnet restore "src/Services/Finance/TrueCodeExample.Finance.Api/TrueCodeExample.Finance.Api.csproj"

COPY . .
WORKDIR "/src/src/Services/Finance/TrueCodeExample.Finance.Api"
RUN dotnet publish "TrueCodeExample.Finance.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrueCodeExample.Finance.Api.dll"]
