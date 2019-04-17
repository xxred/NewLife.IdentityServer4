using Easy.Admin.Areas.Admin.Controllers;
using IdentityServer4.XCode.Entities;
using Microsoft.AspNetCore.Mvc;

namespace NewLife.IdentityServer4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimTypesController : EntityController<ClientClaims>
    {
    }
}
