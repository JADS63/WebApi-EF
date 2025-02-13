using NSwag;
using Services; 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddScoped<IPlayerService, PlayerServiceStub>();

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
        document.Servers.Add(new OpenApiServer() { Url = $"https://my-api-servers.fr/" });
    };
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
