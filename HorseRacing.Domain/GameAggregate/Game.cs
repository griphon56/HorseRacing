using HorseRacing.Common;
using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.GameAggregate.Entities;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MediatR;
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
        private readonly List<GameResult> _gameResults = new();
        public IReadOnlyList<GameResult> GameResults => _gameResults.AsReadOnly();

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
        /// <summary>
        /// Метод создания игры
        /// </summary>
        /// <param name="id">Код игры</param>
        /// <param name="name">Наименование игры</param>
        /// <param name="status">Статус</param>
        /// <param name="changeInfo"><see cref="EntityChangeInfo"/></param>
        public static Game Create(GameId id, string name, StatusType status, EntityChangeInfo changeInfo)
        {
            return new Game(id, name, status, changeInfo);
        }

        /// <summary>
        /// Метод обновления информации о игре
        /// </summary>
        /// <param name="name">Наименоване</param>
        /// <param name="status">Статус игры</param>
        /// <param name="dt_start">Дата начала</param>
        /// <param name="dt_end">Дата окончания</param>
        public void Update(string name, StatusType status, DateTime? dt_start = null, DateTime? dt_end = null)
        {
            Name = name;
            Status = status;
            DateStart = dt_start;
            DateEnd = dt_end;
        }

        /// <summary>
        /// Метод подключения игрока
        /// </summary>
        /// <param name="gamePlayer">Игрок</param>
        public void JoinPlayer(GamePlayer gamePlayer)
        {
            if (gamePlayer is not null && this._gamePlayers.Count <= CommonSystemValues.NumberOfPlayers)
            {
                _gamePlayers.Add(gamePlayer);
            }
        }
        /// <summary>
        /// Метод добавления ставки
        /// </summary>
        /// <param name="userId">Код пользователя</param>
        /// <param name="betAmount">Ставка</param>
        /// <param name="betSuit">Масть</param>
        public void PlaceBet(UserId userId, int betAmount, SuitType betSuit)
        {
            var bet = this._gamePlayers.Where(x => x.UserId == userId).FirstOrDefault();
            if (bet is not null)
            {
                bet.Update(betAmount, betSuit);
            }
        }
        /// <summary>
        /// Метод создания и перемешивания колоды
        /// </summary>
        public void InitializeDeck()
        {
            _gameDeckCards.Clear();

            var suits = Enum.GetValues(typeof(SuitType)).Cast<SuitType>()
                .Where(x => x != SuitType.None);
            var ranks = Enum.GetValues(typeof(RankType)).Cast<RankType>()
                .Where(x => x != RankType.Ace);

            var deck = new List<GameDeckCard>();
            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    deck.Add(GameDeckCard.Create(GameDeckCardId.CreateUnique(), this.Id, suit, rank
                        , 0, ZoneType.Deck));
                }
            }

            var rnd = new Random();
            for (int i = deck.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                var temp = deck[i];
                deck[i] = deck[j];
                deck[j] = temp;
            }

            for (int i = 0; i < deck.Count; i++)
            {
                if (i < CommonSystemValues.NumberOfObstacles)
                {
                    deck[i].SetCardZone(ZoneType.Table);
                }

                deck[i].SetCardOrder(i + 1);
            }

            _gameDeckCards.AddRange(deck);
        }
        /// <summary>
        /// Метод инициализации позиций лошадей
        /// </summary>
        public void InitializeHorsePositions()
        {
            _gameHorsePositions.Clear();

            var suits = Enum.GetValues(typeof(SuitType)).Cast<SuitType>()
                .Where(x => x != SuitType.None);

            var horsePositions = new List<GameHorsePosition>();
            foreach (var suit in suits)
            {
                horsePositions.Add(GameHorsePosition.Create(GameHorsePositionId.CreateUnique(), this.Id, suit, 0));
            }

            _gameHorsePositions.AddRange(horsePositions);
        }
        /// <summary>
        /// Метод извлечения карты из колоды
        /// </summary>
        public GameDeckCard GetCardFromDeck()
        {
            var card = _gameDeckCards.Where(card => card.Zone == ZoneType.Deck)
                .OrderBy(x => x.CardOrder).FirstOrDefault();
            if (card is not null)
            {
                card.SetCardZone(ZoneType.Discarded);
            }

            return card;
        }
        /// <summary>
        /// Метод открытия карты на столе
        /// </summary>
        public GameDeckCard GetCardFromTable()
        {
            var card = _gameDeckCards.Where(card => card.Zone == ZoneType.Table)
                .OrderBy(x => x.CardOrder).FirstOrDefault();
            if (card is not null)
            {
                card.SetCardZone(ZoneType.Discarded);
            }

            return card;
        }

        /// <summary>
        /// Метод обновления положения лошади на основе масти карты
        /// </summary>
        public int UpdateHorsePosition(GameDeckCard card)
        {
            if (card is null) return -1;

            var horsePosition = _gameHorsePositions
                .Where(horse => horse.HorseSuit == card.CardSuit).FirstOrDefault();
            if (horsePosition is not null)
            {
                horsePosition.SetPosition(horsePosition.Position + 1);
                return horsePosition.Position;
            }

            return -1;
        }

        /// <summary>
        /// Метод обновления положения лошади на основе масти карты преграды
        /// </summary>
        public int UpdateHorsePositionWithBlock(GameDeckCard card)
        {
            if (card is null) return -1;

            var horsePosition = _gameHorsePositions
                .Where(horse => horse.HorseSuit == card.CardSuit).FirstOrDefault();
            if (horsePosition is not null)
            {
                horsePosition.SetPosition(horsePosition.Position - 1);
                return horsePosition.Position;
            }

            return -1;
        }
        /// <summary>
        /// Метод проверки завершения игры
        /// </summary>
        /// <returns>Возвращает true, если одна из лошадей пересекла финишную линию</returns>
        public bool IsGameFinished()
        {
            return _gameHorsePositions.Select(x => x.Position).Max() == CommonSystemValues.NumberOfObstacles + 1;
        }

        /// <summary>
        /// Метод добавления результата игры
        /// </summary>
        /// <param name="gameResult"></param>
        public void AddGameResult(List<GameResult> gameResults)
        {
            if (gameResults is not null && gameResults.Count > 0)
            {
                _gameResults.AddRange(gameResults);
            }
        }

        public void AddEventGame(GameId gameId, int step, GameEventType eventType, SuitType? cardSuit = null
            , RankType? cardRank = null, int? cardOrder = null, SuitType? horseSuit = null, int? position = null)
        {
            _gameEvents.Add(GameEvent.Create(gameId, step, eventType, cardSuit, cardRank, cardOrder, horseSuit, position));
        }
    }
}
