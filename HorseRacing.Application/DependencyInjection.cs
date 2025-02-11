using FluentValidation;
using HorseRacing.Application.Common.Behaviors;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HorseRacing.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            ///<summary>
            /// Добавляет MediatR для обработки команд и запросов, регистрация сервисов и обработчиков из DI.
            /// </summary>
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);

            });
            ///<summary>
            /// Добавляет пайплайн-поведение для валидации запросов и команд.
            /// </summary>
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMappings();
            return services;
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}
