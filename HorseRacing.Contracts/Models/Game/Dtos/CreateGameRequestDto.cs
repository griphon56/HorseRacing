using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class CreateGameRequestDto : BaseDto
    {
        /// <summary>
        /// Название игры
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Ставка пользователя создавшего игру
        /// </summary>
        public int BetAmount { get; set; } = 10;
        /// <summary>
        /// Масть лошади, 
        /// которую пользователь выбрал при создании игры
        /// </summary>
        public int BetSuit { get; set; } = 0;
    }
}
