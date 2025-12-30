using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Resources;
using SilvernetJSONAPI.Models;

namespace SilvernetJSONAPI.Definitions
{
    public class TenantResourceDefinition : JsonApiResourceDefinition<TenantResource, long>
    {
        public TenantResourceDefinition(IResourceGraph resourceGraph) : base(resourceGraph)
        {
        }
    }
}
