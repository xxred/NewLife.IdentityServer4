





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
  public class ExtendedIdentityResourceRepository : IExtendedIdentityResourceRepository, IRepository<ExtendedIdentityResource, string>
  {
    private readonly ExtendedConfigurationDbContext context;

    public ExtendedIdentityResourceRepository(ExtendedConfigurationDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      this.context = context;
    }

    public Task<IdentityResult> Add(ExtendedIdentityResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ExtendedIdentityResources.Add(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<IdentityResult> Delete(ExtendedIdentityResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ExtendedIdentityResources.Remove(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public async Task<IEnumerable<ExtendedIdentityResource>> Find(Expression<Func<ExtendedIdentityResource, bool>> expression)
    {
      List<ExtendedIdentityResource> listAsync = await this.context.ExtendedIdentityResources.Where<ExtendedIdentityResource>(expression).ToListAsync<ExtendedIdentityResource>(new CancellationToken());
      return (IEnumerable<ExtendedIdentityResource>) listAsync;
    }

    public async Task<IEnumerable<ExtendedIdentityResource>> GetAll()
    {
      List<ExtendedIdentityResource> listAsync = await this.context.ExtendedIdentityResources.ToListAsync<ExtendedIdentityResource>(new CancellationToken());
      return (IEnumerable<ExtendedIdentityResource>) listAsync;
    }

    public async Task<ExtendedIdentityResource> GetByKey(string key)
    {
      ExtendedIdentityResource identityResource = await this.context.ExtendedIdentityResources.FirstOrDefaultAsync<ExtendedIdentityResource>((Expression<Func<ExtendedIdentityResource, bool>>) (x => x.Id == key), new CancellationToken());
      return identityResource;
    }

    public async Task<ExtendedIdentityResource> GetByIdentityResourceName(string identityResourceName)
    {
      ExtendedIdentityResource identityResource = await this.context.ExtendedIdentityResources.FirstOrDefaultAsync<ExtendedIdentityResource>((Expression<Func<ExtendedIdentityResource, bool>>) (x => x.IdentityResourceName == identityResourceName), new CancellationToken());
      return identityResource;
    }

    public Task<IdentityResult> Update(ExtendedIdentityResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ExtendedIdentityResources.Attach(entity);
      this.context.ExtendedIdentityResources.Update(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<int> Count()
    {
      return this.context.ExtendedIdentityResources.CountAsync<ExtendedIdentityResource>(new CancellationToken());
    }
  }
}
