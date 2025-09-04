using HorseRacing.Contracts.Base.Responses;

namespace HorseRacing.Contracts.Models.User.Responses.GetAccountBalance
{
    public class GetAccountBalanceResponse : BaseResponse<GetAccountBalanceResponseDto>
    {
        public GetAccountBalanceResponse() : base() { }
        public GetAccountBalanceResponse(GetAccountBalanceResponseDto data) : base(data) { }
    }
}
