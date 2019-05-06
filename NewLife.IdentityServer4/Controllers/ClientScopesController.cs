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
    [DisplayName("客户端作用域")]
    [Description("默认情况下，客户端无权访问任何资源-通过添加相应的作用域名称指定允许的资源")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientScopesController : EntityController<ClientScopes>
    {
    }
}
