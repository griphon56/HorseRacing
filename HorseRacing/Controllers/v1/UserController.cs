using Asp.Versioning;
using HorseRacing.Api.Controllers.Base;
using HorseRacing.Application.RequestHandlers.UserHandlers.Commands.UpdateUser;
using HorseRacing.Application.RequestHandlers.UserHandlers.Queries.GetUser;
using HorseRacing.Contracts.Models.User.Requests.GetUser;
using HorseRacing.Contracts.Models.User.Requests.UpdateUser;
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

        [HttpPost("get-game")]
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

        [HttpPost("update-game")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var getUserResult = await _mediator.Send(_mapper.Map<UpdateUserCommand>(request));

            return getUserResult.Match(
                res => Ok(),
                errors => Problem(errors));
        }
    }
}
