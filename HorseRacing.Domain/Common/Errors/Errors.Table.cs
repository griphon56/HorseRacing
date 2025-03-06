using ErrorOr;

namespace HorseRacing.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Table
        {
            /// <summary>
            /// Ошибка возникает, если передано некорректное наименование поля, по которому осуществляется сортировка.
            /// </summary>
            public static Error UnknownField => Error.Validation($"Deb.Infrastructure.Extension.OrderByExtension.{nameof(UnknownField)}", "Передано неизвестное поле, по которому должна выполняться сортировка.");
        }
    }
}
