using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StudentServicesWebApi.Filters;

/// <summary>
/// Document filter to control the order of slide controllers in Swagger UI
/// Ensures PhotoSlides appear before TextSlides
/// </summary>
public class SlideControllerOrderDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc.Paths == null)
            return;

        // Define the desired order for slide controllers
        var controllerOrder = new List<string>
        {
            "PhotoSlides",
            "TextSlides"
        };

        // Create ordered dictionary to maintain order
        var orderedPaths = new Dictionary<string, OpenApiPathItem>();
        var remainingPaths = new Dictionary<string, OpenApiPathItem>();

        // First, add slide controllers in the desired order
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

        // Add remaining paths (non-slide controllers)
        foreach (var path in swaggerDoc.Paths)
        {
            if (!orderedPaths.ContainsKey(path.Key))
            {
                remainingPaths[path.Key] = path.Value;
            }
        }

        // Replace the paths collection with the ordered one
        swaggerDoc.Paths.Clear();

        // Add slide controllers first
        foreach (var path in orderedPaths)
        {
            swaggerDoc.Paths.Add(path.Key, path.Value);
        }

        // Add remaining controllers
        foreach (var path in remainingPaths.OrderBy(p => p.Key))
        {
            swaggerDoc.Paths.Add(path.Key, path.Value);
        }

        // Also order the tags to ensure consistent grouping
        if (swaggerDoc.Tags != null && swaggerDoc.Tags.Any())
        {
            var orderedTags = new List<OpenApiTag>();
            
            // Add slide tags first in desired order
            foreach (var controller in controllerOrder)
            {
                var tag = swaggerDoc.Tags.FirstOrDefault(t => t.Name.Equals(controller, StringComparison.OrdinalIgnoreCase));
                if (tag != null)
                {
                    orderedTags.Add(tag);
                }
            }

            // Add remaining tags
            var remainingTags = swaggerDoc.Tags.Where(t => !orderedTags.Any(ot => ot.Name.Equals(t.Name, StringComparison.OrdinalIgnoreCase)))
                                                 .OrderBy(t => t.Name);
            
            orderedTags.AddRange(remainingTags);
            
            swaggerDoc.Tags = orderedTags;
        }
    }
}