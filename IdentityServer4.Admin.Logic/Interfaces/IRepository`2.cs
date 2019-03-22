





using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces
{
  public interface IRepository<TEntity, in TKey> where TEntity : class
  {
    Task<IdentityResult> Add(TEntity entity);

    Task<int> Count();

    Task<IdentityResult> Delete(TEntity entity);

    Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> expression);

    Task<IEnumerable<TEntity>> GetAll();

    Task<TEntity> GetByKey(TKey key);

    Task<IdentityResult> Update(TEntity entity);
  }
}
