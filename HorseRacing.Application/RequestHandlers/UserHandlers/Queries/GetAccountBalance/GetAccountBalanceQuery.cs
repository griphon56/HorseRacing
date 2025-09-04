using ErrorOr;
using HorseRacing.Application.RequestHandlers.UserHandlers.Common;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.UserHandlers.Queries.GetAccountBalance
{
    public class GetAccountBalanceQuery : IRequest<ErrorOr<GetAccountBalanceResult>>
    {
        public UserId UserId { get; set; }
    }
}
