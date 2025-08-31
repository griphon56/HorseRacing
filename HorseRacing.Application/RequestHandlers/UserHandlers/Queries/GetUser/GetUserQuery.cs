using ErrorOr;
using HorseRacing.Application.RequestHandlers.UserHandlers.Common;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.UserHandlers.Queries.GetUser
{
    public class GetUserQuery : IRequest<ErrorOr<GetUserResult>>
    {
        public required UserId UserId { get; set; }
    }
}
