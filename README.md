# TrueCodeExample

Микросервисная система на .NET 8: курсы валют ЦБ РФ, избранные валюты пользователя, JWT-аутентификация.

## Стек

- .NET 8, Minimal API, CQRS (vertical slice), Mediator, FluentValidation
- PostgreSQL, EF Core Migrations
- YARP Gateway, Refit, Quartz.NET
- Serilog + Seq (централизованные логи)
- Docker Compose
- xUnit, Moq, FluentAssertions

## Архитектура

```
Client → Gateway (YARP) → Users.Api / Finance.Api
                ↓
           PostgreSQL (UsersDb, FinanceDb)

CurrencyWorker → ЦБ РФ (XML) → FinanceDb
MigrationService → Migrate() для обеих БД (one-shot при старте)
```

| Сервис | Назначение |
|--------|------------|
| **Users.Api** | Регистрация, логин, refresh/logout (JWT + refresh tokens в Postgres) |
| **Finance.Api** | Список валют, курсы по избранному, управление favorites |
| **CurrencyWorker** | Периодическая синхронизация курсов с cbr.ru |
| **MigrationService** | Применение EF Core миграций перед стартом API |
| **Gateway** | Единая точка входа, проброс `Authorization` |

## Быстрый старт (Docker)

Требования: Docker Desktop (или Docker Engine + Compose v2).

```bash
docker compose up --build
```

Порядок запуска:
1. Postgres (healthcheck) и **Seq** (сбор логов)
2. `migration-service` — накатывает схему, завершается
3. `users-api`, `finance-api`, `currency-worker`
4. `gateway`

### Порты

| Сервис | URL |
|--------|-----|
| **Gateway** (точка входа) | http://localhost:5000 |
| **Seq** (просмотр логов) | http://localhost:8081 |
| Users.Api (напрямую) | http://localhost:5100 |
| Finance.Api (напрямую) | http://localhost:5200 |
| PostgreSQL | localhost:5432 |
| Seq ingestion | localhost:5341 |

Swagger доступен в Development при локальном запуске API (`/swagger`).

### Логи (Serilog + Seq)

Все сервисы пишут логи в **Console** (видны в `docker compose logs`) и в **Seq** (`http://localhost:8081`).

Фильтрация в Seq UI по свойству `Application` (имя сервиса): `TrueCodeExample.Users.Api`, `TrueCodeExample.Finance.Api`, `TrueCodeExample.Gateway`, `TrueCodeExample.CurrencyWorker`, `TrueCodeExample.MigrationService`.

Локально без Docker: поднимите Seq отдельно (`docker compose up seq -d`) или уберите `Seq:ServerUrl` из `appsettings.Development.yaml`.

### Повторный запуск миграций

После добавления новых миграций:

```bash
docker compose run --rm migration-service
```

## API через Gateway

Базовый URL: `http://localhost:5000`

### Auth (`/users/*` → Users.Api)

| Метод | Путь | Auth |
|-------|------|------|
| POST | `/users/auth/register` | — |
| POST | `/users/auth/login` | — |
| POST | `/users/auth/refresh` | — |
| POST | `/users/auth/logout` | Bearer |

Тело register/login:

```json
{ "name": "alice", "password": "secret12" }
```

Ответ:

```json
{
  "userId": "...",
  "accessToken": "...",
  "accessTokenExpiresAtUtc": "...",
  "refreshToken": "...",
  "refreshTokenExpiresAtUtc": "..."
}
```

### Finance (`/finance/*` → Finance.Api)

| Метод | Путь | Auth |
|-------|------|------|
| GET | `/finance/currencies` | Bearer |
| GET | `/finance/rates` | Bearer |
| POST | `/finance/favorites/{charCode}` | Bearer |
| DELETE | `/finance/favorites/{charCode}` | Bearer |

Курс в рублях за единицу: `Value / Nominal`.

### Проверка работы (PowerShell)

После `docker compose up --build` дождитесь синка валют (в логах `currency-worker`: `Synced N currencies from CBR`). Затем выполните в PowerShell:

```powershell
$baseUrl = "http://localhost:5000"
$name = "reviewer$((Get-Random))"

Write-Host "=== Register ===" -ForegroundColor Cyan
$reg = Invoke-RestMethod -Uri "$baseUrl/users/auth/register" `
    -Method POST -ContentType "application/json" `
    -Body (@{ name = $name; password = "secret12" } | ConvertTo-Json)
$headers = @{ Authorization = "Bearer $($reg.accessToken)" }
Write-Host "User: $name"

Write-Host "`n=== Login ===" -ForegroundColor Cyan
$login = Invoke-RestMethod -Uri "$baseUrl/users/auth/login" `
    -Method POST -ContentType "application/json" `
    -Body (@{ name = $name; password = "secret12" } | ConvertTo-Json)
Write-Host "Login OK, userId=$($login.userId)"

Write-Host "`n=== Currencies ===" -ForegroundColor Cyan
$currencies = Invoke-RestMethod -Uri "$baseUrl/finance/currencies" -Headers $headers
Write-Host "Total: $($currencies.Count)"
$currencies | Select-Object -First 3 | Format-Table charCode, name, nominal, value

