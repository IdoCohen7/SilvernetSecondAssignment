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

public class TenantResourceService : JsonApiResourceService<TenantResource, long>
{
    private readonly WriteDbContext _writeDb;

    public TenantResourceService(
        WriteDbContext writeDb,
        IResourceRepositoryAccessor repositoryAccessor,
        IQueryLayerComposer queryLayerComposer,
        IPaginationContext paginationContext,
        IJsonApiOptions options,
        ILoggerFactory loggerFactory,
        IJsonApiRequest request,
        IResourceChangeTracker<TenantResource> resourceChangeTracker,
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
    }

    public override async Task<TenantResource> CreateAsync(TenantResource resource, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("Resource", "Tenant"))
        using (LogContext.PushProperty("Operation", "Create"))
        {
            try
            {
                if (resource == null) throw new InvalidOperationException("Tenant payload is missing.");

                Validations.ValidateIsraeliPhone(resource.Phone);

                Log.Information("Creating tenant. Name={Name}, Email={Email}, Phone={Phone}",
                    resource.Name, resource.Email, Validations.MaskPhone(resource.Phone));

                _writeDb.Tenants.Add(resource);
                await _writeDb.SaveChangesAsync(cancellationToken);

                Log.Information("Tenant created successfully. TenantId={TenantId}", resource.Id);

                return resource;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Create tenant failed. Email={Email}", resource?.Email);
                throw;
            }
        }
    }

    public override async Task<TenantResource?> UpdateAsync(long id, TenantResource resource, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("Resource", "Tenant"))
        using (LogContext.PushProperty("Operation", "Update"))
        using (LogContext.PushProperty("TenantId", id))
        {
            try
            {
                if (resource == null) throw new InvalidOperationException("Tenant payload is missing.");

                if (resource.Phone != null)
                    Validations.ValidateIsraeliPhone(resource.Phone);

                Log.Information("Updating tenant. TenantId={TenantId}, Email={Email}, Phone={Phone}",
                    id, resource.Email, Validations.MaskPhone(resource.Phone));

                var existing = await _writeDb.Tenants.FindAsync(new object[] { id }, cancellationToken);
                if (existing == null)
                {
                    Log.Warning("Tenant not found. TenantId={TenantId}", id);
                    return null;
                }

                if (!string.IsNullOrEmpty(resource.Name)) existing.Name = resource.Name;
                if (!string.IsNullOrEmpty(resource.Email)) existing.Email = resource.Email;
                if (!string.IsNullOrEmpty(resource.Phone)) existing.Phone = resource.Phone;

                await _writeDb.SaveChangesAsync(cancellationToken);

                Log.Information("Tenant updated successfully. TenantId={TenantId}", id);

                return existing;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Update tenant failed. TenantId={TenantId}", id);
                throw;
            }
        }
    }

    public override async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("Resource", "Tenant"))
        using (LogContext.PushProperty("Operation", "Delete"))
        using (LogContext.PushProperty("TenantId", id))
        {
            try
            {
                Log.Information("Deleting tenant. TenantId={TenantId}", id);

                var existing = await _writeDb.Tenants.FindAsync(new object[] { id }, cancellationToken);
                if (existing == null)
                {
                    Log.Warning("Tenant not found. TenantId={TenantId}", id);
                    return;
                }

                _writeDb.Tenants.Remove(existing);
                await _writeDb.SaveChangesAsync(cancellationToken);

                Log.Information("Tenant deleted successfully. TenantId={TenantId}", id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Delete tenant failed. TenantId={TenantId}", id);
                throw;
            }
        }
    }
}
