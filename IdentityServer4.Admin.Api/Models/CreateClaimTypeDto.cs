namespace IdentityServer4.Admin.Api.Models
{
    public class CreateClaimTypeDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        public string ValueType { get; set; }

        public string Rule { get; set; }

        public string RuleValidationFailureDescription { get; set; }

        public bool UserEditable { get; set; }
    }
}
