using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilvernetJSONAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SilvernetJSONAPI.Controllers
{
    [Authorize]
    public class TenantsController : JsonApiController<TenantResource, long>
    {
        public TenantsController(
            IJsonApiOptions options,
            IResourceGraph resourceGraph,
            ILoggerFactory loggerFactory,
            IResourceService<TenantResource, long> resourceService)
            : base(options, resourceGraph, loggerFactory, resourceService)
        {
        }

        [HttpGet]
        public override Task<IActionResult> GetAsync(CancellationToken cancellationToken)
            => base.GetAsync(cancellationToken);

        [HttpGet("{id:long}")]
        public override Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
            => base.GetAsync(id, cancellationToken);

        [HttpPost]
        public override Task<IActionResult> PostAsync([FromBody] TenantResource resource, CancellationToken cancellationToken)
            => base.PostAsync(resource, cancellationToken);

        [HttpPatch("{id:long}")]
        public override Task<IActionResult> PatchAsync(long id, [FromBody] TenantResource resource, CancellationToken cancellationToken)
            => base.PatchAsync(id, resource, cancellationToken);

        [HttpDelete("{id:long}")]
        public override Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
            => base.DeleteAsync(id, cancellationToken);
    }
}
