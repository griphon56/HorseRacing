namespace HorseRacing.Application.Common.Exceptions
{
    /// <summary>
	/// Класс исключения касающиеся сортировки данных в таблицах
	/// </summary>
	public class TableOrderArgsException : Exception
    {
        public TableOrderArgsException() { }

        public TableOrderArgsException(string message) : base(message) { }

        public TableOrderArgsException(string message, Exception inner) : base(message, inner) { }
    }
}
