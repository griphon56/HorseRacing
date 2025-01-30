namespace HorseRacing.Domain.Common.Interfases
{
    /// <summary>
	/// Интерфейс объект значения
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IOrdinaryValueObject<T>
    {
        /// <summary>
        /// Значение
        /// </summary>
        public T Value { get; }
    }
}
