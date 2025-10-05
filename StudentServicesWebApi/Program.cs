using StudentServicesWebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure all services using extension methods
builder.Services
    .AddDatabaseContext(builder.Configuration)
    .AddAutoMapperConfiguration()
    .AddFluentValidationConfiguration()
    .AddApplicationConfiguration(builder.Configuration)
    .AddApplicationServices()
    .AddJwtAuthentication(builder.Configuration)
    .AddHostedServices()
    .AddCorsConfiguration()
    .AddSwaggerConfiguration();

var app = builder.Build();

// Seed the database and configure middleware pipeline
await app.SeedDatabaseAsync();
app.UseApplicationMiddleware();

app.Run();
