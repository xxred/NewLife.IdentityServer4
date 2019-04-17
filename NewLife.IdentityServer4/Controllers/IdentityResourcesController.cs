using Easy.Admin.Areas.Admin.Controllers;
using IdentityServer4.XCode.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewLife.IdentityServer4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityResourcesController : EntityController<IdentityResources>
    {
    }
}
