using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SilvernetJSONAPI.Swagger
{
    // this class configures the project to receive the expeceted "application/vnd.api+json" format that json:api expects in swagger
    public class JsonApiContentTypeDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var path in swaggerDoc.Paths.Values)
            {
                foreach (var op in path.Operations.Values)
                {
                    if (op.RequestBody?.Content != null)
                    {
                        if (op.RequestBody.Content.TryGetValue("application/json", out var json))
                        {
                            op.RequestBody.Content["application/vnd.api+json"] = json;
                            op.RequestBody.Content.Remove("application/json");
                        }

                        if (op.RequestBody.Content.TryGetValue("application/*+json", out var wildcard))
                        {
                            op.RequestBody.Content["application/vnd.api+json"] = wildcard;
                            op.RequestBody.Content.Remove("application/*+json");
                        }
                    }

                    foreach (var response in op.Responses.Values)
                    {
                        if (response.Content == null) continue;

                        if (response.Content.TryGetValue("application/json", out var jsonResp))
                        {
                            response.Content["application/vnd.api+json"] = jsonResp;
                            response.Content.Remove("application/json");
                        }

                        if (response.Content.TryGetValue("application/*+json", out var wildcardResp))
                        {
                            response.Content["application/vnd.api+json"] = wildcardResp;
                            response.Content.Remove("application/*+json");
                        }
                    }
                }
            }
        }
    }
}
