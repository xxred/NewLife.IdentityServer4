using System.ComponentModel;
using Easy.Admin.Areas.Admin.Controllers;
using IdentityServer4.XCode.Entities;
using Microsoft.AspNetCore.Mvc;

namespace NewLife.IdentityServer4.Controllers
{
    [DisplayName("Api声明")]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiClaimsController : EntityController<ApiClaims>
    {
    }
}
