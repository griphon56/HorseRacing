using HorseRacing.Domain.GameAggregate.Enums;

namespace HorseRacing.Domain.GameAggregate.ReadOnlyModels
{
    /// <summary>
    /// Модель представления доступных мастей игры
    /// </summary>
    public class GameAvailableSuitView
    {
        /// <summary>
        /// Enum масти
        /// </summary>
        public SuitType Suit { get; set; }
        /// <summary>
        /// Название масти
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
