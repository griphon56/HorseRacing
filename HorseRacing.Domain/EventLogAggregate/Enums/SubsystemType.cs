using System.ComponentModel;

namespace HorseRacing.Domain.EventLogAggregate.Enums
{
    public enum SubsystemType
    {
        /// <summary>
        /// Подсистема контроля пользователей
        /// </summary>
        [Description("Подсистема контроля пользователей")]
        UsersControlSubSys,
    }
}
