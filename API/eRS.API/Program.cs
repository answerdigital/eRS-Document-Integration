using AutoMapper;
using AutoMapper.EquivalencyExpression;
using eRS.Data;
using Microsoft.EntityFrameworkCore;
using Playground.Service.Mappers;

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

app.UseAuthorization();

app.MapControllers();

app.Run();
