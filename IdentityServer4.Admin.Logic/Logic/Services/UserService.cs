





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Entities;
using IdentityServer4.Admin.Logic.Entities.Exceptions;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.Admin.Logic.Logic.Extensions;
using IdentityServer4.Admin.Logic.Logic.Licensing;
using IdentityServer4.Admin.Logic.Logic.Mappers;
using IdentityServer4.Admin.Logic.Logic.Services.UserQueries;
using IdentityServer4.Admin.Logic.Options;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
    public class UserService : IUserService
  {
    private readonly IUserQueryFactory userQueryFactory;
    private readonly IIdentityUnitOfWorkFactory factory;
    private readonly ILookupNormalizer normalizer;
    private readonly IdentityErrorDescriber describer;
    private readonly ILicenseValidationService licensingService;
    private readonly IUserRegistrationService userRegistrationService;
    private readonly WebhookOptions options;
    private readonly IWebhookService webhookTokenService;

    public UserService(IIdentityUnitOfWorkFactory factory, ILookupNormalizer normalizer, ILicenseValidationService licensingService, IOptions<WebhookOptions> options, IUserRegistrationService userRegistrationService, IWebhookService webhookTokenService, IUserQueryFactory userQueryFactory, IdentityErrorDescriber describer = null)
    {
      IUserQueryFactory userQueryFactory1 = userQueryFactory;
      if (userQueryFactory1 == null)
        throw new ArgumentNullException(nameof (userQueryFactory));
      this.userQueryFactory = userQueryFactory1;
      IIdentityUnitOfWorkFactory unitOfWorkFactory = factory;
      if (unitOfWorkFactory == null)
        throw new ArgumentNullException(nameof (factory));
      this.factory = unitOfWorkFactory;
      ILookupNormalizer lookupNormalizer = normalizer;
      if (lookupNormalizer == null)
        throw new ArgumentNullException(nameof (normalizer));
      this.normalizer = lookupNormalizer;
      this.describer = describer ?? new IdentityErrorDescriber();
      ILicenseValidationService validationService = licensingService;
      if (validationService == null)
        throw new ArgumentNullException(nameof (licensingService));
      this.licensingService = validationService;
      IUserRegistrationService registrationService = userRegistrationService;
      if (registrationService == null)
        throw new ArgumentNullException(nameof (userRegistrationService));
      this.userRegistrationService = registrationService;
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      this.options = options.Value;
      IWebhookService webhookService = webhookTokenService;
      if (webhookService == null)
        throw new ArgumentNullException(nameof (webhookTokenService));
      this.webhookTokenService = webhookService;
    }

    public async Task<IdentityResult> Create(User user, string password)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        bool flag = await this.licensingService.IsWithinUserQuota(true);
        if (!flag)
          return IdentityResult.Failed(new IdentityError()
          {
            Code = "420",
            Description = "User quota reached"
          });
        IdentityExpressUser identityExpressUser = await uow.UserManager.FindByNameAsync(user.Username);
        IdentityExpressUser existingUser = identityExpressUser;
        identityExpressUser = (IdentityExpressUser) null;
        if (existingUser != null)
          return IdentityResult.Failed(this.describer.DuplicateUserName(user.Username));
        IdentityResult identityResult1 = await this.ValidateRequiredClaims(user.Claims);
        IdentityResult validateRequiredClaims = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (!validateRequiredClaims.Succeeded)
          return validateRequiredClaims;
        IdentityResult identityResult2 = await this.ValidateClaims(user.Claims);
        IdentityResult validateClaims = identityResult2;
        identityResult2 = (IdentityResult) null;
        if (!validateClaims.Succeeded)
          return validateClaims;
        IdentityResult identityResult3 = await this.ValidateRoles(user.Roles);
        IdentityResult validateRoles = identityResult3;
        identityResult3 = (IdentityResult) null;
        if (!validateRoles.Succeeded)
          return validateRoles;
        IdentityExpressUser identityExpressUser1 = new IdentityExpressUser();
        identityExpressUser1.Id = user.Subject;
        identityExpressUser1.UserName = user.Username;
        identityExpressUser1.Email = user.Email;
        identityExpressUser1.FirstName = user.FirstName;
        identityExpressUser1.LastName = user.LastName;
        identityExpressUser1.NormalizedFirstName = this.normalizer.Normalize(user.FirstName);
        identityExpressUser1.NormalizedLastName = this.normalizer.Normalize(user.LastName);
        IdentityExpressUser mappedUser = identityExpressUser1;
        IList<IdentityExpressClaim> identityExpressClaimList = await this.MapClaims(user.Subject, user.Claims);
        IEnumerator<IdentityExpressClaim> enumerator1 = identityExpressClaimList.GetEnumerator();
        identityExpressClaimList = (IList<IdentityExpressClaim>) null;
        try
        {
          while (enumerator1.MoveNext())
          {
            IdentityExpressClaim mappedClaim = enumerator1.Current;
            mappedUser.Claims.Add(mappedClaim);
            mappedClaim = (IdentityExpressClaim) null;
          }
        }
        finally
        {
          enumerator1?.Dispose();
        }
        enumerator1 = (IEnumerator<IdentityExpressClaim>) null;
        IList<IdentityExpressUserRole> identityExpressUserRoleList = await this.MapRoles(user.Subject, user.Roles);
        IEnumerator<IdentityExpressUserRole> enumerator2 = identityExpressUserRoleList.GetEnumerator();
        identityExpressUserRoleList = (IList<IdentityExpressUserRole>) null;
        try
        {
          while (enumerator2.MoveNext())
          {
            IdentityExpressUserRole mappedRole = enumerator2.Current;
            mappedUser.Roles.Add(mappedRole);
            mappedRole = (IdentityExpressUserRole) null;
          }
        }
        finally
        {
          enumerator2?.Dispose();
        }
        enumerator2 = (IEnumerator<IdentityExpressUserRole>) null;
        this.SetAndStripEmailConfirmed(mappedUser, mappedUser.Claims);
        this.SetAndStripPhoneNumber(mappedUser, mappedUser.Claims);
        this.SetAndStripPhoneNumberConfirmed(mappedUser, mappedUser.Claims);
        IdentityResult result;
        if (!string.IsNullOrWhiteSpace(password))
        {
          IdentityResult identityResult4 = await uow.UserManager.CreateAsync(mappedUser, password);
          result = identityResult4;
          identityResult4 = (IdentityResult) null;
        }
        else
        {
          IdentityResult identityResult4 = await uow.UserManager.CreateAsync(mappedUser);
          result = identityResult4;
          identityResult4 = (IdentityResult) null;
        }
        if (result.Succeeded)
        {
          IdentityResult identityResult4 = await uow.Commit();
          result = identityResult4;
          identityResult4 = (IdentityResult) null;
          if (!string.IsNullOrEmpty(this.options.RegistrationConfirmationEndpoint))
          {
            IdentityResult identityResult = await this.userRegistrationService.RegistrationConfirmation(mappedUser);
          }
        }
        return result;
      }
    }

    public async Task<IdentityResult> Delete(User user)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressUser identityExpressUser = await uow.UserManager.FindByIdAsync(user.Subject);
        IdentityExpressUser existingUser = identityExpressUser;
        identityExpressUser = (IdentityExpressUser) null;
        if (existingUser == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "User not found with subject " + user.Subject
          });
        IdentityResult identityResult1 = await uow.UserManager.DeleteAsync(existingUser);
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult2 = await uow.Commit();
          result = identityResult2;
          identityResult2 = (IdentityResult) null;
        }
        return result;
      }
    }

    public async Task<User> GetBySubject(string subject)
    {
      if (string.IsNullOrWhiteSpace(subject))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (subject));
      User service;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressUser identityExpressUser = await uow.UserManager.FindByIdAsync(subject);
        IdentityExpressUser user = identityExpressUser;
        identityExpressUser = (IdentityExpressUser) null;
        service = user.ToService(true);
      }
      return service;
    }

    public async Task<PagedResult<User>> Get(Pagination pagination = null, UserQueryBy query = null, IList<UserOrderBy> ordering = null, UserState state = null, bool includeRelationships = true)
    {
      int userCount;
      List<IdentityExpressUser> users;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IQueryable<IdentityExpressUser> queryableUsers = uow.UserManager.Users;
        if (includeRelationships)
          queryableUsers = (IQueryable<IdentityExpressUser>) ((IIncludableQueryable<IdentityExpressUser, IEnumerable<IdentityExpressClaim>>) ((IIncludableQueryable<IdentityExpressUser, IEnumerable<IdentityExpressUserRole>>) queryableUsers.Include<IdentityExpressUser, ICollection<IdentityExpressUserRole>>((Expression<Func<IdentityExpressUser, ICollection<IdentityExpressUserRole>>>) (x => x.Roles))).ThenInclude<IdentityExpressUser, IdentityExpressUserRole, IdentityExpressRole>((Expression<Func<IdentityExpressUserRole, IdentityExpressRole>>) (x => x.Role)).Include<IdentityExpressUser, ICollection<IdentityExpressClaim>>((Expression<Func<IdentityExpressUser, ICollection<IdentityExpressClaim>>>) (x => x.Claims))).ThenInclude<IdentityExpressUser, IdentityExpressClaim, IdentityExpressClaimType>((Expression<Func<IdentityExpressClaim, IdentityExpressClaimType>>) (x => x.ClaimTypeNav));
        if (query != null)
        {
          if (!string.IsNullOrWhiteSpace(query.Id))
          {
            IdentityExpressUser identityExpressUser = await uow.UserManager.FindByIdAsync(query.Id);
            IdentityExpressUser foundUser = identityExpressUser;
            identityExpressUser = (IdentityExpressUser) null;
            return PagedResult<User>.Single(foundUser.ToService(true));
          }
          if (!string.IsNullOrWhiteSpace(query.FullSearch))
          {
            IdentityExpressUser identityExpressUser = await uow.UserManager.FindByIdAsync(query.FullSearch);
            IdentityExpressUser foundUser = identityExpressUser;
            identityExpressUser = (IdentityExpressUser) null;
            if (foundUser != null)
              return PagedResult<User>.Single(foundUser.ToService(true));
            string normalizedSearch = this.normalizer.Normalize(query.FullSearch);
            queryableUsers = queryableUsers.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => x.NormalizedUserName != default (string) && x.NormalizedUserName.StartsWith(normalizedSearch) || x.NormalizedEmail != default (string) && x.NormalizedEmail.StartsWith(normalizedSearch) || x.NormalizedFirstName.StartsWith(normalizedSearch) || x.NormalizedLastName.StartsWith(normalizedSearch)));
            foundUser = (IdentityExpressUser) null;
          }
          if (!string.IsNullOrWhiteSpace(query.Username))
            queryableUsers = queryableUsers.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => x.NormalizedUserName.StartsWith(this.normalizer.Normalize(query.Username))));
          if (!string.IsNullOrWhiteSpace(query.Email))
            queryableUsers = queryableUsers.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => x.NormalizedEmail.StartsWith(this.normalizer.Normalize(query.Email))));
          if (!string.IsNullOrWhiteSpace(query.Name))
            queryableUsers = queryableUsers.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => x.NormalizedFirstName.StartsWith(this.normalizer.Normalize(query.Name)) || x.NormalizedLastName.StartsWith(this.normalizer.Normalize(query.Name))));
        }
        if (state != null && (!state.Active || !state.Blocked || !state.Deleted))
        {
          if (state.Active && state.Blocked)
            queryableUsers = queryableUsers.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => !x.IsDeleted));
          else if (state.Active && state.Deleted)
            queryableUsers = queryableUsers.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => !x.IsBlocked));
          else if (state.Blocked && state.Deleted)
            queryableUsers = queryableUsers.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => x.IsDeleted || x.IsBlocked));
          else if (state.Active)
            queryableUsers = queryableUsers.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => !x.IsDeleted && !x.IsBlocked));
          else if (state.Blocked)
            queryableUsers = queryableUsers.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => x.IsBlocked && !x.IsDeleted));
          else if (state.Deleted)
            queryableUsers = queryableUsers.Where<IdentityExpressUser>((Expression<Func<IdentityExpressUser, bool>>) (x => x.IsDeleted));
        }
        Task<int> userCountTask = queryableUsers.CountAsync<IdentityExpressUser>(new CancellationToken());
        IList<UserOrderBy> source = ordering;
        if (source != null && source.Any<UserOrderBy>())
        {
          if (!ordering.Validate())
            throw new ValidationException("Invalid Ordering Received");
          IOrderedQueryable<IdentityExpressUser> orderedUsers = queryableUsers.ApplyOrdering(ordering.First<UserOrderBy>());
          foreach (UserOrderBy orderBy in ordering.Skip<UserOrderBy>(1))
            orderedUsers = QueriableExtensions.ApplyOrdering(orderedUsers, orderBy);
          queryableUsers = (IQueryable<IdentityExpressUser>) orderedUsers;
          orderedUsers = (IOrderedQueryable<IdentityExpressUser>) null;
        }
        if (pagination != null)
          queryableUsers = pagination.Page == 1 || pagination.Page == 0 ? queryableUsers.Skip<IdentityExpressUser>(0).Take<IdentityExpressUser>(pagination.PageSize) : queryableUsers.Skip<IdentityExpressUser>(pagination.Page * pagination.PageSize).Take<IdentityExpressUser>(pagination.PageSize);
        Task<List<IdentityExpressUser>> usersTask = queryableUsers.ToListAsync<IdentityExpressUser>(new CancellationToken());
        await Task.WhenAll((Task) userCountTask, (Task) usersTask);
        userCount = userCountTask.Result;
        users = usersTask.Result;
        queryableUsers = (IQueryable<IdentityExpressUser>) null;
        userCountTask = (Task<int>) null;
        usersTask = (Task<List<IdentityExpressUser>>) null;
      }
      int pageCount = 0;
      Pagination pagination1 = pagination;
      int num;
      if (pagination1 == null)
      {
        num = 0;
      }
      else
      {
        int pageSize = pagination1.PageSize;
        num = 1;
      }
      if (num != 0 && (uint) pagination.PageSize > 0U)
        pageCount = (int) Math.Ceiling((double) userCount / (double) pagination.PageSize);
      PagedResult<User> pagedResult = new PagedResult<User>();
      Pagination pagination2 = pagination;
      pagedResult.CurrentPage = pagination2 != null ? pagination2.Page : 1;
      Pagination pagination3 = pagination;
      pagedResult.PageSize = pagination3 != null ? pagination3.PageSize : (userCount != 0 ? userCount : 1);
      pagedResult.PageCount = pageCount <= 0 ? 1 : pageCount;
      pagedResult.TotalCount = (long) userCount;
      pagedResult.Results = (IEnumerable<User>) users.Select<IdentityExpressUser, User>((Func<IdentityExpressUser, User>) (x => x.ToService(true))).ToList<User>();
      IList<UserOrderBy> source1 = ordering;
      pagedResult.IsSorted = source1 != null && source1.Any<UserOrderBy>();
      return pagedResult;
    }

    public async Task<PagedResult<User>> FindFast(Pagination pagination, string query, UserState state)
    {
      if (pagination == null)
        throw new ArgumentNullException(nameof (pagination));
      if (state == null)
        throw new ArgumentNullException(nameof (state));
      IUserQuery userQuery = this.userQueryFactory.Create(this.normalizer.Normalize(query), state, pagination);
      PagedResult<User> result;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        PagedResult<User> pagedResult = await userQuery.GetUsers(uow.UserManager);
        result = pagedResult;
        pagedResult = (PagedResult<User>) null;
      }
      return result;
    }

    public async Task<IdentityResult> AddRoles(User user, IList<string> roles)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      if (roles == null || !roles.Any<string>())
        throw new ArgumentNullException(nameof (roles));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressUser identityExpressUser1 = await uow.UserManager.FindByIdAsync(user.Subject);
        IdentityExpressUser identityExpressUser2 = identityExpressUser1;
        identityExpressUser1 = (IdentityExpressUser) null;
        if (identityExpressUser2 == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "User not found with subject " + user.Subject
          });
        IdentityResult identityResult1 = await this.ValidateRoles((IList<Role>) roles.Select<string, Role>((Func<string, Role>) (x => new Role(x))).ToList<Role>());
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult2 = await uow.UserManager.AddToRolesAsync(identityExpressUser2, (IEnumerable<string>) roles);
          result = identityResult2;
          identityResult2 = (IdentityResult) null;
          if (result.Succeeded)
          {
            IdentityResult identityResult3 = await uow.Commit();
            result = identityResult3;
            identityResult3 = (IdentityResult) null;
          }
        }
        return result;
      }
    }

    public async Task<IdentityResult> DeleteRoles(User user, IList<string> roles)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      if (roles == null || !roles.Any<string>())
        throw new ArgumentNullException(nameof (roles));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressUser identityExpressUser1 = await uow.UserManager.FindByIdAsync(user.Subject);
        IdentityExpressUser identityExpressUser2 = identityExpressUser1;
        identityExpressUser1 = (IdentityExpressUser) null;
        if (identityExpressUser2 == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "User not found with subject " + user.Subject
          });
        IdentityResult identityResult1 = await uow.UserManager.RemoveFromRolesAsync(identityExpressUser2, (IEnumerable<string>) roles);
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult2 = await uow.Commit();
          result = identityResult2;
          identityResult2 = (IdentityResult) null;
        }
        return result;
      }
    }

    public async Task<IdentityResult> AddClaim(User user, Claim claim)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      if (claim == null)
        throw new ArgumentNullException(nameof (claim));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressUser identityExpressUser1 = await uow.UserManager.FindByIdAsync(user.Subject);
        IdentityExpressUser identityExpressUser2 = identityExpressUser1;
        identityExpressUser1 = (IdentityExpressUser) null;
        if (identityExpressUser2 == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "User not found with subject " + user.Subject
          });
        IdentityResult identityResult1 = await this.ValidateClaims((IList<Claim>) new List<Claim>()
        {
          claim
        });
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          if (ReservedClaimType.ReservedClaimTypes.Any<ReservedClaimType>((Func<ReservedClaimType, bool>) (x => string.Compare(x.Name, claim.Type, StringComparison.CurrentCultureIgnoreCase) == 0)))
          {
            IList<IdentityExpressClaim> identityExpressClaimList = await this.MapClaims(identityExpressUser2.Id, (IList<Claim>) new List<Claim>()
            {
              claim
            });
            IList<IdentityExpressClaim> mappedClaims = identityExpressClaimList;
            identityExpressClaimList = (IList<IdentityExpressClaim>) null;
            this.SetAndStripEmailConfirmed(identityExpressUser2, (ICollection<IdentityExpressClaim>) mappedClaims);
            this.SetAndStripPhoneNumber(identityExpressUser2, (ICollection<IdentityExpressClaim>) mappedClaims);
            this.SetAndStripPhoneNumberConfirmed(identityExpressUser2, (ICollection<IdentityExpressClaim>) mappedClaims);
            IdentityResult identityResult2 = await uow.UserManager.UpdateAsync(identityExpressUser2);
            result = identityResult2;
            identityResult2 = (IdentityResult) null;
            mappedClaims = (IList<IdentityExpressClaim>) null;
          }
          else
          {
            IdentityResult identityResult2 = await uow.UserManager.AddClaimAsync(identityExpressUser2, claim);
            result = identityResult2;
            identityResult2 = (IdentityResult) null;
          }
          if (result.Succeeded)
          {
            IdentityResult identityResult2 = await uow.Commit();
            result = identityResult2;
            identityResult2 = (IdentityResult) null;
          }
        }
        return result;
      }
    }

    public async Task<IdentityResult> DeleteClaim(User user, Claim claim)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      if (claim == null)
        throw new ArgumentNullException(nameof (claim));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressUser identityExpressUser1 = await uow.UserManager.FindByIdAsync(user.Subject);
        IdentityExpressUser identityExpressUser2 = identityExpressUser1;
        identityExpressUser1 = (IdentityExpressUser) null;
        if (identityExpressUser2 == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "User not found with subject " + user.Subject
          });
        IdentityResult identityResult1 = await uow.UserManager.RemoveClaimAsync(identityExpressUser2, claim);
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult2 = await uow.Commit();
          result = identityResult2;
          identityResult2 = (IdentityResult) null;
        }
        return result;
      }
    }

    public async Task<IdentityResult> EditClaim(User user, Claim oldClaim, Claim newClaim)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      if (oldClaim == null)
        throw new ArgumentNullException(nameof (oldClaim));
      if (newClaim == null)
        throw new ArgumentNullException(nameof (newClaim));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressUser identityExpressUser1 = await uow.UserManager.FindByIdAsync(user.Subject);
        IdentityExpressUser identityExpressUser2 = identityExpressUser1;
        identityExpressUser1 = (IdentityExpressUser) null;
        if (identityExpressUser2 == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "User not found with subject " + user.Subject
          });
        IdentityResult identityResult1 = await this.ValidateClaims((IList<Claim>) new List<Claim>()
        {
          newClaim
        });
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          if (ReservedClaimType.ReservedClaimTypes.Any<ReservedClaimType>((Func<ReservedClaimType, bool>) (x => string.Compare(x.Name, newClaim.Type, StringComparison.CurrentCultureIgnoreCase) == 0)))
          {
            IList<IdentityExpressClaim> identityExpressClaimList = await this.MapClaims(identityExpressUser2.Id, (IList<Claim>) new List<Claim>()
            {
              newClaim
            });
            IList<IdentityExpressClaim> mappedClaims = identityExpressClaimList;
            identityExpressClaimList = (IList<IdentityExpressClaim>) null;
            this.SetAndStripEmailConfirmed(identityExpressUser2, (ICollection<IdentityExpressClaim>) mappedClaims);
            this.SetAndStripPhoneNumber(identityExpressUser2, (ICollection<IdentityExpressClaim>) mappedClaims);
            this.SetAndStripPhoneNumberConfirmed(identityExpressUser2, (ICollection<IdentityExpressClaim>) mappedClaims);
            IdentityExpressClaim claimToRemove = identityExpressUser2.Claims.FirstOrDefault<IdentityExpressClaim>((Func<IdentityExpressClaim, bool>) (x =>
            {
              if (x.ClaimTypeNav.Name == oldClaim.Type)
                return x.ClaimValue == oldClaim.Value;
              return false;
            }));
            identityExpressUser2.Claims.Remove(claimToRemove);
            IdentityResult identityResult2 = await uow.UserManager.UpdateAsync(identityExpressUser2);
            result = identityResult2;
            identityResult2 = (IdentityResult) null;
            mappedClaims = (IList<IdentityExpressClaim>) null;
            claimToRemove = (IdentityExpressClaim) null;
          }
          else
          {
            IdentityResult identityResult2 = await uow.UserManager.ReplaceClaimAsync(identityExpressUser2, oldClaim, newClaim);
            result = identityResult2;
            identityResult2 = (IdentityResult) null;
          }
          if (result.Succeeded)
          {
            IdentityResult identityResult2 = await uow.Commit();
            result = identityResult2;
            identityResult2 = (IdentityResult) null;
          }
        }
        return result;
      }
    }

    public async Task<IdentityResult> Update(User user)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressUser identityExpressUser = await uow.UserManager.FindByIdAsync(user.Subject);
        IdentityExpressUser existingUser = identityExpressUser;
        identityExpressUser = (IdentityExpressUser) null;
        if (existingUser == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "User '" + user.Subject + "' does not exist."
          });
        string initialConcurrencyStamp = existingUser.ConcurrencyStamp;
        existingUser.UserName = user.Username;
        existingUser.NormalizedUserName = this.normalizer.Normalize(user.Username);
        existingUser.Email = user.Email;
        existingUser.NormalizedEmail = this.normalizer.Normalize(user.Email);
        existingUser.FirstName = user.FirstName;
        existingUser.NormalizedFirstName = this.normalizer.Normalize(user.FirstName);
        existingUser.LastName = user.LastName;
        existingUser.NormalizedLastName = this.normalizer.Normalize(user.LastName);
        existingUser.IsBlocked = user.IsBlocked;
        existingUser.LockoutEnd = user.LockoutEnd;
        existingUser.LockoutEnabled = user.LockoutEnabled;
        existingUser.IsDeleted = user.IsDeleted;
        IdentityResult identityResult1 = await this.ValidateRequiredClaims(user.Claims);
        IdentityResult validateRequiredClaims = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (!validateRequiredClaims.Succeeded)
          return validateRequiredClaims;
        IdentityResult identityResult2 = await this.ValidateClaims(user.Claims);
        IdentityResult validateClaims = identityResult2;
        identityResult2 = (IdentityResult) null;
        if (!validateClaims.Succeeded)
          return validateClaims;
        IdentityResult identityResult3 = await this.ValidateRoles(user.Roles);
        IdentityResult validateRoles = identityResult3;
        identityResult3 = (IdentityResult) null;
        if (!validateRoles.Succeeded)
          return validateRoles;
        IList<IdentityExpressClaim> identityExpressClaimList = await this.MapClaims(user.Subject, user.Claims);
        IList<IdentityExpressClaim> updatedClaims = identityExpressClaimList;
        identityExpressClaimList = (IList<IdentityExpressClaim>) null;
        IList<IdentityExpressUserRole> identityExpressUserRoleList = await this.MapRoles(user.Subject, user.Roles);
        IList<IdentityExpressUserRole> updatedRoles = identityExpressUserRoleList;
        identityExpressUserRoleList = (IList<IdentityExpressUserRole>) null;
        this.SetAndStripEmailConfirmed(existingUser, (ICollection<IdentityExpressClaim>) updatedClaims);
        this.SetAndStripPhoneNumber(existingUser, (ICollection<IdentityExpressClaim>) updatedClaims);
        this.SetAndStripPhoneNumberConfirmed(existingUser, (ICollection<IdentityExpressClaim>) updatedClaims);
        IdentityResult identityResult6 = await uow.UserManager.AddClaimsAsync(existingUser, (IEnumerable<Claim>) updatedClaims.Where<IdentityExpressClaim>((Func<IdentityExpressClaim, bool>) (x => existingUser.Claims.All<IdentityExpressClaim>((Func<IdentityExpressClaim, bool>) (y => new
        {
          ClaimType = y.ClaimType,
          ClaimValue = y.ClaimValue
        }.GetHashCode() != new
        {
          ClaimType = x.ClaimType,
          ClaimValue = x.ClaimValue
        }.GetHashCode())))).Select<IdentityExpressClaim, Claim>((Func<IdentityExpressClaim, Claim>) (x => x.ToClaim())).ToList<Claim>());
        IdentityResult identityResult7 = await uow.UserManager.RemoveClaimsAsync(existingUser, (IEnumerable<Claim>) existingUser.Claims.Where<IdentityExpressClaim>((Func<IdentityExpressClaim, bool>) (x => updatedClaims.All<IdentityExpressClaim>((Func<IdentityExpressClaim, bool>) (y => new
        {
          ClaimType = y.ClaimType,
          ClaimValue = y.ClaimValue
        }.GetHashCode() != new
        {
          ClaimType = x.ClaimType,
          ClaimValue = x.ClaimValue
        }.GetHashCode())))).Select<IdentityExpressClaim, Claim>((Func<IdentityExpressClaim, Claim>) (x => x.ToClaim())).ToList<Claim>());
        IdentityResult rolesAsync = await uow.UserManager.AddToRolesAsync(existingUser, (IEnumerable<string>) updatedRoles.Where<IdentityExpressUserRole>((Func<IdentityExpressUserRole, bool>) (x => existingUser.Roles.All<IdentityExpressUserRole>((Func<IdentityExpressUserRole, bool>) (y => x.RoleId != y.RoleId)))).Select<IdentityExpressUserRole, string>((Func<IdentityExpressUserRole, string>) (x => x.Role.Name)).ToList<string>());
        IdentityResult identityResult8 = await uow.UserManager.RemoveFromRolesAsync(existingUser, (IEnumerable<string>) existingUser.Roles.Where<IdentityExpressUserRole>((Func<IdentityExpressUserRole, bool>) (x => updatedRoles.All<IdentityExpressUserRole>((Func<IdentityExpressUserRole, bool>) (y => x.RoleId != y.RoleId)))).Select<IdentityExpressUserRole, string>((Func<IdentityExpressUserRole, string>) (x => x.Role.Name)).ToList<string>());
        existingUser.ConcurrencyStamp = initialConcurrencyStamp;
        IdentityResult identityResult4 = await uow.UserManager.UpdateAsync(existingUser);
        IdentityResult result = identityResult4;
        identityResult4 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult5 = await uow.Commit();
          result = identityResult5;
          identityResult5 = (IdentityResult) null;
        }
        return result;
      }
    }

    public async Task<IdentityResult> ResetPassword(User user)
    {
      bool endpoint = await this.webhookTokenService.PostToEndpoint(this.options.PasswordResetEndpoint, user.Email);
      bool isSuccess = endpoint;
      return isSuccess ? IdentityResult.Success : IdentityResult.Failed(Array.Empty<IdentityError>());
    }

    private async Task<IdentityResult> ValidateRequiredClaims(IList<Claim> claims)
    {
      IdentityResult identityResult;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IEnumerable<IdentityExpressClaimType> source = await uow.ClaimTypeRepository.Find((Expression<Func<IdentityExpressClaimType, bool>>) (x => x.Required));
        List<IdentityExpressClaimType> requiredClaims = source.ToList<IdentityExpressClaimType>();
        source = (IEnumerable<IdentityExpressClaimType>) null;
        List<IdentityExpressClaimType> requiredClaimsWithoutValue = !requiredClaims.Any<IdentityExpressClaimType>() || claims != null ? requiredClaims.Where<IdentityExpressClaimType>((Func<IdentityExpressClaimType, bool>) (x => claims.All<Claim>((Func<Claim, bool>) (y => y.Type != x.Name)))).ToList<IdentityExpressClaimType>() : requiredClaims;
        identityResult = !requiredClaimsWithoutValue.Any<IdentityExpressClaimType>() ? IdentityResult.Success : IdentityResult.Failed(requiredClaimsWithoutValue.Select<IdentityExpressClaimType, IdentityError>((Func<IdentityExpressClaimType, IdentityError>) (x => new IdentityError()
        {
          Description = "Missing Required ClaimType of '" + x.Name + "'"
        })).ToArray<IdentityError>());
      }
      return identityResult;
    }

    private async Task<IdentityResult> ValidateClaims(IList<Claim> claims)
    {
      
      
      UserService.\u003C\u003Ec__DisplayClass22_0 cDisplayClass220 = new UserService.\u003C\u003Ec__DisplayClass22_0();
      
      cDisplayClass220.claims = claims;
      
      
      if (cDisplayClass220.claims == null || !cDisplayClass220.claims.Any<Claim>())
        return IdentityResult.Success;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        
        
        UserService.\u003C\u003Ec__DisplayClass22_1 cDisplayClass221 = new UserService.\u003C\u003Ec__DisplayClass22_1();
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        
        
        
        IEnumerable<IdentityExpressClaimType> source = await uow.ClaimTypeRepository.Find(Expression.Lambda<Func<IdentityExpressClaimType, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new Expression[2]
        {
          cDisplayClass220.claims,
          (Expression) Expression.Lambda<Func<Claim, bool>>((Expression) Expression.Equal(y.Type, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (IdentityExpressClaimType.Name)))), new ParameterExpression[1]
          {
            parameterExpression2
          })
        }), new ParameterExpression[1]
        {
          parameterExpression1
        }));
        
        cDisplayClass221.claimTypes = source.ToList<IdentityExpressClaimType>;
        source = (IEnumerable<IdentityExpressClaimType>) null;
        
        
        
        
        return cDisplayClass221.claimTypes.Count == cDisplayClass220.claims.Select<Claim, string>((Func<Claim, string>) (x => x.Type)).Distinct<string>().Count<string>() ? IdentityResult.Success : IdentityResult.Failed(cDisplayClass220.claims.Where<Claim>(new Func<Claim, bool>(cDisplayClass221.\u003CValidateClaims\u003Eb__1)).Select<Claim, IdentityError>((Func<Claim, IdentityError>) (x => new IdentityError()
        {
          Description = "Could not find ClaimType of '" + x.Type + "'"
        })).ToArray<IdentityError>());
      }
    }

    private async Task<IList<IdentityExpressClaim>> MapClaims(string subject, IList<Claim> claims)
    {
      
      
      UserService.\u003C\u003Ec__DisplayClass23_0 cDisplayClass230 = new UserService.\u003C\u003Ec__DisplayClass23_0();
      
      cDisplayClass230.claims = claims;
      
      cDisplayClass230.subject = subject;
      
      
      if (cDisplayClass230.claims == null || !cDisplayClass230.claims.Any<Claim>())
        return (IList<IdentityExpressClaim>) new List<IdentityExpressClaim>();
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        
        
        UserService.\u003C\u003Ec__DisplayClass23_1 cDisplayClass231 = new UserService.\u003C\u003Ec__DisplayClass23_1();
        
        cDisplayClass231.CS\u0024\u003C\u003E8__locals1 = cDisplayClass230;
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        
        
        
        
        IEnumerable<IdentityExpressClaimType> source = await uow.ClaimTypeRepository.Find(Expression.Lambda<Func<IdentityExpressClaimType, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new Expression[2]
        {
          cDisplayClass231.CS\u0024\u003C\u003E8__locals1.claims,
          (Expression) Expression.Lambda<Func<Claim, bool>>((Expression) Expression.Equal(y.Type, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (IdentityExpressClaimType.Name)))), new ParameterExpression[1]
          {
            parameterExpression2
          })
        }), new ParameterExpression[1]
        {
          parameterExpression1
        }));
        
        cDisplayClass231.claimTypes = source.ToList<IdentityExpressClaimType>;
        source = (IEnumerable<IdentityExpressClaimType>) null;
        
        
        
        return (IList<IdentityExpressClaim>) cDisplayClass231.CS\u0024\u003C\u003E8__locals1.claims.Select<Claim, IdentityExpressClaim>(new Func<Claim, IdentityExpressClaim>(cDisplayClass231.\u003CMapClaims\u003Eb__1)).ToList<IdentityExpressClaim>();
      }
    }

    private async Task<IdentityResult> ValidateRoles(IList<Role> roles)
    {
      
      
      UserService.\u003C\u003Ec__DisplayClass24_0 cDisplayClass240 = new UserService.\u003C\u003Ec__DisplayClass24_0();
      
      cDisplayClass240.roles = roles;
      
      cDisplayClass240.\u003C\u003E4__this = this;
      
      
      if (cDisplayClass240.roles == null || !cDisplayClass240.roles.Any<Role>())
        return IdentityResult.Success;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        
        
        UserService.\u003C\u003Ec__DisplayClass24_1 cDisplayClass241 = new UserService.\u003C\u003Ec__DisplayClass24_1();
        
        cDisplayClass241.CS\u0024\u003C\u003E8__locals1 = cDisplayClass240;
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        
        
        
        
        
        List<IdentityExpressRole> identityExpressRoleList = await uow.RoleManager.Roles.Where<IdentityExpressRole>(Expression.Lambda<Func<IdentityExpressRole, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new Expression[2]
        {
          cDisplayClass241.CS\u0024\u003C\u003E8__locals1.roles,
          (Expression) Expression.Lambda<Func<Role, bool>>((Expression) Expression.Equal(this.normalizer.Normalize(y.Name), (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (IdentityRole<string>.get_NormalizedName), __typeref (IdentityRole<string>)))), new ParameterExpression[1]
          {
            parameterExpression2
          })
        }), new ParameterExpression[1]
        {
          parameterExpression1
        })).ToListAsync<IdentityExpressRole>(new CancellationToken());
        
        cDisplayClass241.foundRoles = identityExpressRoleList;
        identityExpressRoleList = (List<IdentityExpressRole>) null;
        
        
        
        
        
        
        return cDisplayClass241.foundRoles.Count == cDisplayClass241.CS\u0024\u003C\u003E8__locals1.roles.Select<Role, string>((Func<Role, string>) (x => x.Name)).Distinct<string>().Count<string>() ? IdentityResult.Success : IdentityResult.Failed(cDisplayClass241.CS\u0024\u003C\u003E8__locals1.roles.Where<Role>(new Func<Role, bool>(cDisplayClass241.\u003CValidateRoles\u003Eb__1)).Select<Role, IdentityError>((Func<Role, IdentityError>) (x => new IdentityError()
        {
          Description = "Could not find Role with name '" + x.Name + "'"
        })).ToArray<IdentityError>());
      }
    }

    private async Task<IList<IdentityExpressUserRole>> MapRoles(string subject, IList<Role> roles)
    {
      
      
      UserService.\u003C\u003Ec__DisplayClass25_0 cDisplayClass250 = new UserService.\u003C\u003Ec__DisplayClass25_0();
      
      cDisplayClass250.roles = roles;
      
      cDisplayClass250.\u003C\u003E4__this = this;
      
      cDisplayClass250.subject = subject;
      
      
      if (cDisplayClass250.roles == null || !cDisplayClass250.roles.Any<Role>())
        return (IList<IdentityExpressUserRole>) new List<IdentityExpressUserRole>();
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        
        
        UserService.\u003C\u003Ec__DisplayClass25_1 cDisplayClass251 = new UserService.\u003C\u003Ec__DisplayClass25_1();
        
        cDisplayClass251.CS\u0024\u003C\u003E8__locals1 = cDisplayClass250;
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        
        
        
        
        
        List<IdentityExpressRole> identityExpressRoleList = await uow.RoleManager.Roles.Where<IdentityExpressRole>(Expression.Lambda<Func<IdentityExpressRole, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new Expression[2]
        {
          cDisplayClass251.CS\u0024\u003C\u003E8__locals1.roles,
          (Expression) Expression.Lambda<Func<Role, bool>>((Expression) Expression.Equal(this.normalizer.Normalize(y.Name), (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (IdentityRole<string>.get_NormalizedName), __typeref (IdentityRole<string>)))), new ParameterExpression[1]
          {
            parameterExpression2
          })
        }), new ParameterExpression[1]
        {
          parameterExpression1
        })).ToListAsync<IdentityExpressRole>(new CancellationToken());
        
        cDisplayClass251.foundRoles = identityExpressRoleList;
        identityExpressRoleList = (List<IdentityExpressRole>) null;
        
        
        
        return (IList<IdentityExpressUserRole>) cDisplayClass251.CS\u0024\u003C\u003E8__locals1.roles.Select<Role, IdentityExpressUserRole>(new Func<Role, IdentityExpressUserRole>(cDisplayClass251.\u003CMapRoles\u003Eb__1)).ToList<IdentityExpressUserRole>();
      }
    }

    public async Task<IList<UserLoginInfo>> GetUserLogins(string subject)
    {
      if (subject == null)
        throw new ArgumentNullException(nameof (subject));
      IList<UserLoginInfo> userLoginInfoList;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressUser identityExpressUser = await uow.UserManager.FindByIdAsync(subject);
        IdentityExpressUser user = identityExpressUser;
        identityExpressUser = (IdentityExpressUser) null;
        IList<UserLoginInfo> loginsAsync = await uow.UserManager.GetLoginsAsync(user);
        userLoginInfoList = loginsAsync;
      }
      return userLoginInfoList;
    }

    public async Task<IdentityResult> DeleteUserLogin(string subject, string loginProvider, string providerKey)
    {
      if (subject == null)
        throw new ArgumentNullException(nameof (subject));
      if (loginProvider == null)
        throw new ArgumentNullException(nameof (loginProvider));
      if (providerKey == null)
        throw new ArgumentNullException(nameof (providerKey));
      IdentityResult identityResult1;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressUser identityExpressUser = await uow.UserManager.FindByIdAsync(subject);
        IdentityExpressUser user = identityExpressUser;
        identityExpressUser = (IdentityExpressUser) null;
        IdentityResult identityResult2 = await uow.UserManager.RemoveLoginAsync(user, loginProvider, providerKey);
        IdentityResult identityResult = await uow.Commit();
        identityResult1 = identityResult;
      }
      return identityResult1;
    }

    private void SetAndStripEmailConfirmed(IdentityExpressUser user, ICollection<IdentityExpressClaim> claims)
    {
      if (claims == null)
        return;
      IdentityExpressClaim identityExpressClaim = claims.FirstOrDefault<IdentityExpressClaim>((Func<IdentityExpressClaim, bool>) (x => x.ClaimTypeNav.NormalizedName == this.normalizer.Normalize("email_verified")));
      if (identityExpressClaim == null)
        return;
      bool result;
      if (bool.TryParse(identityExpressClaim.ClaimValue, out result))
      {
        user.EmailConfirmed = result;
        claims.Remove(identityExpressClaim);
      }
      else
        claims.Remove(identityExpressClaim);
    }

    private void SetAndStripPhoneNumber(IdentityExpressUser user, ICollection<IdentityExpressClaim> claims)
    {
      if (claims == null)
        return;
      IdentityExpressClaim identityExpressClaim = claims.FirstOrDefault<IdentityExpressClaim>((Func<IdentityExpressClaim, bool>) (x => x.ClaimTypeNav.NormalizedName == this.normalizer.Normalize("phone_number")));
      if (identityExpressClaim == null)
        return;
      user.PhoneNumber = identityExpressClaim.ClaimValue;
      claims.Remove(identityExpressClaim);
    }

    private void SetAndStripPhoneNumberConfirmed(IdentityExpressUser user, ICollection<IdentityExpressClaim> claims)
    {
      if (claims == null)
        return;
      IdentityExpressClaim identityExpressClaim = claims.FirstOrDefault<IdentityExpressClaim>((Func<IdentityExpressClaim, bool>) (x => x.ClaimTypeNav.NormalizedName == this.normalizer.Normalize("phone_number_verified")));
      if (identityExpressClaim == null)
        return;
      bool result;
      if (bool.TryParse(identityExpressClaim.ClaimValue, out result))
      {
        user.PhoneNumberConfirmed = result;
        claims.Remove(identityExpressClaim);
      }
      else
        claims.Remove(identityExpressClaim);
    }
  }
}
