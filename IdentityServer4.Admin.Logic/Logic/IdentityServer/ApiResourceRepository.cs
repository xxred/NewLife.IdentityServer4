





using IdentityServer4.Admin.Logic.Interfaces;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.IdentityServer
{
  public class ApiResourceRepository : IRepository<ApiResource, string>
  {
    private readonly ConfigurationDbContext context;

    public ApiResourceRepository(ConfigurationDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      this.context = context;
    }

    public Task<IdentityResult> Add(ApiResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ApiResources.Add(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<IdentityResult> Delete(ApiResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ApiResources.Remove(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public async Task<IEnumerable<ApiResource>> Find(Expression<Func<ApiResource, bool>> expression)
    {
      List<ApiResource> listAsync = await ((IIncludableQueryable<ApiResource, IEnumerable<ApiScope>>) this.context.ApiResources.Include<ApiResource, List<ApiScope>>((Expression<Func<ApiResource, List<ApiScope>>>) (x => x.Scopes))).ThenInclude<ApiResource, ApiScope, List<ApiScopeClaim>>((Expression<Func<ApiScope, List<ApiScopeClaim>>>) (x => x.UserClaims)).Include<ApiResource, List<ApiSecret>>((Expression<Func<ApiResource, List<ApiSecret>>>) (x => x.Secrets)).Include<ApiResource, List<ApiResourceClaim>>((Expression<Func<ApiResource, List<ApiResourceClaim>>>) (x => x.UserClaims)).Where<ApiResource>(expression).ToListAsync<ApiResource>(new CancellationToken());
      return (IEnumerable<ApiResource>) listAsync;
    }

    public async Task<IEnumerable<ApiResource>> GetAll()
    {
      List<ApiResource> listAsync = await ((IIncludableQueryable<ApiResource, IEnumerable<ApiScope>>) this.context.ApiResources.Include<ApiResource, List<ApiScope>>((Expression<Func<ApiResource, List<ApiScope>>>) (x => x.Scopes))).ThenInclude<ApiResource, ApiScope, List<ApiScopeClaim>>((Expression<Func<ApiScope, List<ApiScopeClaim>>>) (x => x.UserClaims)).Include<ApiResource, List<ApiSecret>>((Expression<Func<ApiResource, List<ApiSecret>>>) (x => x.Secrets)).Include<ApiResource, List<ApiResourceClaim>>((Expression<Func<ApiResource, List<ApiResourceClaim>>>) (x => x.UserClaims)).ToListAsync<ApiResource>(new CancellationToken());
      return (IEnumerable<ApiResource>) listAsync;
    }

    public async Task<ApiResource> GetByKey(string key)
    {
      ApiResource apiResource = await ((IIncludableQueryable<ApiResource, IEnumerable<ApiScope>>) this.context.ApiResources.Include<ApiResource, List<ApiScope>>((Expression<Func<ApiResource, List<ApiScope>>>) (x => x.Scopes))).ThenInclude<ApiResource, ApiScope, List<ApiScopeClaim>>((Expression<Func<ApiScope, List<ApiScopeClaim>>>) (x => x.UserClaims)).Include<ApiResource, List<ApiSecret>>((Expression<Func<ApiResource, List<ApiSecret>>>) (x => x.Secrets)).Include<ApiResource, List<ApiResourceClaim>>((Expression<Func<ApiResource, List<ApiResourceClaim>>>) (x => x.UserClaims)).FirstOrDefaultAsync<ApiResource>((Expression<Func<ApiResource, bool>>) (x => x.Name == key), new CancellationToken());
      return apiResource;
    }

    public Task<IdentityResult> Update(ApiResource entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ApiResources.Attach(entity);
      this.context.ApiResources.Update(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<int> Count()
    {
      return this.context.ApiResources.CountAsync<ApiResource>(new CancellationToken());
    }
  }
}
