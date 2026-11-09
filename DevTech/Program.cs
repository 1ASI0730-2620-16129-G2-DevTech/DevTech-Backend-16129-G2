using Acme.Center.Platform.Resources.Errors;
using Acme.Center.Platform.Resources.Shared;
using Acme.Center.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using Acme.Center.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Cortex.Mediator.DependencyInjection;
using DevTech.Shared.Domain.Repositories;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using DevTech.SubscriptionPayment.Application.CommandServices;
using DevTech.SubscriptionPayment.Application.Internal.CommandServices;
using DevTech.SubscriptionPayment.Application.Internal.OutboundServices;
using DevTech.SubscriptionPayment.Application.Internal.QueryServices;
using DevTech.SubscriptionPayment.Application.QueryServices;
using DevTech.SubscriptionPayment.Domain.Repositories;
using DevTech.SubscriptionPayment.Infrastructure.PaymentGateway;
using DevTech.SubscriptionPayment.Infrastructure.PaymentGateway.External;
using DevTech.SubscriptionPayment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

// Add ProblemDetails services
builder.Services.AddProblemDetails();

// Add OpenAPI/Scalar documentation services
builder.Services.AddOpenApi();

// Add Database Connection

// Configure Database Context and route EF logs through the app logger pipeline.
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    // Azure SQL Database (SQL Server). EnableRetryOnFailure retries transient errors, for example
    // while the serverless database is resuming after an automatic pause.
    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure())
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .AddInterceptors(serviceProvider.GetRequiredService<DispatchDomainEventsInterceptor>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

builder.Services.AddLocalization();

// Explicitly register IStringLocalizer for ErrorMessages and Commons
builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();

builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();

// Register the custom ProblemDetailsFactory
builder.Services.AddSingleton<ProblemDetailsFactory>();

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Dependency Injection

// Subscription & Payment Bounded Context
builder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<ISubscriptionPlanCommandService, SubscriptionPlanCommandService>();
builder.Services.AddScoped<ISubscriptionPlanQueryService, SubscriptionPlanQueryService>();

builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();

builder.Services.AddScoped<IPaymentCommandService, PaymentCommandService>();
builder.Services.AddScoped<IPaymentQueryService, PaymentQueryService>();

// Payment Gateway: port (IPaymentGateway) -> adapter -> anti-corruption layer -> external provider
builder.Services.AddScoped<IExternalPaymentProviderApi, SimulatedExternalPaymentProviderApi>();
builder.Services.AddScoped<PaymentGatewayAcl>();
builder.Services.AddScoped<IPaymentGateway, PaymentGatewayAdapter>();

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DispatchDomainEventsInterceptor>();

// Add Cortex Mediator for Event Handling
builder.Services.AddCortexMediator(
    [typeof(Program)]);

var app = builder.Build();

// Create the database schema on startup if it does not exist yet.
// Replace with context.Database.Migrate() once the team adds EF Core migrations.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Apply CORS policy
app.UseCors("AllowAllPolicy");

// Configure the HTTP request pipeline
app.UseExceptionHandler();

var supportedCultures = new [] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    // Enable OpenAPI/Scalar documentation endpoints in development environment
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
