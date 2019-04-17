using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewLife.IdentityServer4.ViewModels
{
    public class UserInfo
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string DisplayName { get; set; }
        public string[] Roles { get; set; }
    }
}
