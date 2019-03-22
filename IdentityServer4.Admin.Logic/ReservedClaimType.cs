





using IdentityExpress.Identity;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Logic
{
  public class ReservedClaimType
  {
    public static readonly List<ReservedClaimType> ReservedClaimTypes = new List<ReservedClaimType>()
    {
      new ReservedClaimType("6923d5b4-61f9-4cdc-aad8-6c38091c525d", "email_verified", "Email Address Verified. (Reserved Claim Type)", IdentityExpressClaimValueType.Boolean),
      new ReservedClaimType("9d9f0cd1-2946-4e72-a954-53f29cec5c07", "phone_number", "Phone Number. (Reserved Claim Type)", IdentityExpressClaimValueType.String),
      new ReservedClaimType("af25caba-6e8b-4d10-b998-5aa7f51ed9cf", "phone_number_verified", "Phone Number Verified. (Reserved Claim Type)", IdentityExpressClaimValueType.Boolean)
    };
    public const string EmailVerified = "email_verified";
    public const string PhoneNumber = "phone_number";
    public const string PhoneNumberVerified = "phone_number_verified";

    private ReservedClaimType(string id, string name, string description, IdentityExpressClaimValueType valueType)
    {
      this.Id = id;
      this.Name = name;
      this.Description = description;
      this.ValueType = valueType;
    }

    public string Id { get; }

    public string Name { get; }

    public string Description { get; }

    public IdentityExpressClaimValueType ValueType { get; }
  }
}
