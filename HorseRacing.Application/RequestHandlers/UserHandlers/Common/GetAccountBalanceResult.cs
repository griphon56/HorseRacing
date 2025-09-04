using HorseRacing.Application.Base;
using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Application.RequestHandlers.UserHandlers.Common
{
    public class GetAccountBalanceResult : BaseResult
    {
        /// <summary>
        /// Код пользователя
        /// </summary>
        public UserId UserId { get; set; }
        /// <summary>
        /// Баланс счёта
        /// </summary>
        public decimal Balance { get; set; }
    }
}
