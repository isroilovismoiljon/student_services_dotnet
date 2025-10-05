using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StudentServicesWebApi.Filters;

/// <summary>
/// Document filter to explicitly set OpenAPI version
/// </summary>
public class OpenApiVersionDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Ensure proper OpenAPI document structure
        if (swaggerDoc.Info == null)
        {
            swaggerDoc.Info = new OpenApiInfo();
        }
        
        // Ensure version is properly set
        if (string.IsNullOrEmpty(swaggerDoc.Info.Version))
        {
            swaggerDoc.Info.Version = "";
        }
    }
}