using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HorseRacing.Api.Hubs
{
    public static class CustomUserIdProviderUtilities
    {
        /// <summary>
        /// Метод получения кода пользователя для хаба
        /// </summary>
        /// <param name="principal"></param>
        public static string GetUserIdForHub(this ClaimsPrincipal principal)
        {
            return principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
        }
        /// <summary>
        /// Метод получения логина пользователя для хаба
        /// </summary>
        /// <param name="principal"></param>
        public static string GetUserNameForHub(this ClaimsPrincipal principal)
        {
            return string.IsNullOrEmpty(principal?.Identity!.Name)
                ? principal?.FindFirst(JwtRegisteredClaimNames.Name)?.Value!
                : principal?.Identity!.Name!;
        }
    }
}
