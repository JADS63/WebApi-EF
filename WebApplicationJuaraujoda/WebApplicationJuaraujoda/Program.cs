using NSwag;
using Services;
using Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

builder.Services.AddScoped<IPlayerService, PlayerServiceModel>();

builder.Services.AddOpenApiDocument(options =>
{
    options.PostProcess = document =>
    {
        document.Info = new NSwag.OpenApiInfo
        {
            Version = "v1",
            Title = "My API",
            Description = "Juaraujoda API",
            TermsOfService = "https://terms.of.service.fr/",
            Contact = new NSwag.OpenApiContact
            {
                Name = "Julien Araujo Da Silva",
                Url = "https://code.lord.fr"
            },
            License = new NSwag.OpenApiLicense
            {
                Name = "Julien Araujo Da Silva",
                Url = "https://license.fr"
            }
        };
        // Définissez l'URL du serveur en HTTP
        document.Servers.Clear();
        document.Servers.Add(new OpenApiServer() { Url = "http://localhost:7001" });
    };
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Dans un environnement de développement, activez Swagger (en HTTP)
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

// Supprimez ou commentez l'appel à HTTPS redirection pour rester en HTTP
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
