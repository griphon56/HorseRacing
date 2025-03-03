using HorseRacing.Domain.Common.Models.Base;
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
        public DateTime  DateStart { get; private set; }
        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime  DateEnd { get; private set; }

        private Game () : base(GameId.CreateUnique(), new EntityChangeInfo(DateTime.UtcNow)) { }

        private Game(GameId id, StatusType status, string name, DateTime dateStart, DateTime dateEnd, EntityChangeInfo changeInfo)
             : base(id ?? GameId.CreateUnique(), changeInfo)
        {
            Status = status;
            Name = name;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }

        public static Game Create(GameId id, StatusType
            status, string name, DateTime dateStart, DateTime dateEnd, EntityChangeInfo changeInfo)
        {
            return new Game(id, status, name, dateStart, dateEnd, changeInfo);
        }
    }
}
