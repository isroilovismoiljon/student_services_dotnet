using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Extensions;

public static class MiddlewareExtensions
{
    public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        await dataSeeder.SeedAsync();
        
        return app;
    }
    public static WebApplication UseSwaggerConfiguration(this WebApplication app)
    {
        app.UseSwagger(); // Use default settings for OpenAPI 3.0
        
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Services API");
            c.RoutePrefix = "swagger";
            c.DisplayRequestDuration();
            c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            c.DefaultModelsExpandDepth(-1);
            c.EnableDeepLinking();
        });

        return app;
    }
    public static WebApplication UseApplicationMiddleware(this WebApplication app)
    {
        // Configure Swagger (enabled for all environments for testing purposes)
        app.UseSwaggerConfiguration();

        // CORS middleware
        app.UseCors();

        // Static files middleware for serving uploaded images
        app.UseStaticFiles();
        
        // Security and routing middleware
        app.UseAuthentication();
        app.UseAuthorization();

        // Map controllers
        app.MapControllers();

        return app;
    }
}