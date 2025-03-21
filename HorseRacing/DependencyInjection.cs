using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Text.Json.Serialization;
using HorseRacing.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using HorseRacing.Api.Extensions;
using HorseRacing.Api.Helpers;
using HorseRacing.Api.Common.Errors;
using HorseRacing.Api.AuthorizationPolicy;
using HorseRacing.Api.Services;
using HorseRacing.Application.Common.Interfaces.Services;

namespace HorseRacing.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, WebApplicationBuilder builder)
        {
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;

            if (!isDevelopment)
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));
                });
            }

            //services.AddControllers();
            services.AddControllers()
                .AddJsonOptions(options => {
                    // Отключает преобразование PascaleCase свойств сущностей в camelCase
                    // при получении запросов и отправке ответов
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            services.AddEndpointsApiExplorer()
                .AddApiVersioningExtension()
                .AddSwaggerGenExtension();

            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.WriteIndented = true;

                // serialize enums as strings in api responses (e.g. Role)
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                // ignore omitted parameters on models to enable optional params (e.g. User update)
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            builder.Services.AddHttpContextAccessor();

            services.AddCors(o => o.AddPolicy("AllowFrontend", builder =>
            {
                builder
                    .WithOrigins("http://127.0.0.1:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSingleton<ProblemDetailsFactory, HorseRacingProblemDetailsFactory>();

            //services.AddSignalR(options => options.EnableDetailedErrors = true);

            #region Внедрение сервиса определения пользователя
            services.AddTransient<IHttpContextUserService, HttpContextUserService>();
            //services.AddScoped<IOuterCommonHubCallService, OuterCommonHubCallService>();
            #endregion

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            builder.Services.AddSingleton<IAuthorizationHandler, SessionHandler>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.SessionPolicy, policy =>
                {
                    policy.Requirements.Add(new SessionRequirement("X-Session-Id"));
                });
            });

            return services;
        }

        public static void MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<HRDbContext>();

                dbContext.Database.Migrate();
            }
        }
    }
}
