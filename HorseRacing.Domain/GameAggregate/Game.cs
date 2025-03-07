using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.GameAggregate.Entities;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace HorseRacing.Domain.GameAggregate
{
    /// <summary>
    /// Агрегат "Игра"
    /// </summary>
    [Display(Description = "Игра")]
    public class Game : AggregateRootChangeInfoGuid<GameId>
    {
        /// <summary>
        /// Статус игры
        /// </summary>
        public StatusType Status { get; private set; }
        /// <summary>
        /// Наименование комнаты
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        /// <summary>
        /// Дата начала игры
        /// </summary>
        public DateTime? DateStart { get; private set; }
        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime? DateEnd { get; private set; }

        /// <summary>
        /// Результат игры
        /// </summary>
        private GameResult? _gameResult;
        public GameResult GameResult => _gameResult;

        /// <summary>
		/// Колода карт
		/// </summary>
		private readonly List<GameDeckCard> _gameDeckCards = new();
        public IReadOnlyList<GameDeckCard> GameDeckCards => _gameDeckCards.AsReadOnly();

        /// <summary>
		/// События игры
		/// </summary>
		private readonly List<GameEvent> _gameEvents = new();
        public IReadOnlyList<GameEvent> GameEvents => _gameEvents.AsReadOnly();

        /// <summary>
		/// Позиция лошадей
		/// </summary>
		private readonly List<GameHorsePosition> _gameHorsePositions = new();
        public IReadOnlyList<GameHorsePosition> GameHorsePositions => _gameHorsePositions.AsReadOnly();

        /// <summary>
		/// Игроки
		/// </summary>
		private readonly List<GamePlayer> _gamePlayers = new();
        public IReadOnlyList<GamePlayer> GamePlayers => _gamePlayers.AsReadOnly();

        private Game() : base(GameId.CreateUnique(), new EntityChangeInfo(DateTime.UtcNow)) { }

        private Game(GameId id, string name, StatusType status, EntityChangeInfo changeInfo
            , DateTime? dateStart = null, DateTime? dateEnd = null)
            : base(id ?? GameId.CreateUnique(), changeInfo)
        {
            Name = name;
            Status = status;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }

        public static Game Create(GameId id, string name, StatusType status, EntityChangeInfo changeInfo)
        {
            return new Game(id, name, status, changeInfo);
        }

        public void JoinPlayer(List<GamePlayer> gamePlayers)
        {
            if(gamePlayers is not null && gamePlayers.Count > 0)
            {
                _gamePlayers.AddRange(gamePlayers);
            }
        }
    }
}
