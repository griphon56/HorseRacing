namespace HorseRacing.Application.Common.Interfaces.Authentication
{
    /// <summary>
	///  Генератор Jwt токена пользователя.
	/// </summary>
	public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Метод генерации токена
        /// </summary>
        string GenerateToken(Domain.UserAggregate.User user);
    }
}
