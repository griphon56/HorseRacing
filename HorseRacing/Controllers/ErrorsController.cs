using Microsoft.AspNetCore.Mvc;

namespace HorseRacing.Api.Controllers
{
    public class ErrorsController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]
        public IActionResult Error()
        {
            return Problem();
        }
    }
}
