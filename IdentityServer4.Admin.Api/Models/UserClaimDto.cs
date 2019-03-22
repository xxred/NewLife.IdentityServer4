using IdentityServer4.Admin.Logic.Entities.Services;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Api.Models
{
    public class UserClaimDto
    {
        public string Subject { get; set; }

        public List<ClaimDto> Claims { get; set; }
    }
}