Write-Host "=== Add USD to favorites ===" -ForegroundColor Cyan
$fav = Invoke-WebRequest -Uri "$baseUrl/finance/favorites/USD" -Method POST -Headers $headers -UseBasicParsing
Write-Host "Status: $($fav.StatusCode)"

Write-Host "`n=== Rates (favorites) ===" -ForegroundColor Cyan
$rates = Invoke-RestMethod -Uri "$baseUrl/finance/rates" -Headers $headers
$rates | Format-Table charCode, name, nominal, value

Write-Host "=== Refresh token ===" -ForegroundColor Cyan
$refreshed = Invoke-RestMethod -Uri "$baseUrl/users/auth/refresh" `
    -Method POST -ContentType "application/json" `
    -Body (@{ refreshToken = $reg.refreshToken } | ConvertTo-Json)
Write-Host "Refresh OK"

Write-Host "`n=== Logout ===" -ForegroundColor Cyan
$logout = Invoke-WebRequest -Uri "$baseUrl/users/auth/logout" -Method POST `
    -Headers @{ Authorization = "Bearer $($refreshed.accessToken)" } -UseBasicParsing
Write-Host "Status: $($logout.StatusCode)"

Write-Host "`n=== Old refresh token (expect 401) ===" -ForegroundColor Cyan
try {
    Invoke-WebRequest -Uri "$baseUrl/users/auth/refresh" -Method POST `
        -ContentType "application/json" `
        -Body (@{ refreshToken = $reg.refreshToken } | ConvertTo-Json) -UseBasicParsing
} catch {
    Write-Host "Status: $($_.Exception.Response.StatusCode.value__)"
}

Write-Host "`n=== Duplicate register (expect 409) ===" -ForegroundColor Cyan
try {
    Invoke-WebRequest -Uri "$baseUrl/users/auth/register" -Method POST `
        -ContentType "application/json" `
        -Body (@{ name = $name; password = "secret12" } | ConvertTo-Json) -UseBasicParsing
} catch {
    Write-Host "Status: $($_.Exception.Response.StatusCode.value__)"
}

Write-Host "`nDone." -ForegroundColor Green
```

Ожидаемые результаты:

| Шаг | HTTP-код |
|-----|----------|
| Register / Login / Currencies / Rates / Refresh | 200 |
| Add favorite / Logout | 204 |
| Повторный refresh старым токеном | 401 |
| Повторная регистрация | 409 |

> Кириллица в названиях валют в консоли может отображаться некорректно — это кодировка терминала; в JSON данные приходят в UTF-8.

### Пример сценария (curl, bash)

```bash
# Регистрация
curl -X POST http://localhost:5000/users/auth/register \
  -H "Content-Type: application/json" \
  -d '{"name":"alice","password":"secret12"}'

# Список валют (после синка worker'а)
curl http://localhost:5000/finance/currencies \
  -H "Authorization: Bearer <accessToken>"

# Добавить USD в избранное
curl -X POST http://localhost:5000/finance/favorites/USD \
  -H "Authorization: Bearer <accessToken>"

# Курсы избранных валют
curl http://localhost:5000/finance/rates \
  -H "Authorization: Bearer <accessToken>"
```

## Локальная разработка

Требования: .NET 8 SDK, PostgreSQL 16.

1. Создать БД (или запустить только Postgres из compose):

```bash
docker compose up postgres -d
```

2. Применить миграции:

```bash
dotnet run --project src/Services/MigrationService/TrueCodeExample.MigrationService
```

3. Запустить сервисы (в отдельных терминалах):

```bash
dotnet run --project src/Services/Users/TrueCodeExample.Users.Api
dotnet run --project src/Services/Finance/TrueCodeExample.Finance.Api
dotnet run --project src/Services/CurrencyWorker/TrueCodeExample.CurrencyWorker
dotnet run --project src/Gateway/TrueCodeExample.Gateway
```

Connection strings и `Jwt__SecretKey` задаются в `Properties/launchSettings.json` каждого проекта.

## Тесты

```bash
dotnet test
```

- `tests/TrueCodeExample.Users.Tests` — хендлеры Users
- `tests/TrueCodeExample.Finance.Tests` — хендлеры Finance

## Структура решения

```
src/
  TrueCodeExample.Common/          # JWT, YAML-конфиг, middleware, exceptions
  Gateway/TrueCodeExample.Gateway/
  Services/
    Users/                         # Domain → Application → DataAccess → Infrastructure → Api
    Finance/
    CurrencyWorker/
    MigrationService/
tests/
  TrueCodeExample.Users.Tests/
  TrueCodeExample.Finance.Tests/
docker/
  postgres/init-databases.sql
compose.yaml
```

## Конфигурация

- Формат: YAML (`appsettings.yaml` + `appsettings.{Environment}.yaml`)
- Логирование: Serilog (`Serilog` + `Seq:ServerUrl` в конфиге)
- Секреты и connection strings — через переменные окружения (`ConnectionStrings__UsersDb`, `Jwt__SecretKey`, `Seq__ServerUrl`, …)
- JWT: access token 15 мин, refresh token 7 дней, ротация при refresh, logout отзывает refresh в БД
