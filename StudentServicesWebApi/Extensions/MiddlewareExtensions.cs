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
        app.UseSwagger(); 
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
        app.UseSwaggerConfiguration();
        app.UseCors();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}
