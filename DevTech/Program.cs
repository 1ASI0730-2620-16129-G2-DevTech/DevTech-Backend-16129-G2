using DevTech.LaundryOperations.Application.Acl;
using DevTech.LaundryOperations.Application.CommandServices;
using DevTech.LaundryOperations.Application.Internal.CommandServices;
using DevTech.LaundryOperations.Application.Internal.QueryServices;
using DevTech.LaundryOperations.Application.QueryServices;
using DevTech.LaundryOperations.Domain.Repositories;
using DevTech.LaundryOperations.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.LaundryOperations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using DevTech.LaundryOperations.Interfaces.Acl;
using DevTech.OrderManagement.Infrastructure.Configuration;
using DevTech.PickupsDeliveries.Infrastructure.Configuration;
using DevTech.PickupsDeliveries.Interfaces.Rest.Transform;
using DevTech.Resources;
using DevTech.Shared.Domain.Repositories;
using DevTech.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using DevTech.Shared.Infrastructure.Pipeline.Middleware.Components;
using DevTech.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------------------------
// Web API
// ---------------------------------------------------------------------------------------------

// Lower case URLs and kebab case controller routes (for example: api/v1/laundry-orders)
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource)));

// Localization: Shared/Resources/Errors/ErrorMessages(.es).resx and Resources/SharedResource(.en/.es).resx
builder.Services.AddLocalization();

// Localized RFC 7807 problem details for unhandled exceptions
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddScoped<ProblemDetailsFactory>();

// OpenAPI document (with the localized descriptions of the Pickups and Deliveries endpoints)
builder.Services.AddOpenApi(options =>
{
    options.AddOperationTransformer(LocalizedOpenApiAssembler.TransformOperationAsync);
    options.AddSchemaTransformer(LocalizedOpenApiAssembler.TransformSchemaAsync);
});

// Clock used by the domain rules that depend on the current date and time
builder.Services.AddSingleton(TimeProvider.System);

// ---------------------------------------------------------------------------------------------
// Shared kernel
// ---------------------------------------------------------------------------------------------

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                           "Connection string 'DefaultConnection' was not found. Add it to appsettings.Development.json.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(connectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ---------------------------------------------------------------------------------------------
// Laundry Operations bounded context
// ---------------------------------------------------------------------------------------------

// Repositories
builder.Services.AddScoped<ILaundryOrderRepository, LaundryOrderRepository>();
builder.Services.AddScoped<ILaundryResourceRepository, LaundryResourceRepository>();
builder.Services.AddScoped<IWashingCycleRepository, WashingCycleRepository>();

// Command Services
builder.Services.AddScoped<
    ILaundryOrderCommandService,
    LaundryOrderCommandService>();

builder.Services.AddScoped<
    ILaundryResourceCommandService,
    LaundryResourceCommandService>();

// Query Services
builder.Services.AddScoped<ILaundryOrderQueryService, LaundryOrderQueryService>();
builder.Services.AddScoped<ILaundryResourceQueryService, LaundryResourceQueryService>();
builder.Services.AddScoped<IWashingCycleQueryService, WashingCycleQueryService>();

// Facade offered to the other bounded contexts
builder.Services.AddScoped<ILaundryOperationsContextFacade, LaundryOperationsContextFacade>();

// ---------------------------------------------------------------------------------------------
// Order Management bounded context
// ---------------------------------------------------------------------------------------------

// Repository, command and query services, REST assembler and the facade offered to other contexts
builder.Services.AddOrderManagement();

// ---------------------------------------------------------------------------------------------
// Pickups and Deliveries bounded context
// ---------------------------------------------------------------------------------------------

// Repository, command and query services and the anti-corruption layer towards Order Management
builder.Services.AddPickupsDeliveries();

var app = builder.Build();

// ---------------------------------------------------------------------------------------------
// Database
// ---------------------------------------------------------------------------------------------

// Apply pending EF Core migrations at startup.
// In Development, run with --init-db to also load the Laundry Operations seed data.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();

    if (app.Environment.IsDevelopment() && args.Contains("--init-db"))
        await DevelopmentSeed.InitializeAsync(context);
}

// ---------------------------------------------------------------------------------------------
// HTTP request pipeline
// ---------------------------------------------------------------------------------------------

app.UseExceptionHandler();

var supportedCultures = new[] { "en", "es" };
app.UseRequestLocalization(options => options
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
