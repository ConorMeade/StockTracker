/*

    Author: Conor Meade

    Bootstrapped using Visual Studio.

    GET Endpoint for getting stock prices using iexcloud in a given date range.

    Date: 4/21/2023

*/

using StockTracker;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add specific DataService for getting iex data, DI with IDataService
builder.Services.AddScoped<IDataService, DataService>();


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add swagger service, makes executing endpoints easier
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "StockData API",
            Description = "An ASP.NET Core Web API for generating historical stock price data, chiefly daily returns",
        });

        // This ensures we get the proper xml file on different OS
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ensure we are operating on HTTPS
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
