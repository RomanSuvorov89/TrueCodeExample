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

Finance.CurrencyWorker → Finance.Infrastructure (CBR) → ЦБ РФ (XML) → FinanceDb
MigrationService → Migrate() для обеих БД (one-shot при старте)
```

| Сервис | Назначение |
|--------|------------|
| **Users.Api** | Регистрация, логин, refresh/logout (JWT + refresh tokens в Postgres) |
| **Finance.Api** | Список валют (публичный), курсы по избранному, управление favorites |
| **Finance.CurrencyWorker** | Периодическая синхронизация курсов с cbr.ru (Quartz) |
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
4. `gateway` (ждёт healthcheck API)

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

Фильтрация в Seq UI по свойству `Application` (имя сервиса): `TrueCodeExample.Users.Api`, `TrueCodeExample.Finance.Api`, `TrueCodeExample.Gateway`, `TrueCodeExample.Finance.CurrencyWorker`, `TrueCodeExample.MigrationService`.

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
| GET | `/finance/currencies` | — (публичный) |
| GET | `/finance/rates` | Bearer |
| POST | `/finance/favorites/{charCode}` | Bearer |
| DELETE | `/finance/favorites/{charCode}` | Bearer |

Курс в рублях за единицу: `Value / Nominal`.

### Пример сценария (curl, bash)

```bash
# Регистрация
curl -X POST http://localhost:5000/users/auth/register \
  -H "Content-Type: application/json" \
  -d '{"name":"alice","password":"secret12"}'

# Список валют (публичный, после синка worker'а)
curl http://localhost:5000/finance/currencies

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
dotnet run --project src/Services/Finance/TrueCodeExample.Finance.CurrencyWorker
dotnet run --project src/Gateway/TrueCodeExample.Gateway
```

Connection strings и `Jwt__SecretKey` задаются в `Properties/launchSettings.json` каждого проекта.

## Тесты

```bash
dotnet test
```

- `tests/TrueCodeExample.Users.Tests` — unit-тесты хендлеров Users
- `tests/TrueCodeExample.Finance.Tests` — unit-тесты хендлеров Finance
- `tests/TrueCodeExample.Integration.Tests` — API + Postgres (Testcontainers, нужен Docker)

CI: `.github/workflows/ci.yml` (build + unit + integration tests).

## Структура решения

```
src/
  TrueCodeExample.Common/          # JWT, YAML-конфиг, middleware, exceptions
  Gateway/TrueCodeExample.Gateway/
  Services/
    Users/                         # Domain → Application → DataAccess → Infrastructure → Api
    Finance/                       # Domain → Application → DataAccess → Infrastructure → Api → CurrencyWorker
    MigrationService/
tests/
  TrueCodeExample.Users.Tests/
  TrueCodeExample.Finance.Tests/
  TrueCodeExample.Integration.Tests/
docker/
  postgres/init-databases.sql
compose.yaml
```

## Конфигурация

- Формат: YAML (`appsettings.yaml` + `appsettings.{Environment}.yaml`)
- Логирование: Serilog (`Serilog` + `Seq:ServerUrl` в конфиге)
- Секреты и connection strings — через переменные окружения (`ConnectionStrings__UsersDb`, `Jwt__SecretKey`, `Seq__ServerUrl`, …)
- JWT: access token 15 мин, refresh token 7 дней, ротация при refresh, logout отзывает refresh в БД (access token действует до истечения TTL)
- Health: `GET /health` на Gateway, Users.Api, Finance.Api и CurrencyWorker
- DataAccess: read-only запросы перед `Update`/`Remove` используют `AsNoTracking()`
