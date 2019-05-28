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
    [DisplayName("客户端属性")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientPropertiesController : EntityController<ClientProperties>
    {
    }
}
