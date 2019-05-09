using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Easy.Admin.Areas.Admin.Controllers;
using NewLife.IdentityServer4.Attributes;

namespace NewLife.IdentityServer4.Controllers
{
    [DisplayName("路由")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var ctrls = typeof(RoutesController).Assembly.GetTypes()
            .Where(w => typeof(BaseController).IsAssignableFrom(w)
            && w.GetCustomAttributes(typeof(NotMenuAttribute), false).Length < 1)
            .ToList();

            var list = new List<object>();

            var prefix = "/";

            foreach (var ctrl in ctrls)
            {
                var name = ctrl.Name.TrimEnd("Controller");
                var title = ctrl.GetDisplayName() ?? name;
                var route = new
                {
                    path = prefix + name,
                    component = "views/layout/Layout",
                    children = new[]
                    {
                        new
                        {
                            path = "index",
                            //template = $"<table-base table-name=\"{name}\" />",
                            name= name,
                            meta=new
                            {
                                title= title,
                                icon="international"
                            }
                        }
                    }
                };
                list.Add(route);
            }

            return new JsonResult(list);
        }
    }
}