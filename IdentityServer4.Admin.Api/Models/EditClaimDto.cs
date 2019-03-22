namespace IdentityServer4.Admin.Api.Models
{
    public class EditClaimDto
    {
        public string OldClaimType { get; set; }

        public string OldClaimValue { get; set; }

        public string NewClaimType { get; set; }

        public string NewClaimValue { get; set; }
    }
}
