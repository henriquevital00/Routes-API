
using Microsoft.EntityFrameworkCore;
using Routes.Models.Data;
using Routes.Models.Interfaces;
using Routes.Repositories;
using Routes.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IRoutesService, RoutesService>();
builder.Services.AddScoped<IDijkstraService, DijkstraService>();
builder.Services.AddScoped<IRoutesRepositorie, RoutesRepositorie>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connecting Database
builder.Services.AddDbContext<RoutesContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
