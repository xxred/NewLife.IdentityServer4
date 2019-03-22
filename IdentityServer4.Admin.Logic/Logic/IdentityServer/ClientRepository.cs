





using IdentityServer4.Admin.Logic.Interfaces;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.IdentityServer
{
  public class ClientRepository : IRepository<Client, string>
  {
    private readonly ConfigurationDbContext context;

    public ClientRepository(ConfigurationDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      this.context = context;
    }

    public Task<IdentityResult> Add(Client entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.Clients.Add(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<IdentityResult> Delete(Client entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.Clients.Remove(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public async Task<IEnumerable<Client>> Find(Expression<Func<Client, bool>> expression)
    {
      List<Client> listAsync = await this.context.Clients.Include<Client, List<ClientGrantType>>((Expression<Func<Client, List<ClientGrantType>>>) (x => x.AllowedGrantTypes)).Include<Client, List<ClientRedirectUri>>((Expression<Func<Client, List<ClientRedirectUri>>>) (x => x.RedirectUris)).Include<Client, List<ClientPostLogoutRedirectUri>>((Expression<Func<Client, List<ClientPostLogoutRedirectUri>>>) (x => x.PostLogoutRedirectUris)).Include<Client, List<ClientScope>>((Expression<Func<Client, List<ClientScope>>>) (x => x.AllowedScopes)).Include<Client, List<ClientCorsOrigin>>((Expression<Func<Client, List<ClientCorsOrigin>>>) (x => x.AllowedCorsOrigins)).Include<Client, List<ClientIdPRestriction>>((Expression<Func<Client, List<ClientIdPRestriction>>>) (x => x.IdentityProviderRestrictions)).Include<Client, List<ClientSecret>>((Expression<Func<Client, List<ClientSecret>>>) (x => x.ClientSecrets)).Include<Client, List<ClientClaim>>((Expression<Func<Client, List<ClientClaim>>>) (x => x.Claims)).Include<Client, List<ClientProperty>>((Expression<Func<Client, List<ClientProperty>>>) (x => x.Properties)).Where<Client>(expression).ToListAsync<Client>(new CancellationToken());
      return (IEnumerable<Client>) listAsync;
    }

    public async Task<IEnumerable<Client>> GetAll()
    {
      List<Client> listAsync = await this.context.Clients.Include<Client, List<ClientGrantType>>((Expression<Func<Client, List<ClientGrantType>>>) (x => x.AllowedGrantTypes)).Include<Client, List<ClientRedirectUri>>((Expression<Func<Client, List<ClientRedirectUri>>>) (x => x.RedirectUris)).Include<Client, List<ClientPostLogoutRedirectUri>>((Expression<Func<Client, List<ClientPostLogoutRedirectUri>>>) (x => x.PostLogoutRedirectUris)).Include<Client, List<ClientScope>>((Expression<Func<Client, List<ClientScope>>>) (x => x.AllowedScopes)).Include<Client, List<ClientCorsOrigin>>((Expression<Func<Client, List<ClientCorsOrigin>>>) (x => x.AllowedCorsOrigins)).Include<Client, List<ClientIdPRestriction>>((Expression<Func<Client, List<ClientIdPRestriction>>>) (x => x.IdentityProviderRestrictions)).Include<Client, List<ClientSecret>>((Expression<Func<Client, List<ClientSecret>>>) (x => x.ClientSecrets)).Include<Client, List<ClientClaim>>((Expression<Func<Client, List<ClientClaim>>>) (x => x.Claims)).Include<Client, List<ClientProperty>>((Expression<Func<Client, List<ClientProperty>>>) (x => x.Properties)).ToListAsync<Client>(new CancellationToken());
      return (IEnumerable<Client>) listAsync;
    }

    public async Task<Client> GetByKey(string key)
    {
      Client client = await this.context.Clients.Include<Client, List<ClientGrantType>>((Expression<Func<Client, List<ClientGrantType>>>) (x => x.AllowedGrantTypes)).Include<Client, List<ClientRedirectUri>>((Expression<Func<Client, List<ClientRedirectUri>>>) (x => x.RedirectUris)).Include<Client, List<ClientPostLogoutRedirectUri>>((Expression<Func<Client, List<ClientPostLogoutRedirectUri>>>) (x => x.PostLogoutRedirectUris)).Include<Client, List<ClientScope>>((Expression<Func<Client, List<ClientScope>>>) (x => x.AllowedScopes)).Include<Client, List<ClientCorsOrigin>>((Expression<Func<Client, List<ClientCorsOrigin>>>) (x => x.AllowedCorsOrigins)).Include<Client, List<ClientIdPRestriction>>((Expression<Func<Client, List<ClientIdPRestriction>>>) (x => x.IdentityProviderRestrictions)).Include<Client, List<ClientSecret>>((Expression<Func<Client, List<ClientSecret>>>) (x => x.ClientSecrets)).Include<Client, List<ClientClaim>>((Expression<Func<Client, List<ClientClaim>>>) (x => x.Claims)).Include<Client, List<ClientProperty>>((Expression<Func<Client, List<ClientProperty>>>) (x => x.Properties)).FirstOrDefaultAsync<Client>((Expression<Func<Client, bool>>) (x => x.ClientId == key), new CancellationToken());
      return client;
    }

    public Task<IdentityResult> Update(Client entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.Clients.Attach(entity);
      this.context.Clients.Update(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<int> Count()
    {
      return this.context.Clients.CountAsync<Client>(new CancellationToken());
    }
  }
}
