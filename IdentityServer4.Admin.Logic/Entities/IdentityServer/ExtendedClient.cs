





using System;

namespace IdentityServer4.Admin.Logic.Entities.IdentityServer
{
  public class ExtendedClient
  {
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string NormalizedClientId { get; set; }

    public string NormalizedClientName { get; set; }

    public string Description { get; set; }

    public bool Reserved { get; set; }

    public string ClientId { get; set; }
  }
}
