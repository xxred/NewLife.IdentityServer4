using System.ComponentModel;
using Easy.Admin.Areas.Admin.Controllers;
using Extensions.Identity.Stores.XCode;
using Microsoft.AspNetCore.Mvc;

namespace NewLife.IdentityServer4.Controllers
{
    /// <summary>
    /// 用户角色关系
    /// </summary>
    [DisplayName("用户角色关联")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : EntityController<IdentityUserRole>
    {
    }
}