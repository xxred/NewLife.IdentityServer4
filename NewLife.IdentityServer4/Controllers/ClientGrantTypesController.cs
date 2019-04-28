using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Easy.Admin.Areas.Admin.Controllers;
using IdentityServer4.Models;
using IdentityServer4.XCode.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace NewLife.IdentityServer4.Controllers
{
    [DisplayName(" ⁄»®¿‡–Õ")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientGrantTypesController : EntityController<ClientGrantTypes>
    {
    }
}
