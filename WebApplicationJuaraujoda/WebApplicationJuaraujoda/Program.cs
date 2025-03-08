using NSwag;
// PAS NSwag.AspNetCore  <--  On n'utilise PAS les extensions NSwag pour l'UI
using Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models; // <-- Important pour Swashbuckle

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// Utilisez PlayerServiceStub pour le débogage.
builder.Services.AddScoped<IPlayerService, PlayerServiceStub>();

// Configure NSwag pour générer le document OpenAPI (MAIS PAS L'UI)
builder.Services.AddOpenApiDocument(config =>
{
    config.PostProcess = document =>
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
                Url = "https://code.lord.fr/"
            },
            License = new NSwag.OpenApiLicense
            {
                Name = "Julien Araujo Da Silva",
                Url = "https://license.fr/"
            }
        };
        document.Servers.Clear();
    };
});

// Configure Swashbuckle pour l'UI (UNIQUEMENT L'UI)
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" }); // Information de base

    // Très important : Dites à Swashbuckle de NE PAS générer le document
    // On utilise celui de NSwag
    c.IgnoreObsoleteActions(); // Pour éviter les conflits potentiels
    c.DocInclusionPredicate((_, _) => false); // <-- Empêche la génération du document
});


builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOpenApi(); // NSwag (pour le document JSON)
app.UseSwaggerUI(c =>  // Swashbuckle (pour l'UI)
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); // Utilise le document généré par NSwag
    c.RoutePrefix = string.Empty; // Sert l'UI à la racine (/)
});
app.UseHttpsRedirection(); 

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();