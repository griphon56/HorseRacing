using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Domain.Common.Errors;

namespace HorseRacing.Api.Services
{
    public class HttpContextUserService : IHttpContextUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string _DefaultTypeNameForNameSearch = "name";
        private const string _DefaultTypeNameForNameSearchDefaultAD = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        public HttpContextUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ErrorOr<string> GetCurrentUserName()
        {
            return GetCurrentUserName(_httpContextAccessor.HttpContext);
        }
        /// <summary>
        /// Метод получения логина текущего пользователя из HttpContext
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        private ErrorOr<string> GetCurrentUserName(HttpContext? httpContext)
        {
            if (httpContext == null || httpContext!.User == null || httpContext!.User!.Claims == null)
            {
                return Errors.User.WorkingUserNotFound;
            }

            return httpContext!.User!.Claims!.Where(m =>
                    m.Type == HttpContextUserService._DefaultTypeNameForNameSearch ||
                    m.Type == HttpContextUserService._DefaultTypeNameForNameSearchDefaultAD)
                .Select(m => m.Value).FirstOrDefault() ?? "";
        }
    }
}
