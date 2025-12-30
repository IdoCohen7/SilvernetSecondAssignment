using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Queries.Expressions;
using JsonApiDotNetCore.Resources;
using Microsoft.AspNetCore.Http;
using SilvernetJSONAPI.Models;

namespace SilvernetJSONAPI.Definitions;

public class UserResourceDefinition : JsonApiResourceDefinition<UserResource, long>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserResourceDefinition(IResourceGraph resourceGraph, IHttpContextAccessor httpContextAccessor)
        : base(resourceGraph)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override FilterExpression OnApplyFilter(FilterExpression? existingFilter)
    {
        var routeValues = _httpContextAccessor.HttpContext?.Request.RouteValues;

        if (routeValues == null ||
            !routeValues.TryGetValue("tenantId", out var tenantIdObj) ||
            !long.TryParse(tenantIdObj?.ToString(), out var tenantId))
        {
            return existingFilter!;
        }

        var tenantIdAttribute = ResourceType.Attributes.Single(a =>
            a.Property.Name == nameof(UserResource.TenantId));

        var tenantFilter = new ComparisonExpression(
            ComparisonOperator.Equals,
            new ResourceFieldChainExpression(tenantIdAttribute),
            new LiteralConstantExpression(tenantId)
        );

        return existingFilter == null
            ? tenantFilter
            : new LogicalExpression(LogicalOperator.And, new[] { tenantFilter, existingFilter });
    }
}
