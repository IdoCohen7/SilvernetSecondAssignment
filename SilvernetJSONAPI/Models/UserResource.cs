using JsonApiDotNetCore.Controllers.Annotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SilvernetJSONAPI.Models
{
    [Resource(PublicName = "users", GenerateControllerEndpoints = JsonApiDotNetCore.Controllers.JsonApiEndpoints.None)]
    public class UserResource : Identifiable<long>
    {
        [Attr, Required, MaxLength(10)]
        public string FirstName { get; set; } = default!;

        [Attr, Required, MaxLength(10)]
        public string LastName { get; set; } = default!;

        [Attr, Required, MaxLength(12), Phone]
        public string Phone { get; set; } = default!;

        [Attr, Required, MaxLength(50), EmailAddress]
        public string Email { get; set; } = default!;

        [Attr, Required, MaxLength(9)]
        public string IdNumber { get; set; } = default!;

        [Attr]
        public DateTime CreationDate { get; set; }

        [Attr]
        public long TenantId { get; set; }

        [HasOne]
        public TenantResource Tenant { get; set; } = default!;
    }
}
