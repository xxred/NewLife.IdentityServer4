





using IdentityServer4.Admin.Logic.Entities.Services;

namespace IdentityServer4.Admin.Logic.Logic.Services.UserQueries
{
  public interface IUserQueryFactory
  {
    IUserQuery Create(string match, UserState matchingUserStates, Pagination pagination);
  }
}
