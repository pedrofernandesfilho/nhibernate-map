# nhibernate-map

## Docker-compose

Postgres database.

```bash
docker-compose up -d
```

## Migrations

```bash
dotnet run -p .\src\Migration\ up -s "Server=localhost;Port=5432;Database=products;User Id=admin;Password=123;"
```