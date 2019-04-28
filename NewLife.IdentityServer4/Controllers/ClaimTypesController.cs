using System.ComponentModel;
using Easy.Admin.Areas.Admin.Controllers;
using IdentityServer4.XCode.Entities;
using Microsoft.AspNetCore.Mvc;

namespace NewLife.IdentityServer4.Controllers
{
    [DisplayName("客户端声明种类")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimTypesController : EntityController<ClientClaims>
    {
    }
}
