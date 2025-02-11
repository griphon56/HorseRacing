using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HorseRacing.Api.AuthorizationPolicy
{
    /// <summary>
    ///  Обработчик, содержащий логику авторизации
    /// </summary>
    public class SessionHandler : AuthorizationHandler<SessionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IServiceScopeFactory _serviceScopeFactory;
        public SessionHandler(IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected async override Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, SessionRequirement requirement)
        {
            var httpRequest = _httpContextAccessor.HttpContext!.Request;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                context.Succeed(requirement);
                return  Task.CompletedTask;
            }
        }
    }
}
