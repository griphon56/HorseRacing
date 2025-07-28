using HorseRacing.Application.Base;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Common
{
    public class JoinGameWithBetResult : BaseModelResult
    {
        /// <summary>
        /// Признак подключения последнего игрока
        /// </summary>
        public bool IsLastPlayer { get; set; }
    }
}
