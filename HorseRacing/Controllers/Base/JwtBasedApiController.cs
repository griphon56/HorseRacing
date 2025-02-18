using HorseRacing.Domain.Common.Models.Authentication.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HorseRacing.Api.Controllers.Base
{
    /// <summary>
    /// API-контроллер на основе jwt.
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtAuthenticationModuleOption.SchemeName)]
    public class JwtBasedApiController : BaseApiController
    {
        public JwtBasedApiController(ILogger<BaseApiController> logger) : base(logger) { }
    }
}
