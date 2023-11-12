using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shopping.Aggregator.Services.Implementations;
using Shopping.Aggregator.Services.Interfaces;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<ICatalogService, CatalogService>(client => 
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]));
builder.Services.AddHttpClient<IBasketService, BasketService>(client =>
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]));
builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
