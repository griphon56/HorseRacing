using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using ErrorOr;
using HorseRacing.Api.Common.Http;

namespace HorseRacing.Api.Common.Errors
{
    public class HorseRacingProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _options;
        private readonly Action<ProblemDetailsContext>? _configure;
        private readonly ILogger<HorseRacingProblemDetailsFactory> _logger;

        public HorseRacingProblemDetailsFactory(
            IOptions<ApiBehaviorOptions> options,
            ILogger<HorseRacingProblemDetailsFactory> logger,
            IOptions<ProblemDetailsOptions>? problemDetailsOptions = null)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _configure = problemDetailsOptions?.Value?.CustomizeProblemDetails;
            _logger = logger;
        }

        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {
            statusCode ??= 500;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {
            ArgumentNullException.ThrowIfNull(modelStateDictionary);

            statusCode ??= 400;

            var problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            if (title != null)
            {
                // For validation problem details, don't overwrite the default title with null.
                problemDetails.Title = title;
            }

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
        {
            problemDetails.Status ??= statusCode;

            if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }

            var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
            if (traceId != null)
            {
                problemDetails.Extensions["traceId"] = traceId;
            }
            var errors = httpContext?.Items[HttpContextItemKeys.Errors] as List<Error>;
            if (errors is not null)
            {
                problemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));
            }

            var feature = httpContext!.Features.Get<IExceptionHandlerFeature>();
            if (feature?.Error is not null)
            {
                problemDetails.Extensions.Add("unhandledException.Message", feature?.Error.Message);
                problemDetails.Extensions.Add("unhandledException.StackTrace", feature?.Error.StackTrace);
                
                if (feature?.Error.InnerException is not null)
                {
                    problemDetails.Extensions.Add("unhandledException.InnerException.Message", feature?.Error.InnerException?.Message);
                    problemDetails.Extensions.Add("unhandledException.InnerException.StackTrace", feature?.Error.InnerException?.StackTrace);
                }
            }

            //_configure?.Invoke(new() { HttpContext = httpContext!, ProblemDetails = problemDetails });
        }
    }
}
