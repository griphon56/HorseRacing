using ErrorOr;
using HorseRacing.Api.Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HorseRacing.Api.Controllers.Base
{
    /// <summary>
    /// Базовый API-контроллер.
    /// </summary>
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private readonly ILogger<BaseApiController> _logger;

        public BaseApiController(ILogger<BaseApiController> logger)
        {
            _logger = logger;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected IActionResult Problem(List<Error> errors)
        {
            if (errors.Count == 0)
            {
                return Problem();
            }

            if (errors.All(m => m.Type == ErrorType.Validation || m.Type == ErrorType.Conflict))
            {
                return ValidationProblem(errors);
            }

            HttpContext.Items[HttpContextItemKeys.Errors] = errors;
            LogErrors(errors);
            return Problem(errors[0]);
        }

        private void LogErrors(List<Error> errors)
        {
            if (errors is null || errors.Count == 0)
            {
                return;
            }

            _logger.LogError($"Ошибки на уровне BaseApiController.Problem = {string.Join("; ", errors.Select(m => $"Код ошибки - {m.Code}, Описание - {m.Description} "))} ");
        }

        /// <summary>
        /// Метод возвращает ошибку со статусом и описанием.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <returns>Возвращение значения результата действия (IActionResult).</returns>
        private IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            return Problem(statusCode: statusCode, title: error.Description);
        }

        /// <summary>
        /// Метод валидации списка ошибок.
        /// </summary>
        /// <param name="errors">Ошибки.</param>
        /// <returns>Возвращение значения результата действия (IActionResult).</returns>
        private IActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();
            errors.ForEach(e => modelStateDictionary.AddModelError(e.Code, e.Description));
            LogErrors(errors);
            return ValidationProblem(modelStateDictionary);
        }
    }
}
