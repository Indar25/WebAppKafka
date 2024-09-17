using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using WebAppKafka.Config;
using WebAppKafka.Infrastructure;
using WebAppKafka.Services;

var builder = WebApplication.CreateBuilder(args);
// Get the configuration object (this was missing)
var configuration = builder.Configuration;


// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddDbContext<KafkaDBContext>(options => {
    options.UseSqlServer(configuration.GetConnectionString("DBConnectionString"));
});

// Register KafkaConfig and services
builder.Services.AddSingleton<KafkaConfig>();

// Register KafkaConfig and services
builder.Services.AddSingleton<KafkaConfig>();
builder.Services.AddScoped<KafkaProducerService>();

// Register KafkaConsumerService as a Hosted Service
builder.Services.AddScoped<KafkaConsumerService>();

builder.Services.AddScoped<KafkaAdminService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.RoutePrefix = "swagger"; // Sets Swagger UI at "/swagger/index.html"
    });
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
