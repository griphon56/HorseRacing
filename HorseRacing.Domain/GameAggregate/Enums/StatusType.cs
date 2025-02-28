using System.ComponentModel;

namespace HorseRacing.Domain.GameAggregate.Enums
{
    /// <summary>
    /// Статус игры
    /// </summary>
    public enum StatusType
    {
        [Description("Ожидание")]
        Wait,
        [Description("В процессе")]
        InProgress,
        [Description("Завершена")]
        Complete
    }
}
