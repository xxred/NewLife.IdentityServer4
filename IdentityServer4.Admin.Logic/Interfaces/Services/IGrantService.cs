using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
    public interface IGrantService
    {
        Task<IdentityResult> RevokeAll(string subject);

        Task<IdentityResult> RevokeClient(string subject, string clientId);

        Task<IList<Consent>> GetConsentForSubject(string subject);
    }
}
