namespace HorseRacing.Application.Common.Interfaces.Services
{
    /// <summary>
	/// Интерфейс провайдера даты и времени.
	/// </summary>
	public interface IDateTimeProvider
    {
        /// <summary>
        /// Возвращает объект DateTime, которому присвоены текущие дата и время данного компьютера,
        /// выраженные в формате UTC.
        /// </summary>
        public DateTime UtcNow { get; }
    }
}
