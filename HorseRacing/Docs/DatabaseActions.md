# Информация по работе с БД
## Создание миграций БД
```
add-migration InitialCreate -Project HorseRacing.Migrations.MSSQL -StartupProject HorseRacing.Api -Args "--ProviderName MSSQLServer"
```

## Обновление БД
```
update-database -Project HorseRacing.Migrations.MSSQL -StartupProject HorseRacing.Api -Args "--ProviderName MSSQLServer"
```