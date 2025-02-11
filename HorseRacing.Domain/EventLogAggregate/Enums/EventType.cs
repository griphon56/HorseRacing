using HorseRacing.Common.Utilities;
using System.ComponentModel;

namespace HorseRacing.Domain.EventLogAggregate.Enums
{
    public enum EventType
    {
        None,
        /// <summary>
        /// Регистрация прошла успешно
        /// </summary>
        [Description("Регистрация прошла успешно"), EnumVisibility(true), EnumSeverity(SeverityType.success)]
        RegistrationSuccess,
        /// <summary>
        /// Во время регистрации возникла ошибка
        /// </summary>
        [Description("Во время регистрации возникла ошибка"), EnumVisibility(true), EnumSeverity(SeverityType.warning)]
        RegistrationError,
    }
}
