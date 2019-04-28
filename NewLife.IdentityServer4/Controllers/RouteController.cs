using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Easy.Admin.Areas.Admin.Controllers;
namespace NewLife.IdentityServer4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var ctrls = typeof(RoutesController).Assembly.GetTypes()
            .Where(w => typeof(BaseController).IsAssignableFrom(w.BaseType))
            .ToList();

            var list = new List<object>();

            foreach (var ctrl in ctrls)
            {
                var name = ctrl.Name.TrimEnd("Controller");
                var route = new
                {
                    path = name,
                    component = "./src/views/layout/Layout",
                    children = new[]
                    {
                        new
                        {
                            path = "index",
                            template = $"<table-base table-name=\"{name}\" />",
                            name= name,
                            meta=new
                            {
                                title=ctrl.GetDescription()?? name,
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