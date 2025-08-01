using HorseRacing.Application.Base;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;
using HorseRacing.Domain.GameAggregate.ValueObjects;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Common
{
    public class PlayGameResult : BaseModelResult
    {
        /// <summary>
        /// Код игры
        /// </summary>
        public required GameId GameId { get; set; }
        /// <summary>
        /// Колода до начала игры
        /// </summary>
        public List<GameDeckCardView> InitialDeck { get; set; } = new();
        /// <summary>
        /// Информация о лошади и игроке который на неё поставил
        /// </summary>
        public List<HorseBetView> HorseBets { get; set; } = new();
        /// <summary>
        /// Все события по шагам
        /// </summary>
        public List<GameEventView> Events { get; set; } = new();

        public PlayGameResult(GameId gameId, List<GameDeckCardView> initialDeck, List<HorseBetView> horseBets, List<GameEventView> events)
        {
            GameId = gameId;
            InitialDeck = initialDeck;
            HorseBets = horseBets;
            Events = events;
        }
    }
}
