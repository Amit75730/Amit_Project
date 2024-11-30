using EdiWebAPI;
using EdiWebAPI.Models;
using EdiWebAPI.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Load configuration settings from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add Cosmos DB context
builder.Services.AddDbContext<CosmosDbContext>(options =>
    options.UseCosmos(
        builder.Configuration.GetValue<string>("CosmosDb:Endpoint"),
        builder.Configuration.GetValue<string>("CosmosDb:Key"),
        builder.Configuration.GetValue<string>("CosmosDb:DatabaseName")));

// // Register the Azure Service Bus Client
// builder.Services.AddSingleton<ServiceBusClient>(sp =>
//     new ServiceBusClient(builder.Configuration["AzureServiceBus:ConnectionString"]));

builder.Services.AddSingleton<ServiceBusSenderService>();
builder.Services.AddSingleton<ServiceBusReceiverService>();


// Add controllers and logging
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular app origin
              .AllowAnyHeader()                    // Allow any headers
              .AllowAnyMethod();                   // Allow any HTTP methods
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAngularApp");

app.UseAuthorization();
app.MapControllers();

// Start receiving Service Bus messages asynchronously in the background
var serviceBusReceiverService = app.Services.GetRequiredService<ServiceBusReceiverService>();
_ = serviceBusReceiverService.ReceiveMessagesAsync(); // Start listening for messages in the background


app.Run();
