using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Commands.SelectAuthProvider;
using HorseRacing.Domain.Common.Models.Authentication.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace HorseRacing.Api.Middleware
{
    public class ValidateAuthentication
    {
        private readonly RequestDelegate _next;
        public const string headerAuthErrorDescriptionKey = "errorDescription";
        public const string headerAuthErrorCodeKey = "errorCode";
        public readonly IHttpContextUserService _httpContextUserService;

        public ValidateAuthentication(RequestDelegate next, IHttpContextUserService httpContextUserService)
        {
            _next = next;
            _httpContextUserService = httpContextUserService;
        }

        public async Task InvokeAsync(HttpContext context, IMediator _mediator
            , IOptions<JwtAuthenticationModuleOption> _jwtProviderSettings)
        {
            var selectedProviders = await _mediator.Send(
                new SelectAuthProviderCommand(new List<AuthenticationModuleOption>()
            {
                _jwtProviderSettings.Value
            }));

            var oldPath = context.Request.Path;
            var response = context.Response;

            await _next(context);
        }
    }
}
