
using System;

namespace IdentityServer4.Admin.Api.Models
{
  public class UserLightWeightDto
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
  }
}
