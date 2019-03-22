using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Logic.Services;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Api.Models
{
    public class CreateUserDto
    {
        public string Username { get; set; }

        [IgnorePropertyComparison]
        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<RoleDto> Roles { get; set; }

        public IList<ClaimDto> Claims { get; set; }
    }
}
