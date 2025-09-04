using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.User.Requests.GetAccountBalance
{
    public class GetAccountBalanceRequest : BaseRequest<GetAccountBalanceRequestDto>
    {
        public GetAccountBalanceRequest() : base() { }
        public GetAccountBalanceRequest(GetAccountBalanceRequestDto data) : base(data) { }
    }
}
