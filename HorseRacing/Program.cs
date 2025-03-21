using Asp.Versioning.ApiExplorer;
using HorseRacing.Api;
using HorseRacing.Api.Extensions;
using HorseRacing.Api.Middleware;
using HorseRacing.Application;
using HorseRacing.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation(builder)
    .AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Logging,
    new HorseRacing.Domain.Common.DependencyConfiguration.InfrastructureConfiguration()
    {
        EnableAuthorizationConfiguration = true,
        EnableCachingConfiguration = true
    }, out string selectedConnectionString, out string selectedProvider);

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsProduction())
{
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
}

app.MigrateDatabase();

app.UseExceptionHandler("/error");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension(provider);
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//try
//{
//    app.UseStaticFiles();
//    app.MapFallbackToFile("index.html");
//}
//catch { }

app.UseRouting();
app.UseCors("AllowFrontend");
// Отключение cors
//app.UseCors(x => { x.AllowAnyMethod().AllowAnyHeader()
//    .WithExposedHeaders(new string[] { "Content-Disposition" })
//    .SetIsOriginAllowed(origin => true) // allow any origin
//    .AllowCredentials(); // allow credentials
//});

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidateAuthentication>();
app.MapControllers();

//app.MapHub<CommonServerHub>(Deb.Infrastructure.DependencyInjection.CommonServerHub);

app.Run();
