using DevTech.LaundryOperations.Application.CommandServices;
using DevTech.LaundryOperations.Application.Internal.CommandServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Dependency Injection

// Laundry Operations Bounded Context

// Command Services
builder.Services.AddScoped<
    ILaundryOrderCommandService,
    LaundryOrderCommandService>();

builder.Services.AddScoped<
    ILaundryResourceCommandService,
    LaundryResourceCommandService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();