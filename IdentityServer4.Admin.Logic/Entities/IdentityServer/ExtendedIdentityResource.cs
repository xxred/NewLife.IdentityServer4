





using System;

namespace IdentityServer4.Admin.Logic.Entities.IdentityServer
{
  public class ExtendedIdentityResource
  {
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string NormalizedName { get; set; }

    public bool Reserved { get; set; }

    public string IdentityResourceName { get; set; }
  }
}
