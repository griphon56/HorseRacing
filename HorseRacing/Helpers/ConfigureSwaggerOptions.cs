using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HorseRacing.Api.Helpers
{
    /// <summary>
	/// Параметры настройки swagger.
	/// </summary>
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "Horse racing",
                Version = description.ApiVersion.ToString(),
                Description = "Web service for game",
                Contact = new OpenApiContact
                {
                    Name = "IT Department",
                    Email = "developerm@mail.ru",
                    Url = new Uri("https://debtmc.ru/support")
                }
            };

            if (description.IsDeprecated)
                info.Description += " <strong>This API version of \"Horse racing\" has been deprecated.</strong>";

            return info;
        }
    }
}
