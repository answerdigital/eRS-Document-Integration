using AutoMapper;
using AutoMapper.EquivalencyExpression;
using eRS.Data;
using eRS.Services.Interfaces;
using eRS.Services.Services;
using Microsoft.EntityFrameworkCore;
using Playground.Service.Mappers;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<eRSContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LaptopConnection"));
});

builder.Services.Configure<JsonSerializerOptions>(jsonSerializerOptions =>
{
    jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    jsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

builder.Services.AddTransient<IAuditService, AuditService>();
builder.Services.AddTransient<IWorklistService, WorklistService>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddCollectionMappers();
    mc.AddProfile(new MappingProfiles());
});
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
