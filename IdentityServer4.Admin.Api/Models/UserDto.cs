using IdentityServer4.Admin.Api.Models;
using IdentityServer4.Admin.Logic.Entities.Services;
using System;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Api.Models
{
    public class UserDto
  {
    public string Subject { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public bool IsBlocked { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public bool IsDeleted { get; set; }

    public IList<RoleDto> Roles { get; set; }

    public IList<ClaimDto> Claims { get; set; }
  }
}
