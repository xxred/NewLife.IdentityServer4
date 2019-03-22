





using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using IdentityServer4.Admin.Logic.Interfaces;
using IdentityServer4.Admin.Logic.Interfaces.IdentityServer;
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
  public class ExtendedClientRepository : IExtendedClientRepository, IRepository<ExtendedClient, string>
  {
    private readonly ExtendedConfigurationDbContext context;

    public ExtendedClientRepository(ExtendedConfigurationDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      this.context = context;
    }

    public Task<IdentityResult> Add(ExtendedClient entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ExtendedClients.Add(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<IdentityResult> Delete(ExtendedClient entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ExtendedClients.Remove(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public async Task<IEnumerable<ExtendedClient>> Find(Expression<Func<ExtendedClient, bool>> expression)
    {
      List<ExtendedClient> listAsync = await this.context.ExtendedClients.Where<ExtendedClient>(expression).ToListAsync<ExtendedClient>(new CancellationToken());
      return (IEnumerable<ExtendedClient>) listAsync;
    }

    public async Task<IEnumerable<ExtendedClient>> GetAll()
    {
      List<ExtendedClient> listAsync = await this.context.ExtendedClients.ToListAsync<ExtendedClient>(new CancellationToken());
      return (IEnumerable<ExtendedClient>) listAsync;
    }

    public async Task<ExtendedClient> GetByKey(string key)
    {
      ExtendedClient extendedClient = await this.context.ExtendedClients.FirstOrDefaultAsync<ExtendedClient>((Expression<Func<ExtendedClient, bool>>) (x => x.Id == key), new CancellationToken());
      return extendedClient;
    }

    public async Task<ExtendedClient> GetByClientId(string clientid)
    {
      ExtendedClient extendedClient = await this.context.ExtendedClients.FirstOrDefaultAsync<ExtendedClient>((Expression<Func<ExtendedClient, bool>>) (x => x.ClientId == clientid), new CancellationToken());
      return extendedClient;
    }

    public async Task<ExtendedClient> GetByNormalizedClientName(string normalizedClientName)
    {
      ExtendedClient extendedClient = await this.context.ExtendedClients.FirstOrDefaultAsync<ExtendedClient>((Expression<Func<ExtendedClient, bool>>) (x => x.NormalizedClientName == normalizedClientName), new CancellationToken());
      return extendedClient;
    }

    public Task<IdentityResult> Update(ExtendedClient entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ExtendedClients.Attach(entity);
      this.context.ExtendedClients.Update(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<int> Count()
    {
      return this.context.ExtendedClients.CountAsync<ExtendedClient>(new CancellationToken());
    }
  }
}
