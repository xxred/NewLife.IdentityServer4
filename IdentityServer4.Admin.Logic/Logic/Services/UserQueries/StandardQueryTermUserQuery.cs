





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
    public class StandardQueryTermUserQuery : UserQuery
  {
    private readonly string query;
    private readonly Pagination pagination;

    public StandardQueryTermUserQuery(string query, UserState state, Pagination pagination)
      : base(state)
    {
      this.query = query;
      this.pagination = pagination;
    }

    public override async Task<PagedResult<User>> GetUsers(IUserManager userManager)
    {
      IQueryable<IdentityExpressUser> queryableUsers = userManager.Users;
      queryableUsers = queryableUsers.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => x.NormalizedUserName != default (string) && x.NormalizedUserName.StartsWith(this.query) || x.NormalizedEmail != default (string) && x.NormalizedEmail.StartsWith(this.query) || x.NormalizedFirstName.StartsWith(this.query) || x.NormalizedLastName.StartsWith(this.query)));
      queryableUsers = this.TranslateUserState(queryableUsers);
      Task<int> userCountTask = queryableUsers.CountAsync<IdentityExpressUser>(new CancellationToken());
      queryableUsers = (IQueryable<IdentityExpressUser>) queryableUsers.OrderBy<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (u => u.UserName));
      queryableUsers = this.ApplyPagination(queryableUsers, this.pagination);
      Task<List<IdentityExpressUser>> usersTask = queryableUsers.ToListAsync<IdentityExpressUser>(new CancellationToken());
      await Task.WhenAll((Task) userCountTask, (Task) usersTask);
      int userCount = userCountTask.Result;
      List<IdentityExpressUser> users = usersTask.Result;
      int pageCount = (int) Math.Ceiling((double) userCount / (double) this.pagination.PageSize);
      return new PagedResult<User>()
      {
        CurrentPage = this.pagination.Page == 0 ? 1 : this.pagination.Page,
        PageSize = this.pagination.PageSize,
        PageCount = pageCount <= 0 ? 1 : pageCount,
        TotalCount = (long) userCount,
        Results = (IEnumerable<User>) users.Select<IdentityExpressUser, User>((Func<IdentityExpressUser, User>) (x => x.ToService(true))).ToList<User>(),
        IsSorted = true
      };
    }
  }
}
