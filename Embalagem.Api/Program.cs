using System.Text.Json;
using Embalagem.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
            policy.WithOrigins("http://localhost:8080")
                .AllowAnyMethod()
                .AllowAnyHeader());
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.PropertyNameCaseInsensitive = true;
    o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

var connectionString = builder.Configuration.GetConnectionString("EmbalagemRepositorySqlServer")
                       ?? throw new NullReferenceException("Connection string não encontrada no Application Settings.");
builder.Services.AddScoped<IRepository>(_ => new SqlServerDapper(connectionString));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowLocalhost");
    app.MapOpenApi();
    app.UseSwaggerUI(o => { o.SwaggerEndpoint("/openapi/v1.json", "Swagger"); });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();