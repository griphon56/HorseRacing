# Информация по работе с БД
## Создание миграций БД
Всегда накатываем миграции на две БД
```
add-migration InitialCreate -Project HorseRacing.Migrations.MSSQL -StartupProject HorseRacing.Api -Args "--ProviderName MSSQLServer"
```

## Обновление БД
Всегда обновляем две БД
```
update-database -Project HorseRacing.Migrations.MSSQL -StartupProject HorseRacing.Api -Args "--ProviderName MSSQLServer"
```