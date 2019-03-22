





namespace IdentityServer4.Admin.Logic.Options
{
  public class WebhookOptions
  {
    public string RegistrationConfirmationEndpoint { get; set; } = "";

    public string PasswordResetEndpoint { get; set; } = "";

    public string AuthorityUrl { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }
  }
}
