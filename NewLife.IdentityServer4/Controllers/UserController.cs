//using System.ComponentModel;
//using Easy.Admin.Areas.Admin.Controllers;
//using Easy.Admin.Areas.Admin.Models;
//using Easy.Admin.Entities;
//using Microsoft.AspNetCore.Mvc;
//using System;

//namespace NewLife.IdentityServer4.Controllers
//{
//    /*
//     * 用户逻辑这块大部分还是自己实现吧，比如下面的默认密码等，或者添加其他功能
//     */

//    /// <summary>
//    /// 用户
//    /// </summary>
//    [Route("api/[controller]")]
//    [ApiController]
//    [DisplayName("用户")]
//    public class UserController : EntityController<ApplicationUser>
//    {
//        public override ApiResult<string> Post(ApplicationUser value)
//        {
//            // 新增账号默认密码123456
//            value.Password = "123456".MD5();
//            return base.Post(value);
//        }
//    }
//}
