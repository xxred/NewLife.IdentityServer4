using System.ComponentModel;
using Easy.Admin.Areas.Admin.Controllers;
using Extensions.Identity.Stores.XCode;
using Microsoft.AspNetCore.Mvc;

namespace NewLife.IdentityServer4.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    [DisplayName("角色")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : EntityController<IdentityRole>
    {

    }
}
