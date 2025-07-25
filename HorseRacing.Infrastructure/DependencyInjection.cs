using HorseRacing.Application.Common.Interfaces.Authentication;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.Common.Interfaces.Persistence.Base;
using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Domain.Common.Caching.Configuration;
using HorseRacing.Domain.Common.DependencyConfiguration;
using HorseRacing.Domain.Common.Models.Authentication;
using HorseRacing.Domain.Common.Models.Authentication.Configuration;
using HorseRacing.Infrastructure.Authentication;
using HorseRacing.Infrastructure.Persistence.DbContexts;
using HorseRacing.Infrastructure.Persistence.Interceptors;
using HorseRacing.Infrastructure.Persistence.Repositories.Base;
using HorseRacing.Infrastructure.Persistence.Repositories.Common;
using HorseRacing.Infrastructure.Services;
using HorseRacing.Infrastructure.Services.RedisCache;
using HorseRacing.LoggerExtenstions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace HorseRacing.Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Имя таблицы журнала событий.
        /// </summary>
        private const string EventLogTableName = "EventLog";
        /// <summary>
        /// Хаб подключения к SignalR
        /// </summary>
        public const string CommonServerHub = "/hubs/commonHub";
        /// <summary>
        /// Добавление инфраструктуры.
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services
            , Microsoft.Extensions.Configuration.IConfiguration configuration, ILoggingBuilder logger
            , InfrastructureConfiguration infrastructureConfiguration, out string selectedConnectionString
            , out string selectedProvider)
        {
            services.AddPersistence(configuration, out selectedProvider, out selectedConnectionString);
            services.AddLogging(logger, selectedProvider, selectedConnectionString);

            services.AddAuth(configuration, infrastructureConfiguration);

            services.AddHashPasswordSettings(configuration);

            services.AddCommonServices();
            services.AddCommonRepositories();

            if (infrastructureConfiguration.EnableCachingConfiguration)
            {
                services.AddCaching(configuration);
            }

            return services;
        }
        /// <summary>
        /// Добавляет кэширование
        /// </summary>
        /// <param name="services">Сервисы</param>
        /// <param name="configuration">Параметры конфигурации</param>
        public static IServiceCollection AddCaching(this IServiceCollection services
            , Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddCacheConfigurationSettings(configuration);
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddSingleton<IExtendedCacheService, ExtendedCacheService>();
            services.AddMemoryCache();

            //внимание! на момент вызова этого метода - обычные репозитории уже должны быть внедрены
            services.AddCachedRepositories();
            return services;
        }
        /// <summary>
        /// Инициализация сервисов
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameService, GameService>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IHashPasswordService, HashPasswordService>();
            return services;
        }
        /// <summary>
        /// Инициализация репозиториев
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddCommonRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));

            return services;
        }
        /// <summary>
        /// Репозитории кеширования
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddCachedRepositories(this IServiceCollection services)
        {
            //services.Decorate<IUserRepository, CachedUserRepository>();

            return services;
        }
        /// <summary>
        /// Конфигурация параметров хэширования паролей
        /// </summary>
        /// <param name="services">Ссылка на сервисы</param>
        /// <param name="configuration">Ссылка на конфигурацию</param>
        private static IServiceCollection AddHashPasswordSettings(this IServiceCollection services
            , Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var hashProviderSettings = new HashPasswordSettings();
            configuration.Bind(HashPasswordSettings.SectionName, hashProviderSettings);
            configuration.GetSection(HashPasswordSettings.SectionName);
            services.AddSingleton(Options.Create(hashProviderSettings));
            return services;
        }
        /// <summary>
        /// Конфигурация параметров кэширования
        /// </summary>
        /// <param name="services">Ссылка на сервисы</param>
        /// <param name="configuration">Ссылка на конфигурацию</param>
        private static IServiceCollection AddCacheConfigurationSettings(this IServiceCollection services
            , Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var cacheConfigurationSettingsSettings = new CacheConfigurationSettings();
            configuration.Bind(CacheConfigurationSettings.SectionName, cacheConfigurationSettingsSettings);
            configuration.GetSection(CacheConfigurationSettings.SectionName);
            services.AddSingleton(Options.Create(cacheConfigurationSettingsSettings));
            return services;
        }

        /// <summary>
        /// Добавление слоя доступа к данным
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="SelectedProvider">Выбранный провайдер.</param>
        /// <param name="SelectedConnectionString">Выбранная строка подключения.</param>
        private static IServiceCollection AddPersistence(this IServiceCollection services
            , Microsoft.Extensions.Configuration.IConfiguration configuration, out string SelectedProvider
            , out string SelectedConnectionString)
        {
            var dbProviderSettings = new DBProviderSettings();
            configuration.Bind(DBProviderSettings.SectionName, dbProviderSettings);
            configuration.GetSection(DBProviderSettings.SectionName);

            services.AddSingleton(Options.Create(dbProviderSettings));
            var InjectedCommandLineArgumentProviderName = configuration.GetValue(DBProviderSettings.ProviderNameArgsForCommandLine, "");

            var dbProvider = dbProviderSettings.ProviderName;
            if (!string.IsNullOrEmpty(InjectedCommandLineArgumentProviderName))
            {
                dbProvider = InjectedCommandLineArgumentProviderName;
            }
            SelectedProvider = dbProvider;
            var connectionString = SelectedConnectionString = configuration.GetConnectionString(dbProvider)!;

            services.AddDbContext<HRDbContext>(options =>
            {
                options.EnableDetailedErrors().EnableSensitiveDataLogging();
                options.DatabaseProviderConfiguration(dbProvider!, connectionString!);
                //options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            });

            services.AddScoped<PublishDomainEventsInterceptor>();

            return services;
        }

        /// <summary>
        /// Конфигурация провайдеров БД.
        /// </summary>
        /// <param name="options">Варианты.</param>
        /// <param name="provider">Провайдер.</param>
        /// <param name="connectionString">Строка подключения.</param>
        private static void DatabaseProviderConfiguration(this DbContextOptionsBuilder options, string provider, string connectionString)
        {
            if (provider == DBProviderSettings.MSSQLServer)
            {
                options.UseSqlServer(connectionString!, x => x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

                options.UseSqlServer(connectionString!, x => x.MigrationsAssembly("HorseRacing.Migrations.MSSQL"));
                    //.LogTo(Console.WriteLine, LogLevel.Information)
                    //.EnableSensitiveDataLogging();

            }
        }
        /// <summary>
        /// Добавление логирования.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="dbProvider">Провайдер базы данных.</param>
        /// <param name="connectionString">Строка подключения.</param>
        private static void AddLogging(this IServiceCollection services, ILoggingBuilder logger, string dbProvider, string connectionString)
        {
            var assembly = Assembly.GetExecutingAssembly().GetName();
            var LoggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug().Destructure.ToMaximumDepth(10)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", $"{assembly.Version}");

            if (dbProvider == DBProviderSettings.MSSQLServer)
            {
                var ColumnOptions = new Serilog.Sinks.MSSqlServer.ColumnOptions
                {
                    AdditionalColumns = new Collection<SqlColumn>
                    {
                        new SqlColumn {ColumnName = CustomLoggerExtensions.InitiatorInfoTextCustomColumnName, PropertyName =CustomLoggerExtensions.InitiatorInfoTextCustomColumnName, DataType = SqlDbType.NVarChar, DataLength = 500},
                        new SqlColumn {ColumnName = CustomLoggerExtensions.EventTypeCustomColumnName, PropertyName =CustomLoggerExtensions.EventTypeCustomColumnName, DataType = SqlDbType.Int},
                        new SqlColumn {ColumnName = CustomLoggerExtensions.SubsystemCustomColumnName, PropertyName =CustomLoggerExtensions.SubsystemCustomColumnName, DataType = SqlDbType.Int},
                        new SqlColumn {ColumnName = CustomLoggerExtensions.TechProcessCustomColumnName, PropertyName =CustomLoggerExtensions.TechProcessCustomColumnName, DataType = SqlDbType.Int},
                        new SqlColumn {ColumnName = CustomLoggerExtensions.EventTitleCustomColumnName, PropertyName =CustomLoggerExtensions.EventTitleCustomColumnName, DataType = SqlDbType.NVarChar, DataLength = 1000},
                        new SqlColumn {ColumnName = CustomLoggerExtensions.UserIdCustomColumnName, PropertyName =CustomLoggerExtensions.UserIdCustomColumnName, DataType = SqlDbType.UniqueIdentifier}
                    }
                };

                LoggerConfiguration.WriteTo.MSSqlServer(connectionString: connectionString,
                  sinkOptions: new MSSqlServerSinkOptions
                  {
                      TableName = EventLogTableName,
                  },
                columnOptions: ColumnOptions);
            }

            // Закомментировал пока запись логов в файл.
            Log.Logger = LoggerConfiguration/*.WriteTo.File(
				new CompactJsonFormatter(),
				Environment.CurrentDirectory + Path.Combine(Path.DirectorySeparatorChar.ToString(), "Logs", "log.json"),
				rollingInterval: RollingInterval.Day,
				restrictedToMinimumLevel: LogEventLevel.Information)*/.CreateBootstrapLogger();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
            logger.ClearProviders();
            logger.AddSerilog(Log.Logger);
            logger.AddConsole();
        }

        /// <summary>
        /// Добавление аутентификации.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="configuration">Конфигурация.</param>
        /// <exception cref="Exception"></exception>
        /// <returns>Возвращает коллекцию сервисов (IServiceCollection).</returns>
        private static IServiceCollection AddAuth(this IServiceCollection services
            , Microsoft.Extensions.Configuration.IConfiguration configuration
            , InfrastructureConfiguration infrastructureConfiguration)
        {
            var jwtModuleOption = new JwtAuthenticationModuleOption();
            configuration.Bind(JwtAuthenticationModuleOption.SectionName, jwtModuleOption);
            configuration.GetSection(JwtAuthenticationModuleOption.SectionName);

            if (string.IsNullOrEmpty(jwtModuleOption.Name))
            {
                throw new Exception($"Аутентификация с использованием Jwt обязательно должно присутствовать в appSettings.json, но параметр {JwtAuthenticationModuleOption.SectionName} не был обнаружен");
            }

            var authBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtAuthenticationModuleOption.SchemeName;
            });

            authBuilder.AddJwtBearer(jwtModuleOption);
            services.AddSingleton(Options.Create(jwtModuleOption));

            if (infrastructureConfiguration.EnableAuthorizationConfiguration)
            {
                services.AddAuthorization();
            }

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }

        /// <summary>
        /// Добавление jwt аутентификации.
        /// </summary>
        /// <param name="builder">Строитель.</param>
        /// <param name="jwtModuleOption">Опция модуля jwt.</param>
        /// <returns>Возвращает значение строителя аутентификации (AuthenticationBuilder).</returns>
        private static AuthenticationBuilder AddJwtBearer(this AuthenticationBuilder builder, JwtAuthenticationModuleOption jwtModuleOption)
        {
            return builder.AddJwtBearer(JwtAuthenticationModuleOption.SchemeName, options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtModuleOption.Settings.Issuer,
                    ValidAudience = jwtModuleOption.Settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtModuleOption.Settings.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = "";
                        if (context.Request.Headers.ContainsKey("Authorization") &&
                            context.Request.Headers["Authorization"][0].StartsWith("Bearer "))
                        {
                            accessToken = context.Request.Headers["Authorization"][0]
                                .Substring("Bearer ".Length);
                        }

                        if (context.Request.Path.StartsWithSegments(DependencyInjection.CommonServerHub))
                        {
                            if (string.IsNullOrEmpty(accessToken))
                            {
                                accessToken = context.Request.Query["access_token"];
                            }
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            }
          );
        }
    }
}
