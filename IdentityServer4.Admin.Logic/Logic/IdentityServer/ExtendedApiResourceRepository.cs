





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
  public class ExtendedApiResourceRepository : IExtendedApiResourceRepository, IRepository<ExtendedApiResource, string>
  {
    private readonly ExtendedConfigurationDbContext context;

    public ExtendedApiResourceRepository(ExtendedConfigurationDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      this.context = context;
    }

    public Task<IdentityResult> Add(ExtendedApiResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ExtendedApiResources.Add(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<IdentityResult> Delete(ExtendedApiResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ExtendedApiResources.Remove(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public async Task<IEnumerable<ExtendedApiResource>> Find(Expression<Func<ExtendedApiResource, bool>> expression)
    {
      List<ExtendedApiResource> listAsync = await this.context.ExtendedApiResources.Where<ExtendedApiResource>(expression).ToListAsync<ExtendedApiResource>(new CancellationToken());
      return (IEnumerable<ExtendedApiResource>) listAsync;
    }

    public async Task<IEnumerable<ExtendedApiResource>> GetAll()
    {
      List<ExtendedApiResource> listAsync = await this.context.ExtendedApiResources.ToListAsync<ExtendedApiResource>(new CancellationToken());
      return (IEnumerable<ExtendedApiResource>) listAsync;
    }

    public async Task<ExtendedApiResource> GetByKey(string key)
    {
      ExtendedApiResource extendedApiResource = await this.context.ExtendedApiResources.FirstOrDefaultAsync<ExtendedApiResource>((Expression<Func<ExtendedApiResource, bool>>) (x => x.Id == key), new CancellationToken());
      return extendedApiResource;
    }

    public async Task<ExtendedApiResource> GetByApiResourceName(string apiResourceName)
    {
      ExtendedApiResource extendedApiResource = await this.context.ExtendedApiResources.FirstOrDefaultAsync<ExtendedApiResource>((Expression<Func<ExtendedApiResource, bool>>) (x => x.ApiResourceName == apiResourceName), new CancellationToken());
      return extendedApiResource;
    }

    public Task<IdentityResult> Update(ExtendedApiResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ExtendedApiResources.Attach(entity);
      this.context.ExtendedApiResources.Update(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<int> Count()
    {
      return this.context.ExtendedApiResources.CountAsync<ExtendedApiResource>(new CancellationToken());
    }
  }
}
