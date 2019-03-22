





using System;

namespace IdentityServer4.Admin.Logic.Entities.Services
{
  public class ClaimType
  {
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; }

    public string Description { get; set; }

    public bool Required { get; set; }

    public bool Reserved { get; set; }

    public string ValueType { get; set; }

    public string Rule { get; set; }

    public string RuleValidationFailureDescription { get; set; }

    public bool UserEditable { get; set; }
  }
}
