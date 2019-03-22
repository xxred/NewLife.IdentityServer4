





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Identity
{
  public class IdentityExpressUserManager : UserManager<IdentityExpressUser>, IUserManager
  {
    private const string FindAllWithCountProcedureName = "dbo.FindAllUsersWithCount";
    private const string FindDeletedWithCountProcedureName = "dbo.FindDeletedUsersWithCount";
    private const string FindBlockedWithCountProcedureName = "dbo.FindBlockedUsersWithCount";
    private const string FindActiveWithCountProcedureName = "dbo.FindActiveUsersWithCount";
    private const string FindActiveOrDeletedWithCountProcedureName = "dbo.FindActiveOrDeletedUsersWithCount";
    private const string FindActiveOrBlockedWithCountProcedureName = "dbo.FindActiveOrBlockedWithCount";
    private const string FindBlockedOrDeletedWithCountProcedureName = "dbo.FindBlockedOrDeletedWithCount";
    private const string SearchTermParameterName = "@SearchTerm";
    private const string PageNumberParameterName = "@PageNumber";
    private const string PageSizeParameterName = "@PageSize";
    private const string IdColumnName = "Id";
    private const string ConcurrencyStampColumnName = "ConcurrencyStamp";
    private const string AccessFailedCountColumnName = "AccessFailedCount";
    private const string EmailColumnName = "Email";
    private const string EmailConfirmedColumnName = "EmailConfirmed";
    private const string FirstNameColumnName = "FirstName";
    private const string LastNameColumnName = "LastName";
    private const string IsBlockedColumnName = "IsBlocked";
    private const string IsDeletedColumnName = "IsDeleted";
    private const string LockoutEnabledColumnName = "LockoutEnabled";
    private const string LockoutEndColumnName = "LockoutEnd";
    private const string NormalizedEmailColumnName = "NormalizedEmail";
    private const string NormalizedFirstNameColumnName = "NormalizedFirstName";
    private const string NormalizedLastNameColumnName = "NormalizedLastName";
    private const string NormalizedUserNameColumnName = "NormalizedUserName";
    private const string PasswordHashColumnName = "PasswordHash";
    private const string PhoneNumberColumnName = "PhoneNumber";
    private const string PhoneNumberConfirmedColumnName = "PhoneNumberConfirmed";
    private const string SecurityStampColumnName = "SecurityStamp";
    private const string TwoFactorEnabledColumnName = "TwoFactorEnabled";
    private const string UserNameColumnName = "UserName";

    public IdentityExpressUserManager(IUserStore<IdentityExpressUser> store, IOptions<IdentityOptions> options, IPasswordHasher<IdentityExpressUser> passwordHasher, IEnumerable<IUserValidator<IdentityExpressUser>> userValidators, IEnumerable<IPasswordValidator<IdentityExpressUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<IdentityExpressUser>> logger)
      : base(store, options, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    private IdentityExpressDbContext<IdentityExpressUser> Context
    {
      get
      {
        return ((UserStore<IdentityExpressUser, IdentityExpressRole, IdentityExpressDbContext<IdentityExpressUser>, string, IdentityExpressClaim, IdentityExpressUserRole, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>) this.Store).Context;
      }
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    public Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindAllMatchingUsers(string toMatch, Pagination pagination)
    {
      return this.ExecuteFindUsersStoredProcedure(toMatch, pagination, "dbo.FindAllUsersWithCount");
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    private async Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> ExecuteFindUsersStoredProcedure(string toMatch, Pagination pagination, string procedureName)
    {
      DbConnection connection = this.Context.Database.GetDbConnection();
      if (connection.State != ConnectionState.Open)
        await connection.OpenAsync();
      int totalMatches;
      IEnumerable<IdentityExpressUser> users;
      using (DbCommand cmd = connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = procedureName;
        cmd.AddParameter("@SearchTerm", DbType.String, (object) toMatch, ParameterDirection.Input).AddParameter("@PageNumber", DbType.Int32, (object) pagination.Page, ParameterDirection.Input).AddParameter("@PageSize", DbType.Int32, (object) pagination.PageSize, ParameterDirection.Input);
        DbDataReader dbDataReader = await cmd.ExecuteReaderAsync();
        DbDataReader reader = dbDataReader;
        dbDataReader = (DbDataReader) null;
        try
        {
          int num1 = await reader.ReadAsync() ? 1 : 0;
          totalMatches = (int) reader[0];
          int num2 = await reader.NextResultAsync() ? 1 : 0;
          IEnumerable<IdentityExpressUser> identityExpressUsers = await this.MapUsersFromResultSet(reader);
          users = identityExpressUsers;
          identityExpressUsers = (IEnumerable<IdentityExpressUser>) null;
        }
        finally
        {
          reader?.Dispose();
        }
        reader = (DbDataReader) null;
      }
      return new ValueTuple<IEnumerable<IdentityExpressUser>, int>(users, totalMatches);
    }

    private async Task<IEnumerable<IdentityExpressUser>> MapUsersFromResultSet(DbDataReader reader)
    {
      List<IdentityExpressUser> users = new List<IdentityExpressUser>();
      int idColumn = reader.GetOrdinal("Id");
      int concurrencyStampColumn = reader.GetOrdinal("ConcurrencyStamp");
      int accessFailedCountColumn = reader.GetOrdinal("AccessFailedCount");
      int emailColumn = reader.GetOrdinal("Email");
      int emailConfirmedColumn = reader.GetOrdinal("EmailConfirmed");
      int firstNameColumn = reader.GetOrdinal("FirstName");
      int lastNameColumn = reader.GetOrdinal("LastName");
      int isBlockedColumn = reader.GetOrdinal("IsBlocked");
      int isDeletedColumn = reader.GetOrdinal("IsDeleted");
      int lockoutEnabledColumn = reader.GetOrdinal("LockoutEnabled");
      int lockoutEndColumn = reader.GetOrdinal("LockoutEnd");
      int normalizedEmailColumn = reader.GetOrdinal("NormalizedEmail");
      int normalizedFirstNameColumn = reader.GetOrdinal("NormalizedFirstName");
      int normalizedLastNameColumn = reader.GetOrdinal("NormalizedLastName");
      int normalizedUserNameColumn = reader.GetOrdinal("NormalizedUserName");
      int passwordHashColumn = reader.GetOrdinal("PasswordHash");
      int phoneNumberColumn = reader.GetOrdinal("PhoneNumber");
      int phoneNumberConfirmedColumn = reader.GetOrdinal("PhoneNumberConfirmed");
      int securityStampColumn = reader.GetOrdinal("SecurityStamp");
      int twoFactorEnabledColumn = reader.GetOrdinal("TwoFactorEnabled");
      int userNameColumn = reader.GetOrdinal("UserName");
      while (true)
      {
        bool flag = await reader.ReadAsync();
        if (flag)
        {
          IdentityExpressUser identityExpressUser = new IdentityExpressUser();
          identityExpressUser.Id = reader.GetStringOrNull(idColumn);
          identityExpressUser.ConcurrencyStamp = reader.GetStringOrNull(concurrencyStampColumn);
          identityExpressUser.AccessFailedCount = reader.GetInt32(accessFailedCountColumn);
          identityExpressUser.Email = reader.GetStringOrNull(emailColumn);
          identityExpressUser.EmailConfirmed = reader.GetBoolean(emailConfirmedColumn);
          identityExpressUser.FirstName = reader.GetStringOrNull(firstNameColumn);
          identityExpressUser.LastName = reader.GetStringOrNull(lastNameColumn);
          identityExpressUser.IsBlocked = reader.GetBoolean(isBlockedColumn);
          identityExpressUser.IsDeleted = reader.GetBoolean(isDeletedColumn);
          identityExpressUser.LockoutEnabled = reader.GetBoolean(lockoutEnabledColumn);
          identityExpressUser.LockoutEnd = reader.GetValueOrNull<DateTimeOffset>(lockoutEndColumn);
          identityExpressUser.NormalizedEmail = reader.GetStringOrNull(normalizedEmailColumn);
          identityExpressUser.NormalizedUserName = reader.GetStringOrNull(normalizedUserNameColumn);
          identityExpressUser.NormalizedFirstName = reader.GetStringOrNull(normalizedFirstNameColumn);
          identityExpressUser.NormalizedLastName = reader.GetStringOrNull(normalizedLastNameColumn);
          identityExpressUser.PasswordHash = reader.GetStringOrNull(passwordHashColumn);
          identityExpressUser.PhoneNumber = reader.GetStringOrNull(phoneNumberColumn);
          identityExpressUser.PhoneNumberConfirmed = reader.GetBoolean(phoneNumberConfirmedColumn);
          identityExpressUser.SecurityStamp = reader.GetStringOrNull(securityStampColumn);
          identityExpressUser.TwoFactorEnabled = reader.GetBoolean(twoFactorEnabledColumn);
          identityExpressUser.UserName = reader.GetStringOrNull(userNameColumn);
          IdentityExpressUser user = identityExpressUser;
          users.Add(user);
          user = (IdentityExpressUser) null;
        }
        else
          break;
      }
      return (IEnumerable<IdentityExpressUser>) users;
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    public Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindDeletedMatchingUsers(string toMatch, Pagination pagination)
    {
      return this.ExecuteFindUsersStoredProcedure(toMatch, pagination, "dbo.FindDeletedUsersWithCount");
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    public Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindBlockedMatchingUsers(string toMatch, Pagination pagination)
    {
      return this.ExecuteFindUsersStoredProcedure(toMatch, pagination, "dbo.FindBlockedUsersWithCount");
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    public Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindActiveMatchingUsers(string toMatch, Pagination pagination)
    {
      return this.ExecuteFindUsersStoredProcedure(toMatch, pagination, "dbo.FindActiveUsersWithCount");
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    public Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindActiveOrBlockedMatchingUsers(string toMatch, Pagination pagination)
    {
      return this.ExecuteFindUsersStoredProcedure(toMatch, pagination, "dbo.FindActiveOrBlockedWithCount");
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    public Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindActiveOrDeletedMatchingUsers(string toMatch, Pagination pagination)
    {
      return this.ExecuteFindUsersStoredProcedure(toMatch, pagination, "dbo.FindActiveOrDeletedUsersWithCount");
    }

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    public Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindDeletedOrBlockedMatchingUsers(string toMatch, Pagination pagination)
    {
      return this.ExecuteFindUsersStoredProcedure(toMatch, pagination, "dbo.FindBlockedOrDeletedWithCount");
    }
  }
}
