using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.User.Responses.GetAccountBalance
{
    public class GetAccountBalanceResponseDto : BaseDto
    {
        /// <summary>
        /// Код пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Баланс счета
        /// </summary>
        public decimal Balance { get; set; }
    }
}
