





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Entities;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using IdentityServer4.Admin.Logic.Entities.Services;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services.UserQueries
{
    public abstract class UserQuery : IUserQuery
  {
    private readonly UserState state;

    protected UserQuery(UserState state)
    {
      this.state = state;
    }

    public abstract Task<PagedResult<User>> GetUsers(IUserManager userManager);

    protected IQueryable<IdentityExpressUser> TranslateUserState(IQueryable<IdentityExpressUser> source)
    {
      if (this.state.Active && this.state.Blocked)
        source = source.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => !x.IsDeleted));
      else if (this.state.Active && this.state.Deleted)
        source = source.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => !x.IsBlocked));
      else if (this.state.Blocked && this.state.Deleted)
        source = source.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => x.IsDeleted || x.IsBlocked));
      else if (this.state.Active)
        source = source.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => !x.IsDeleted && !x.IsBlocked));
      else if (this.state.Blocked)
        source = source.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => x.IsBlocked && !x.IsDeleted));
      else if (this.state.Deleted)
        source = source.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => x.IsDeleted));
      return source;
    }

    protected IQueryable<IdentityExpressUser> ApplyPagination(IQueryable<IdentityExpressUser> userQuery, Pagination pagination)
    {
      int num = pagination.Page == 0 ? 0 : pagination.Page - 1;
      return userQuery.Skip<IdentityExpressUser>(num * pagination.PageSize).Take<IdentityExpressUser>(pagination.PageSize);
    }
  }
}
