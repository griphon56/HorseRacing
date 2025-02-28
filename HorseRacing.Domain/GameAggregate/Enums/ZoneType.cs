using System.ComponentModel;

namespace HorseRacing.Domain.GameAggregate.Enums
{
    /// <summary>
    /// Расположение карты
    /// </summary>
    public enum ZoneType
    {
        [Description("В колоде")]
        Deck,
        [Description("На столе")]
        Table,
        [Description("Сброшена")]
        Discarded
    }
}
