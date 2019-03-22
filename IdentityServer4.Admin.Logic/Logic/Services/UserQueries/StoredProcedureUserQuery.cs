





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Entities;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using IdentityServer4.Admin.Logic.Logic.Mappers;
using IdentityServer4.Admin.Logic.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services.UserQueries
{
    public class StoredProcedureUserQuery : IUserQuery
  {
    private readonly string query;
    private readonly Pagination pagination;
    [TupleElementNames(new string[] {"users", "totalMatches"})]
    private readonly Dictionary<UserState, Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>>> userStateToQueryMap;
    [TupleElementNames(new string[] {"users", "totalMatches"})]
    private readonly Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>> activeQuery;

    public StoredProcedureUserQuery(string query, UserState state, Pagination pagination)
    {
      Dictionary<UserState, Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>>> dictionary = new Dictionary<UserState, Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>>>();
      UserState index1 = new UserState(true, true, true);
      dictionary[index1] = new Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>>(StoredProcedureUserQuery.FindAllMatchingUsers);
      UserState index2 = new UserState(false, false, false);
      dictionary[index2] = new Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>>(StoredProcedureUserQuery.FindAllMatchingUsers);
      UserState index3 = new UserState(true, false, false);
      dictionary[index3] = new Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>>(StoredProcedureUserQuery.FindActiveMatchingUsers);
      UserState index4 = new UserState(false, true, false);
      dictionary[index4] = new Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>>(StoredProcedureUserQuery.FindBlockedMatchingUsers);
      UserState index5 = new UserState(false, false, true);
      dictionary[index5] = new Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>>(StoredProcedureUserQuery.FindDeletedMatchingUsers);
      UserState index6 = new UserState(true, false, true);
      dictionary[index6] = new Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>>(StoredProcedureUserQuery.FindActiveOrDeletedMatchingUsers);
      UserState index7 = new UserState(true, true, false);
      dictionary[index7] = new Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>>(StoredProcedureUserQuery.FindActiveOrBlockedMatchingUsers);
      UserState index8 = new UserState(false, true, true);
      dictionary[index8] = new Func<IUserManager, string, Pagination, Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>>>(StoredProcedureUserQuery.FindDeletedOrBlockedMatchingUsers);
      this.userStateToQueryMap = dictionary;
      
      base.\u002Ector();
      this.query = query;
      this.pagination = pagination;
      this.activeQuery = this.userStateToQueryMap[state];
    }

    public async Task<PagedResult<User>> GetUsers(IUserManager userManager)
    {
      int pagesToSkip = this.pagination.Page == 0 ? 0 : this.pagination.Page - 1;
      Pagination paginationForQuery = new Pagination(pagesToSkip, this.pagination.PageSize);
      ValueTuple<IEnumerable<IdentityExpressUser>, int> valueTuple1 = await this.activeQuery(userManager, this.query, paginationForQuery);
      ValueTuple<IEnumerable<IdentityExpressUser>, int> valueTuple2 = valueTuple1;
      IEnumerable<IdentityExpressUser> users = valueTuple2.Item1;
      int totalMatches = valueTuple2.Item2;
      valueTuple1 = new ValueTuple<IEnumerable<IdentityExpressUser>, int>();
      valueTuple2 = new ValueTuple<IEnumerable<IdentityExpressUser>, int>();
      int pageCount = (int) Math.Ceiling((double) totalMatches / (double) this.pagination.PageSize);
      return new PagedResult<User>()
      {
        CurrentPage = this.pagination.Page == 0 ? 1 : this.pagination.Page,
        PageSize = this.pagination.PageSize,
        TotalCount = (long) totalMatches,
        PageCount = pageCount <= 0 ? 1 : pageCount,
        Results = (IEnumerable<User>) users.Select<IdentityExpressUser, User>((Func<IdentityExpressUser, User>) (u => u.ToService(true))).ToList<User>(),
        IsSorted = true
      };
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    private static Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindAllMatchingUsers(IUserManager userManager, string toMatch, Pagination pagination)
    {
      return userManager.FindAllMatchingUsers(toMatch, pagination);
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    private static Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindDeletedMatchingUsers(IUserManager userManager, string toMatch, Pagination pagination)
    {
      return userManager.FindDeletedMatchingUsers(toMatch, pagination);
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    private static Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindBlockedMatchingUsers(IUserManager userManager, string toMatch, Pagination pagination)
    {
      return userManager.FindBlockedMatchingUsers(toMatch, pagination);
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    private static Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindActiveMatchingUsers(IUserManager userManager, string toMatch, Pagination pagination)
    {
      return userManager.FindActiveMatchingUsers(toMatch, pagination);
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    private static Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindActiveOrBlockedMatchingUsers(IUserManager userManager, string toMatch, Pagination pagination)
    {
      return userManager.FindActiveOrBlockedMatchingUsers(toMatch, pagination);
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    private static Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindActiveOrDeletedMatchingUsers(IUserManager userManager, string toMatch, Pagination pagination)
    {
      return userManager.FindActiveOrDeletedMatchingUsers(toMatch, pagination);
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    private static Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindDeletedOrBlockedMatchingUsers(IUserManager userManager, string toMatch, Pagination pagination)
    {
      return userManager.FindDeletedOrBlockedMatchingUsers(toMatch, pagination);
    }
  }
}
