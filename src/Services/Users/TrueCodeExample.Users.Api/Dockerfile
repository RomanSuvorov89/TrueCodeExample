FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/TrueCodeExample.Common/TrueCodeExample.Common.csproj", "src/TrueCodeExample.Common/"]
COPY ["src/Services/Users/TrueCodeExample.Users.Domain/TrueCodeExample.Users.Domain.csproj", "src/Services/Users/TrueCodeExample.Users.Domain/"]
COPY ["src/Services/Users/TrueCodeExample.Users.Application/TrueCodeExample.Users.Application.csproj", "src/Services/Users/TrueCodeExample.Users.Application/"]
COPY ["src/Services/Users/TrueCodeExample.Users.DataAccess/TrueCodeExample.Users.DataAccess.csproj", "src/Services/Users/TrueCodeExample.Users.DataAccess/"]
COPY ["src/Services/Users/TrueCodeExample.Users.Infrastructure/TrueCodeExample.Users.Infrastructure.csproj", "src/Services/Users/TrueCodeExample.Users.Infrastructure/"]
COPY ["src/Services/Users/TrueCodeExample.Users.Api/TrueCodeExample.Users.Api.csproj", "src/Services/Users/TrueCodeExample.Users.Api/"]

RUN dotnet restore "src/Services/Users/TrueCodeExample.Users.Api/TrueCodeExample.Users.Api.csproj"

COPY . .
WORKDIR "/src/src/Services/Users/TrueCodeExample.Users.Api"
RUN dotnet publish "TrueCodeExample.Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrueCodeExample.Users.Api.dll"]
