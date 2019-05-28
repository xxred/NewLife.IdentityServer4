using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Easy.Admin.Areas.Admin.Controllers;
using IdentityServer4.XCode.Entities;
using Microsoft.AspNetCore.Mvc;

namespace NewLife.IdentityServer4.Controllers
{
    [DisplayName("客户端声明")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientClaimsController : EntityController<ClientClaims>
    {
    }
}
