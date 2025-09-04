using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.User.Requests.GetAccountBalance
{
    public class GetAccountBalanceRequestDto : BaseDto
    {
        /// <summary>
        /// Код пользователя
        /// </summary>
        public Guid UserId { get; set; }
    }
}
