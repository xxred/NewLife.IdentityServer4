





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Identity
{
  public class IdentityExpressClaimTypeRepository : IRepository<IdentityExpressClaimType, string>
  {
    private readonly IdentityExpressDbContext context;

    public IdentityExpressClaimTypeRepository(IdentityExpressDbContext context)
    {
      IdentityExpressDbContext expressDbContext = context;
      if (expressDbContext == null)
        throw new ArgumentNullException(nameof (context));
      this.context = expressDbContext;
    }

    public async Task<IdentityResult> Add(IdentityExpressClaimType entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      EntityEntry<IdentityExpressClaimType> entityEntry = await this.context.ClaimTypes.AddAsync(entity, new CancellationToken());
      return IdentityResult.Success;
    }

    public Task<IdentityResult> Delete(IdentityExpressClaimType entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ClaimTypes.Remove(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public async Task<IEnumerable<IdentityExpressClaimType>> Find(Expression<Func<IdentityExpressClaimType, bool>> expression)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof (expression));
      List<IdentityExpressClaimType> listAsync = await this.context.ClaimTypes.Where<IdentityExpressClaimType>(expression).ToListAsync<IdentityExpressClaimType>(new CancellationToken());
      return (IEnumerable<IdentityExpressClaimType>) listAsync;
    }

    public async Task<IEnumerable<IdentityExpressClaimType>> GetAll()
    {
      List<IdentityExpressClaimType> listAsync = await this.context.ClaimTypes.ToListAsync<IdentityExpressClaimType>(new CancellationToken());
      return (IEnumerable<IdentityExpressClaimType>) listAsync;
    }

    public Task<IdentityExpressClaimType> GetByKey(string key)
    {
      if (string.IsNullOrWhiteSpace(key))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (key));
      return this.context.ClaimTypes.FirstOrDefaultAsync<IdentityExpressClaimType>((Expression<Func<IdentityExpressClaimType, bool>>) (x => x.Id == key), new CancellationToken());
    }

    public Task<IdentityResult> Update(IdentityExpressClaimType entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity));
      this.context.ClaimTypes.Attach(entity);
      entity.ConcurrencyStamp = Guid.NewGuid().ToString();
      this.context.ClaimTypes.Update(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<int> Count()
    {
      return this.context.ClaimTypes.CountAsync<IdentityExpressClaimType>(new CancellationToken());
    }
  }
}
