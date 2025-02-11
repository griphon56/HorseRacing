using ErrorOr;

namespace HorseRacing.Application.Common.Interfaces.Services
{
    public interface IHttpContextUserService
    {
        /// <summary>
        /// Метод получения логина текущего пользователя из HttpContext
        /// </summary>
        ErrorOr<string> GetCurrentUserName();
    }
}
