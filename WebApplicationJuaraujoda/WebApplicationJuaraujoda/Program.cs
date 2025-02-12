using NSwag;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument(options => {
    options.PostProcess = document =>
    {
        document.Info = new NSwag.OpenApiInfo
        {
            Version = "v1",
            Title = "My API ",
            Description = "Juaraujoda API ",
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
