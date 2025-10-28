using StudentServicesWebApi.Extensions;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
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
await app.SeedDatabaseAsync();
app.UseApplicationMiddleware();
app.Run();
