namespace IdentityServer4.Admin.Api.Models
{
  public class ClaimTypeDto
  {
    public string Id { get; set; }

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
