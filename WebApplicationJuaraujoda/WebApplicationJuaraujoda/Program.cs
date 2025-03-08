using Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models; // Important pour Swashbuckle

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// Utilisez PlayerServiceStub pour le débogage.
builder.Services.AddScoped<IPlayerService, PlayerServiceStub>();

// Configure Swashbuckle
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});


builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Gardez UseSwagger et UseSwaggerUI *même en production* avec CodeFirst.
app.UseSwagger(); // Swashbuckle
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Sert l'UI à la racine (/)
});

app.UseHttpsRedirection(); // Décommentez pour CodeFirst

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();