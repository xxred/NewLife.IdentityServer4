





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
  public class IdentityResourceRepository : IRepository<IdentityResource, string>
  {
    private readonly ConfigurationDbContext context;

    public IdentityResourceRepository(ConfigurationDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      this.context = context;
    }

    public Task<IdentityResult> Add(IdentityResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.IdentityResources.Add(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<IdentityResult> Delete(IdentityResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.IdentityResources.Remove(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public async Task<IEnumerable<IdentityResource>> Find(Expression<Func<IdentityResource, bool>> expression)
    {
      List<IdentityResource> listAsync = await this.context.IdentityResources.Include<IdentityResource, List<IdentityClaim>>((Expression<Func<IdentityResource, List<IdentityClaim>>>) (x => x.UserClaims)).Where<IdentityResource>(expression).ToListAsync<IdentityResource>(new CancellationToken());
      return (IEnumerable<IdentityResource>) listAsync;
    }

    public async Task<IEnumerable<IdentityResource>> GetAll()
    {
      List<IdentityResource> listAsync = await this.context.IdentityResources.Include<IdentityResource, List<IdentityClaim>>((Expression<Func<IdentityResource, List<IdentityClaim>>>) (x => x.UserClaims)).ToListAsync<IdentityResource>(new CancellationToken());
      return (IEnumerable<IdentityResource>) listAsync;
    }

    public async Task<IdentityResource> GetByKey(string key)
    {
      IdentityResource identityResource = await this.context.IdentityResources.Include<IdentityResource, List<IdentityClaim>>((Expression<Func<IdentityResource, List<IdentityClaim>>>) (x => x.UserClaims)).FirstOrDefaultAsync<IdentityResource>((Expression<Func<IdentityResource, bool>>) (x => x.Name == key), new CancellationToken());
      return identityResource;
    }

    public Task<IdentityResult> Update(IdentityResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.IdentityResources.Attach(entity);
      this.context.IdentityResources.Update(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<int> Count()
    {
      return this.context.IdentityResources.CountAsync<IdentityResource>(new CancellationToken());
    }
  }
}
