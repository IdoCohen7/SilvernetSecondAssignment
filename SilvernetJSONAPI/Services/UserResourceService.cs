using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Middleware;
using JsonApiDotNetCore.Queries;
using JsonApiDotNetCore.Repositories;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Services;
using Serilog;
using Serilog.Context;
using SilvernetJSONAPI.Data;
using SilvernetJSONAPI.Models;
using SilvernetJSONAPI.Validators;

namespace SilvernetJSONAPI.Services;

public class UserResourceService : JsonApiResourceService<UserResource, long>
{
    private readonly WriteDbContext _writeDb;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserResourceService(
        WriteDbContext writeDb,
        IHttpContextAccessor httpContextAccessor,
        IResourceRepositoryAccessor repositoryAccessor,
        IQueryLayerComposer queryLayerComposer,
        IPaginationContext paginationContext,
        IJsonApiOptions options,
        ILoggerFactory loggerFactory,
        IJsonApiRequest request,
        IResourceChangeTracker<UserResource> resourceChangeTracker,
        IResourceDefinitionAccessor resourceDefinitionAccessor)
        : base(
            repositoryAccessor,
            queryLayerComposer,
            paginationContext,
            options,
            loggerFactory,
            request,
            resourceChangeTracker,
            resourceDefinitionAccessor)
    {
        _writeDb = writeDb;
        _httpContextAccessor = httpContextAccessor;
    }

    private long GetTenantIdFromRoute()
    {
        var routeValues = _httpContextAccessor.HttpContext?.Request.RouteValues;
        if (routeValues != null &&
            routeValues.TryGetValue("tenantId", out var tenantIdObj) &&
            long.TryParse(tenantIdObj?.ToString(), out var tenantId))
        {
            return tenantId;
        }
        throw new InvalidOperationException("TenantId not found in route.");
    }

    public override async Task<UserResource> CreateAsync(UserResource resource, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("Resource", "User"))
        using (LogContext.PushProperty("Operation", "Create"))
        {
            try
            {
                if (resource == null) throw new InvalidOperationException("User payload is missing.");

                // Get TenantId from route - IGNORE what's in the resource
                var tenantId = GetTenantIdFromRoute();
                resource.TenantId = tenantId;

                Validations.ValidateIsraeliId(resource.IdNumber);
                Validations.ValidateIsraeliPhone(resource.Phone);

                Log.Information("Creating user. TenantId={TenantId}, Email={Email}, IdNumber={IdNumber}, Phone={Phone}",
                    resource.TenantId, resource.Email, Validations.MaskId(resource.IdNumber), Validations.MaskPhone(resource.Phone));

                resource.Tenant = null!;

                _writeDb.Users.Add(resource);
                await _writeDb.SaveChangesAsync(cancellationToken);

                Log.Information("User created successfully. UserId={UserId}, TenantId={TenantId}", resource.Id, resource.TenantId);

                return resource;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Create user failed. TenantId={TenantId}, Email={Email}", resource?.TenantId, resource?.Email);
                throw;
            }
        }
    }

    public override async Task<UserResource?> UpdateAsync(long id, UserResource resource, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("Resource", "User"))
        using (LogContext.PushProperty("Operation", "Update"))
        using (LogContext.PushProperty("UserId", id))
        {
            try
            {
                if (resource == null) throw new InvalidOperationException("User payload is missing.");

                // get TenantId from route
                var tenantId = GetTenantIdFromRoute();

                if (resource.IdNumber != null)
                    Validations.ValidateIsraeliId(resource.IdNumber);

                if (resource.Phone != null)
                    Validations.ValidateIsraeliPhone(resource.Phone);

                Log.Information("Updating user. UserId={UserId}, TenantId={TenantId}, Email={Email}, IdNumber={IdNumber}, Phone={Phone}",
                    id, tenantId, resource.Email, Validations.MaskId(resource.IdNumber), Validations.MaskPhone(resource.Phone));

                var existing = await _writeDb.Users.FindAsync(new object[] { id }, cancellationToken);
                if (existing == null)
                {
                    Log.Warning("User not found. UserId={UserId}", id);
                    return null;
                }

                // verify user belongs to this tenant
                if (existing.TenantId != tenantId)
                {
                    Log.Warning("User does not belong to tenant. UserId={UserId}, TenantId={TenantId}", id, tenantId);
                    return null;
                }

                if (!string.IsNullOrEmpty(resource.FirstName)) existing.FirstName = resource.FirstName;
                if (!string.IsNullOrEmpty(resource.LastName)) existing.LastName = resource.LastName;
                if (!string.IsNullOrEmpty(resource.Phone)) existing.Phone = resource.Phone;
                if (!string.IsNullOrEmpty(resource.Email)) existing.Email = resource.Email;
                if (!string.IsNullOrEmpty(resource.IdNumber)) existing.IdNumber = resource.IdNumber;

                await _writeDb.SaveChangesAsync(cancellationToken);

                Log.Information("User updated successfully. UserId={UserId}", id);

                return existing;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Update user failed. UserId={UserId}", id);
                throw;
            }
        }
    }

    public override async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("Resource", "User"))
        using (LogContext.PushProperty("Operation", "Delete"))
        using (LogContext.PushProperty("UserId", id))
        {
            try
            {
                var tenantId = GetTenantIdFromRoute();

                Log.Information("Deleting user. UserId={UserId}, TenantId={TenantId}", id, tenantId);

                var existing = await _writeDb.Users.FindAsync(new object[] { id }, cancellationToken);
                if (existing == null)
                {
                    Log.Warning("User not found. UserId={UserId}", id);
                    return;
                }

                // verify user belongs to this tenant
                if (existing.TenantId != tenantId)
                {
                    Log.Warning("User does not belong to tenant. UserId={UserId}, TenantId={TenantId}", id, tenantId);
                    return;
                }

                _writeDb.Users.Remove(existing);
                await _writeDb.SaveChangesAsync(cancellationToken);

                Log.Information("User deleted successfully. UserId={UserId}", id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Delete user failed. UserId={UserId}", id);
                throw;
            }
        }
    }
}
