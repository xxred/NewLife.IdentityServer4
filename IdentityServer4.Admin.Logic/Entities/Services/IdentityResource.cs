using System;

namespace IdentityServer4.Admin.Logic.Entities.Services
{
  public class IdentityResource : IdentityServer4.Models.IdentityResource
  {
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public bool NonEditable { get; set; }
  }
}
