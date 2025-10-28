using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace StudentServicesWebApi.Filters;
public class OpenApiVersionDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc.Info == null)
        {
            swaggerDoc.Info = new OpenApiInfo();
        }
        if (string.IsNullOrEmpty(swaggerDoc.Info.Version))
        {
            swaggerDoc.Info.Version = "";
        }
    }
}
