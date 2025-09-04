using Asp.Versioning;
using HorseRacing.Api.Controllers.Base;
using HorseRacing.Application.RequestHandlers.UserHandlers.Commands.UpdateUser;
using HorseRacing.Application.RequestHandlers.UserHandlers.Queries.GetAccountBalance;
using HorseRacing.Application.RequestHandlers.UserHandlers.Queries.GetUser;
using HorseRacing.Contracts.Models.User.Requests.GetAccountBalance;
using HorseRacing.Contracts.Models.User.Requests.GetUser;
using HorseRacing.Contracts.Models.User.Requests.UpdateUser;
using HorseRacing.Contracts.Models.User.Responses.GetAccountBalance;
using HorseRacing.Contracts.Models.User.Responses.GetUser;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HorseRacing.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : JwtBasedApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserController(IMediator mediator, IMapper mapper, ILogger<BaseApiController> logger) : base(logger)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("get-user")]
        public async Task<IActionResult> GetUser([FromBody] GetUserRequest request)
        {
            var getUserResult = await _mediator.Send(new GetUserQuery()
            {
                UserId = UserId.Create(request.Data.UserId)
            });

            return getUserResult.Match(
                res => Ok(_mapper.Map<GetUserResponse>(res)),
                errors => Problem(errors));
        }

        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var getUserResult = await _mediator.Send(_mapper.Map<UpdateUserCommand>(request));

            return getUserResult.Match(
                res => Ok(),
                errors => Problem(errors));
        }

        [HttpPost("get-account-balance")]
        public async Task<IActionResult> GetAccountBalance([FromBody] GetAccountBalanceRequest request)
        {
            var geResult = await _mediator.Send(new GetAccountBalanceQuery()
            {
                UserId = UserId.Create(request.Data.UserId)
            });

            return geResult.Match(
                res => Ok(_mapper.Map<GetAccountBalanceResponse>(res)),
                errors => Problem(errors));
        }
    }
}
