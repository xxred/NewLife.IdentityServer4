





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Entities;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using IdentityServer4.Admin.Logic.Logic.Mappers;
using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services.UserQueries
{
    public class AllUserQuery : UserQuery
  {
    private const int SortThreshold = 50000;
    private readonly Pagination pagination;

    public AllUserQuery(UserState state, Pagination pagination)
      : base(state)
    {
      this.pagination = pagination;
    }

    public override async Task<PagedResult<User>> GetUsers(IUserManager userManager)
    {
      IQueryable<IdentityExpressUser> queryableUsers = userManager.Users;
      queryableUsers = this.TranslateUserState(queryableUsers);
      int num = await queryableUsers.CountAsync<IdentityExpressUser>(new CancellationToken());
      int userCount = num;
      bool shouldSort = userCount <= 50000;
      if (shouldSort)
        queryableUsers = (IQueryable<IdentityExpressUser>) queryableUsers.OrderBy<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (u => u.UserName));
      queryableUsers = this.ApplyPagination(queryableUsers, this.pagination);
      List<IdentityExpressUser> identityExpressUserList = await queryableUsers.ToListAsync<IdentityExpressUser>(new CancellationToken());
      List<IdentityExpressUser> users = identityExpressUserList;
      identityExpressUserList = (List<IdentityExpressUser>) null;
      int pageCount = (int) Math.Ceiling((double) userCount / (double) this.pagination.PageSize);
      return new PagedResult<User>()
      {
        CurrentPage = this.pagination.Page == 0 ? 1 : this.pagination.Page,
        PageSize = this.pagination.PageSize,
        PageCount = pageCount <= 0 ? 1 : pageCount,
        TotalCount = (long) userCount,
        Results = (IEnumerable<User>) users.Select<IdentityExpressUser, User>((Func<IdentityExpressUser, User>) (x => x.ToService(true))).ToList<User>(),
        IsSorted = shouldSort
      };
    }
  }
}
