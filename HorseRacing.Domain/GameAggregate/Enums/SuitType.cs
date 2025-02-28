using System.ComponentModel;

namespace HorseRacing.Domain.GameAggregate.Enums
{
    /// <summary>
    /// Тип масти
    /// </summary>
    public enum SuitType
    {
        [Description("Бубны")]
        Diamonds,
        [Description("Червы")]
        Hearts,
        [Description("Пики")]
        Spades,
        [Description("Трефы")]
        Clubs
    }
}
