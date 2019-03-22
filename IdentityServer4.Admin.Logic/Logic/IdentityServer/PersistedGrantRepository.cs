





using IdentityServer4.Admin.Logic.Entities;
using IdentityServer4.Admin.Logic.Interfaces;
using IdentityServer4.Admin.Logic.Interfaces.IdentityServer;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
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
  public class PersistedGrantRepository : IPersistedGrantRepository, IRepository<PersistedGrant, string>
  {
    private readonly IPersistedGrantDbContext context;

    public PersistedGrantRepository(IPersistedGrantDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      this.context = context;
    }

    public Task<IdentityResult> Add(PersistedGrant entity)
    {
      throw new NotImplementedException();
    }

    public Task<IdentityResult> Delete(PersistedGrant entity)
    {
      this.context.PersistedGrants.Remove(entity);
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<IdentityResult> DeleteBySubject(string subject)
    {
      this.context.PersistedGrants.RemoveRange((IEnumerable<PersistedGrant>) this.context.PersistedGrants.Where<PersistedGrant>((Expression<Func<PersistedGrant, bool>>) (x => x.SubjectId == subject)));
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public Task<IdentityResult> DeleteBySubjectAndClient(string subject, string clientId)
    {
      this.context.PersistedGrants.RemoveRange((IEnumerable<PersistedGrant>) this.context.PersistedGrants.Where<PersistedGrant>((Expression<Func<PersistedGrant, bool>>) (x => x.SubjectId == subject && x.ClientId == clientId)));
      return Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    public async Task<IEnumerable<PersistedGrant>> Find(Expression<Func<PersistedGrant, bool>> expression)
    {
      List<PersistedGrant> listAsync = await this.context.PersistedGrants.Where<PersistedGrant>(expression).ToListAsync<PersistedGrant>(new CancellationToken());
      return (IEnumerable<PersistedGrant>) listAsync;
    }

    public async Task<IEnumerable<PersistedGrant>> GetAll()
    {
      List<PersistedGrant> listAsync = await this.context.PersistedGrants.ToListAsync<PersistedGrant>(new CancellationToken());
      return (IEnumerable<PersistedGrant>) listAsync;
    }

    public Task<PersistedGrant> GetByKey(string key)
    {
      return Task.FromResult<PersistedGrant>(this.context.PersistedGrants.FirstOrDefault<PersistedGrant>((Expression<Func<PersistedGrant, bool>>) (x => x.Key == key)));
    }

    public Task<PagedResult<PersistedGrant>> GetByPage(int page, int pageSize)
    {
      throw new NotImplementedException();
    }

    public Task<IdentityResult> Update(PersistedGrant entity)
    {
      throw new NotImplementedException();
    }

    public Task<int> Count()
    {
      return this.context.PersistedGrants.CountAsync<PersistedGrant>(new CancellationToken());
    }
  }
}
