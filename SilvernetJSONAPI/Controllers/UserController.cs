using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Controllers.Annotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilvernetJSONAPI.Models;

namespace SilvernetJSONAPI.Controllers;

[Authorize]
[DisableRoutingConvention]
[Route("api/v1/tenants/{tenantId:long}/users")]
public class UsersController : JsonApiController<UserResource, long>
{
    public UsersController(
        IJsonApiOptions options,
        IResourceGraph resourceGraph,
        ILoggerFactory loggerFactory,
        IResourceService<UserResource, long> resourceService)
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
    public override Task<IActionResult> PostAsync([FromBody] UserResource resource, CancellationToken cancellationToken)
    {
        if (!TryGetTenantId(out var tenantId))
        {
            return Task.FromResult<IActionResult>(BadRequest());
        }

        resource.TenantId = tenantId;
        return base.PostAsync(resource, cancellationToken);
    }

    [HttpPatch("{id:long}")]
    public override Task<IActionResult> PatchAsync(long id, [FromBody] UserResource resource, CancellationToken cancellationToken)
    {
        if (!TryGetTenantId(out var tenantId))
        {
            return Task.FromResult<IActionResult>(BadRequest());
        }

        resource.TenantId = tenantId;
        return base.PatchAsync(id, resource, cancellationToken);
    }

    [HttpDelete("{id:long}")]
    public override Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        => base.DeleteAsync(id, cancellationToken);

    private bool TryGetTenantId(out long tenantId)
    {
        tenantId = default;

        var tenantIdValue = HttpContext?.Request?.RouteValues["tenantId"]?.ToString();
        return long.TryParse(tenantIdValue, out tenantId);
    }
}
