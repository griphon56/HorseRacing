using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Domain.GameAggregate.Enums;

namespace HorseRacing.Contracts.Models.Game.Requests.CreateGame
{
    public class CreateGameRequestDto : BaseDto
    {
        /// <summary>
        /// Идентификатор пользователя создавшего игру
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Название игры
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Ставка пользователя создавшего игру
        /// </summary>
        public int BetAmount { get; set; } = 10;
        /// <summary>
        /// Режим игры
        /// <see cref="GameModeType"/>
        /// </summary>
        public int Mode { get; set; } = 0;
        /// <summary>
        /// Масть лошади, 
        /// которую пользователь выбрал при создании игры
        /// <see cref="SuitType"/>
        /// </summary>
        public int BetSuit { get; set; } = 0;
    }
}
