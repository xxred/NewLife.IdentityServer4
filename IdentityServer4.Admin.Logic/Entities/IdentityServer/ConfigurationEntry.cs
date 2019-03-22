





namespace IdentityServer4.Admin.Logic.Entities.IdentityServer
{
  public class ConfigurationEntry
  {
    [System.ComponentModel.DataAnnotations.Key]
    public string Key { get; set; }

    public string Value { get; set; }
  }
}
