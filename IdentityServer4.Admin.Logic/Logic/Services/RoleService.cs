using IdentityServer4.Admin.Logic.Entities.Exceptions;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.Admin.Logic.Logic.Extensions;
using IdentityServer4.Admin.Logic.Logic.Mappers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
    public class RoleService : IRoleService
  {
    private readonly IIdentityUnitOfWorkFactory factory;
    private readonly ILookupNormalizer normalizer;
    private readonly IdentityErrorDescriber describer;

    public RoleService(IIdentityUnitOfWorkFactory factory, ILookupNormalizer normalizer, IdentityErrorDescriber describer = null)
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      if (normalizer == null)
        throw new ArgumentNullException(nameof (normalizer));
      this.factory = factory;
      this.normalizer = normalizer;
      this.describer = describer ?? new IdentityErrorDescriber();
    }

    public async Task<IdentityResult> Create(Role role)
    {
      if (role == null)
        throw new ArgumentNullException(nameof (role));
      if (string.IsNullOrWhiteSpace(role.Name))
        return IdentityResult.Failed(this.describer.InvalidRoleName(role.Name));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressRole identityExpressRole1 = await uow.RoleManager.FindByNameAsync(role.Name);
        IdentityExpressRole foundRole = identityExpressRole1;
        identityExpressRole1 = (IdentityExpressRole) null;
        if (foundRole != null)
          return IdentityResult.Failed(this.describer.DuplicateRoleName(role.Name));
        IdentityExpressRole identityExpressRole = new IdentityExpressRole();
        identityExpressRole.Id = role.Id;
        identityExpressRole.Name = role.Name;
        identityExpressRole.Description = role.Description;
        identityExpressRole.Reserved = role.Reserved;
        IdentityExpressRole identityExpressRole2 = identityExpressRole;
        IdentityResult identityResult1 = await uow.RoleManager.CreateAsync(identityExpressRole2);
        IdentityResult createResult = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (!createResult.Succeeded)
          return createResult;
        IdentityResult identityResult2 = await uow.Commit();
        return identityResult2;
      }
    }

    public async Task<IdentityResult> Delete(Role role)
    {
      if (role == null)
        throw new ArgumentNullException(nameof (role));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressRole identityExpressRole = await uow.RoleManager.FindByIdAsync(role.Id);
        IdentityExpressRole roleToDelete = identityExpressRole;
        identityExpressRole = (IdentityExpressRole) null;
        if (roleToDelete != null)
        {
          if (roleToDelete.Reserved)
            return IdentityResult.Failed(new IdentityError()
            {
              Description = "Cannot delete reserved role"
            });
          IdentityResult identityResult1 = await uow.RoleManager.DeleteAsync(roleToDelete);
          IdentityResult deleteResult = identityResult1;
          identityResult1 = (IdentityResult) null;
          if (!deleteResult.Succeeded)
            return deleteResult;
          IdentityResult identityResult2 = await uow.Commit();
          return identityResult2;
        }
        roleToDelete = (IdentityExpressRole) null;
      }
      return IdentityResult.Success;
    }

    public async Task<IList<Role>> Get(string name = null, IList<RoleOrderBy> ordering = null, IList<RoleFields> fields = null)
    {
      List<IdentityExpressRole> roles;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IQueryable<IdentityExpressRole> queryableRoles = uow.RoleManager.Roles;
        if (fields == null || fields.Contains(RoleFields.Users))
          queryableRoles = (IQueryable<IdentityExpressRole>) queryableRoles.Include<IdentityExpressRole, ICollection<IdentityExpressUserRole>>((Expression<Func<IdentityExpressRole, ICollection<IdentityExpressUserRole>>>) (x => x.Users));
        if (!string.IsNullOrWhiteSpace(name))
          queryableRoles = queryableRoles.Where<IdentityExpressRole>((Expression<Func<IdentityExpressRole, bool>>) (x => x.NormalizedName.Contains(this.normalizer.Normalize(name))));
        IList<RoleOrderBy> source = ordering;
        if (source != null && source.Any<RoleOrderBy>())
        {
          if (!ordering.Validate())
            throw new ValidationException("Invalid Ordering Received");
          IOrderedQueryable<IdentityExpressRole> orderedUsers = queryableRoles.ApplyOrdering(ordering.First<RoleOrderBy>());
          foreach (RoleOrderBy orderBy in ordering.Skip<RoleOrderBy>(1))
            orderedUsers = QueriableExtensions.ApplyOrdering(orderedUsers, orderBy);
          queryableRoles = (IQueryable<IdentityExpressRole>) orderedUsers;
          orderedUsers = (IOrderedQueryable<IdentityExpressRole>) null;
        }
        List<IdentityExpressRole> identityExpressRoleList = await queryableRoles.ToListAsync<IdentityExpressRole>(new CancellationToken());
        roles = identityExpressRoleList;
        identityExpressRoleList = (List<IdentityExpressRole>) null;
        queryableRoles = (IQueryable<IdentityExpressRole>) null;
      }
      IList<RoleFields> source1 = fields;
      return source1 == null || !source1.Any<RoleFields>() ? (IList<Role>) roles.Select<IdentityExpressRole, Role>((Func<IdentityExpressRole, Role>) (x => x.ToService())).ToList<Role>() : (IList<Role>) roles.Select<IdentityExpressRole, Role>((Func<IdentityExpressRole, Role>) (x =>
      {
        Role service = x.ToService();
        return new Role()
        {
          Id = fields.Contains(RoleFields.Id) ? service.Id : (string) null,
          Name = fields.Contains(RoleFields.Name) ? service.Name : (string) null,
          Description = fields.Contains(RoleFields.Description) ? service.Description : (string) null
        };
      })).ToList<Role>();
    }

    public async Task<Role> GetById(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (id));
      Role service;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressRole identityExpressRole = await uow.RoleManager.FindByIdAsync(id);
        IdentityExpressRole foundRole = identityExpressRole;
        identityExpressRole = (IdentityExpressRole) null;
        service = foundRole.ToService();
      }
      return service;
    }

    public async Task<IdentityResult> Update(Role role)
    {
      if (role == null)
        throw new ArgumentNullException(nameof (role));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressRole identityExpressRole1 = await uow.RoleManager.FindByIdAsync(role.Id);
        IdentityExpressRole existingRole = identityExpressRole1;
        identityExpressRole1 = (IdentityExpressRole) null;
        if (existingRole == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Role '" + role.Id + "' does not exist."
          });
        if (existingRole.Reserved)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Cannot update reserved role"
          });
        existingRole.Description = role.Description;
        if (existingRole.Name != role.Name)
        {
          IdentityExpressRole identityExpressRole2 = await uow.RoleManager.FindByNameAsync(role.Name);
          if (identityExpressRole2 != null)
          {
            identityExpressRole2 = (IdentityExpressRole) null;
            return IdentityResult.Failed(this.describer.DuplicateRoleName(role.Name));
          }
          existingRole.Name = role.Name;
        }
        IdentityResult identityResult1 = await uow.RoleManager.UpdateAsync(existingRole);
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

    public async Task<IdentityResult> AddUsers(Role role, IList<User> users)
    {
      if (role == null)
        throw new ArgumentNullException(nameof (role));
      if (users == null)
        throw new ArgumentNullException(nameof (users));
      IdentityResult identityResult1 = await this.ValidateUsers(users);
      IdentityResult validateUsers = identityResult1;
      identityResult1 = (IdentityResult) null;
      if (!validateUsers.Succeeded)
        return validateUsers;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressRole identityExpressRole = await uow.RoleManager.FindByIdAsync(role.Id);
        IdentityExpressRole existingRole = identityExpressRole;
        identityExpressRole = (IdentityExpressRole) null;
        if (existingRole == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Role with Id '" + role.Id + "' does not exist."
          });
        foreach (User user in users.Where<User>((Func<User, bool>) (x => existingRole.Users.All<IdentityExpressUserRole>((Func<IdentityExpressUserRole, bool>) (y => x.Subject != y.UserId)))).ToList<User>())
        {
          User newUser = user;
          ICollection<IdentityExpressUserRole> users1 = existingRole.Users;
          IdentityExpressUserRole identityExpressUserRole = new IdentityExpressUserRole();
          identityExpressUserRole.RoleId = existingRole.Id;
          identityExpressUserRole.UserId = newUser.Subject;
          users1.Add(identityExpressUserRole);
          newUser = (User) null;
        }
        IdentityResult identityResult2 = await uow.RoleManager.UpdateAsync(existingRole);
        IdentityResult result = identityResult2;
        identityResult2 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult3 = await uow.Commit();
          result = identityResult3;
          identityResult3 = (IdentityResult) null;
        }
        return result;
      }
    }

    public async Task<IdentityResult> RemoveUsers(Role role, IList<User> users)
    {
      if (role == null)
        throw new ArgumentNullException(nameof (role));
      if (users == null)
        throw new ArgumentNullException(nameof (users));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressRole identityExpressRole = await uow.RoleManager.FindByIdAsync(role.Id);
        IdentityExpressRole existingRole = identityExpressRole;
        identityExpressRole = (IdentityExpressRole) null;
        if (existingRole == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Role with Id '" + role.Id + "' does not exist."
          });
        foreach (IdentityExpressUserRole identityExpressUserRole in existingRole.Users.Where<IdentityExpressUserRole>((Func<IdentityExpressUserRole, bool>) (x => users.Any<User>((Func<User, bool>) (y => x.UserId == y.Subject)))).ToList<IdentityExpressUserRole>())
        {
          IdentityExpressUserRole removedUser = identityExpressUserRole;
          existingRole.Users.Remove(removedUser);
          removedUser = (IdentityExpressUserRole) null;
        }
        IdentityResult identityResult1 = await uow.RoleManager.UpdateAsync(existingRole);
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

    private async Task<IdentityResult> ValidateUsers(IList<User> users)
    {
      
      
      RoleService.\u003C\u003Ec__DisplayClass11_0 cDisplayClass110 = new RoleService.\u003C\u003Ec__DisplayClass11_0();
      
      cDisplayClass110.users = users;
      
      cDisplayClass110.\u003C\u003E4__this = this;
      
      
      if (cDisplayClass110.users == null || !cDisplayClass110.users.Any<User>())
        return IdentityResult.Success;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        
        
        RoleService.\u003C\u003Ec__DisplayClass11_1 cDisplayClass111 = new RoleService.\u003C\u003Ec__DisplayClass11_1();
        
        cDisplayClass111.CS\u0024\u003C\u003E8__locals1 = cDisplayClass110;
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        
        
        
        
        
        List<IdentityExpressUser> identityExpressUserList = await uow.UserManager.Users.Where<IdentityExpressUser>(Expression.Lambda<Func<IdentityExpressUser, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new Expression[2]
        {
          cDisplayClass111.CS\u0024\u003C\u003E8__locals1.users,
          (Expression) Expression.Lambda<Func<User, bool>>((Expression) Expression.Equal(this.normalizer.Normalize(y.Username), (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (IdentityUser<string>.get_NormalizedUserName), __typeref (IdentityUser<string>)))), new ParameterExpression[1]
          {
            parameterExpression2
          })
        }), new ParameterExpression[1]
        {
          parameterExpression1
        })).ToListAsync<IdentityExpressUser>(new CancellationToken());
        
        cDisplayClass111.foundUsers = identityExpressUserList;
        identityExpressUserList = (List<IdentityExpressUser>) null;
        
        
        
        if (cDisplayClass111.foundUsers.Count != cDisplayClass111.CS\u0024\u003C\u003E8__locals1.users.Select<User, string>((Func<User, string>) (x => x.Username)).Distinct<string>().Count<string>())
        {
          
          
          
          return IdentityResult.Failed(cDisplayClass111.CS\u0024\u003C\u003E8__locals1.users.Where<User>(new Func<User, bool>(cDisplayClass111.\u003CValidateUsers\u003Eb__1)).Select<User, IdentityError>((Func<User, IdentityError>) (x => new IdentityError()
          {
            Description = "Could not find User with name '" + x.Username + "'"
          })).ToArray<IdentityError>());
        }
        cDisplayClass111 = (RoleService.\u003C\u003Ec__DisplayClass11_1) null;
      }
      return IdentityResult.Success;
    }
  }
}
