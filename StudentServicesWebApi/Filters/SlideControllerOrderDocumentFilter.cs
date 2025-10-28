using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace StudentServicesWebApi.Filters;
public class SlideControllerOrderDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc.Paths == null)
            return;
        var controllerOrder = new List<string>
        {
            "PhotoSlides",
            "TextSlides"
        };
        var orderedPaths = new Dictionary<string, OpenApiPathItem>();
        var remainingPaths = new Dictionary<string, OpenApiPathItem>();
        foreach (var controller in controllerOrder)
        {
            var pathsForController = swaggerDoc.Paths
                .Where(p => p.Key.Contains($"/{controller}", StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.Key)
                .ToList();
            foreach (var path in pathsForController)
            {
                orderedPaths[path.Key] = path.Value;
            }
        }
        foreach (var path in swaggerDoc.Paths)
        {
            if (!orderedPaths.ContainsKey(path.Key))
            {
                remainingPaths[path.Key] = path.Value;
            }
        }
        swaggerDoc.Paths.Clear();
        foreach (var path in orderedPaths)
        {
            swaggerDoc.Paths.Add(path.Key, path.Value);
        }
        foreach (var path in remainingPaths.OrderBy(p => p.Key))
        {
            swaggerDoc.Paths.Add(path.Key, path.Value);
        }
        if (swaggerDoc.Tags != null && swaggerDoc.Tags.Any())
        {
            var orderedTags = new List<OpenApiTag>();
            foreach (var controller in controllerOrder)
            {
                var tag = swaggerDoc.Tags.FirstOrDefault(t => t.Name.Equals(controller, StringComparison.OrdinalIgnoreCase));
                if (tag != null)
                {
                    orderedTags.Add(tag);
                }
            }
            var remainingTags = swaggerDoc.Tags.Where(t => !orderedTags.Any(ot => ot.Name.Equals(t.Name, StringComparison.OrdinalIgnoreCase)))
                                                 .OrderBy(t => t.Name);
            orderedTags.AddRange(remainingTags);
            swaggerDoc.Tags = orderedTags;
        }
    }
}
