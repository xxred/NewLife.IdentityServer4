





namespace IdentityServer4.Admin.Logic.Logic.Licensing
{
  public class ValidateLicenseResult
  {
    public bool IsValid { get; set; }

    public string ErrorMessage { get; set; }

    public static ValidateLicenseResult Valid()
    {
      return new ValidateLicenseResult() { IsValid = true };
    }

    public static ValidateLicenseResult Invalid(string errorMessage)
    {
      return new ValidateLicenseResult()
      {
        IsValid = false,
        ErrorMessage = errorMessage
      };
    }
  }
}
