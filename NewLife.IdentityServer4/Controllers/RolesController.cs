using Easy.Admin.Areas.Admin.Controllers;
using Extensions.Identity.Stores.XCode;
using Microsoft.AspNetCore.Mvc;

namespace NewLife.IdentityServer4.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : EntityController<IdentityRole>
    {

    }
}
