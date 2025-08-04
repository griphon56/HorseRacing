using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Models.Game.Dtos;
using HorseRacing.Domain.GameAggregate.ValueObjects;

namespace HorseRacing.Contracts.Models.Game.Responses.PlayGame
{
    public class PlayGameResponseDto : BaseDto
    {
        /// <summary>
        /// Код игры
        /// </summary>
        public GameId GameId { get; set; }
        /// <summary>
        /// Назване игры
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Колода до начала игры
        /// </summary>
        public List<GameDeckCardDto> InitialDeck { get; set; } = new();
        /// <summary>
        /// Информация о лошади и игроке который на неё поставил
        /// </summary>
        public List<HorseBetDto> HorseBets { get; set; } = new();
        /// <summary>
        /// Все события по шагам
        /// </summary>
        public List<GameEventDto> Events { get; set; } = new();
    }
}
