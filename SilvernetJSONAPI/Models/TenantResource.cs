using JsonApiDotNetCore.Controllers.Annotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SilvernetJSONAPI.Models
{
    [Resource(PublicName = "tenants", GenerateControllerEndpoints = JsonApiDotNetCore.Controllers.JsonApiEndpoints.None)]
    [DisableRoutingConvention]
    public class TenantResource : Identifiable<long>
    {
        [Attr, Required, MaxLength(20)]
        public string Name { get; set; } = default!;

        [Attr, Required, MaxLength(50), EmailAddress]
        public string Email { get; set; } = default!;

        [Attr, Required, MaxLength(12), Phone]
        public string Phone { get; set; } = default!;

        [Attr]
        public DateTime CreationDate { get; set; }

        [HasMany]
        public ICollection<UserResource> Users { get; set; } = new List<UserResource>();
    }
}
