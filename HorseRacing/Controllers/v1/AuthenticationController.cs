using Asp.Versioning;
using HorseRacing.Api.Controllers.Base;
using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Commands.RegistrationUsingForms;
using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Queries.LoginUsingForms;
using HorseRacing.Contracts.Models.Authentication.Requests;
using HorseRacing.Contracts.Models.Authentication.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HorseRacing.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [AllowAnonymous]
    public class AuthenticationController : JwtBasedApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(IMediator mediator, IMapper mapper, ILogger<BaseApiController> logger) : base(logger)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            var command = _mapper.Map<RegistrationCommand>(request.Data);
            var registerResult = await _mediator.Send(command);

            return registerResult.Match(
                registerResult => Ok(_mapper.Map<AuthenticationResponse>(registerResult)),
                errors => Problem(errors));
        }
        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var loginQuery = _mapper.Map<LoginQuery>(request.Data);
            var loginResult = await _mediator.Send(loginQuery);

            if (loginResult.IsError && loginResult.Errors.Any(e => 
                e == Domain.Common.Errors.Errors.Authentication.UserNotFound ||
                e == Domain.Common.Errors.Errors.Authentication.PasswordIncorrect))
            {
                return Problem(statusCode: StatusCodes.Status401Unauthorized, title: loginResult.FirstError.Description);
            }

            return loginResult.Match(
                loginResult => Ok(_mapper.Map<AuthenticationResponse>(loginResult)),
                errors => Problem(errors));
        }
    }
}
