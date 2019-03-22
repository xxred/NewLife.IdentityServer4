using IdentityServer4.Admin.Logic.Entities.Configuration;
using IdentityServer4.Admin.Logic.Entities.Services;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Api.Controllers
{
    public class UserPermissions
    {
        public IEnumerable<Permission> Permissions
        {
            get;
            set;
        }

        public IEnumerable<ClaimDto> PolicyClaims
        {
            get;
            set;
        }

        public UserPermissions(IEnumerable<Permission> permissions, IEnumerable<ClaimDto> policyClaims)
        {
            Permissions = permissions;
            PolicyClaims = policyClaims;
        }
    }
}
