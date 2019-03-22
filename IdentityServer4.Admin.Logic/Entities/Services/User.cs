using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4.Admin.Logic.Entities.Services
{
    public class User
    {
        public User()
        {
        }

        public User(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            Username = username;
        }

        public string Subject { get; set; } = Guid.NewGuid().ToString();

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsBlocked { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool IsDeleted { get; set; }

        public IList<Role> Roles { get; set; } = new List<Role>();

        public IList<Claim> Claims { get; set; } = new List<Claim>();

        public IList<UserLoginInfo> Logins { get; } = new List<UserLoginInfo>();
    }
}
