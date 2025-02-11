using Microsoft.AspNetCore.Authorization;

namespace HorseRacing.Api.AuthorizationPolicy
{
    /// <summary>
    /// Требование для проверки заголовка сеанса
    /// </summary>
    public class SessionRequirement : IAuthorizationRequirement
    {
        public SessionRequirement(string sessionHeaderName)
        {
            SessionHeaderName = sessionHeaderName;
        }
        public string SessionHeaderName { get; }
    }
}
