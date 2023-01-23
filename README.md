### Localization Api Service
#### Contains
* Asp.net core 6.0 web api version app
* Translations Api - Create, Update, GetByEntity, GetById, Search methods
* Postgres data store, Redis like cache. 

### You need up redis and postgres for local development.
### You can use:

### Docker commands

- `docker run --name loc_redis_preview -d -p 6379:6379 redis`
- `docker run -p 5432:5432 -e POSTGRES_PASSWORD="loc" -e POSTGRES_USER="loc" -e POSTGRES_DB="localizations" --name loc_preview postgres:13`

### Or 

### Docker compose

`docker-compose up --build -d` from ./ directory

### p.s. don't forget start migrations project (directory ./backend/LocalizationPreview.Migrations/LocalizationPreview.Migrations.csproj)

### Swagger UI
* http://localhost:5000/localization-api/swagger/index.html

Graph flow:
```mermaid
graph TD;
  User-->LocalizationService;
  LocalizationService-->Database;
  LocalizationService-->Cache(Redis);
  AnotherServices-->Cache(Redis);
```
