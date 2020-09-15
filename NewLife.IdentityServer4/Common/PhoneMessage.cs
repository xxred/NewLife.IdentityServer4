using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Easy.Admin.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NewLife.Log;

namespace Cheerble.WebApi.Common
{
    /// <summary>
    /// 发送手机信息模块
    /// </summary>
    public class PhoneMessage : IPhoneMessage
    {
        private readonly PhoneMessageOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public PhoneMessage(IOptions<PhoneMessageOptions> config)
        {
            var options = config;
            if (options == null)
                throw new ArgumentNullException(nameof(config));
            this._options = options.Value;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="key">接收短信的手机号码。
        /// 格式：
        /// 国内短信：11位手机号码，例如15951955195。（这里加上86，为国内号码，实际发送会去掉，发国外保留区号）
        /// 国际/港澳台消息：国际区号+号码，例如85200000000。
        /// 支持对多个手机号码发送短信，手机号码之间以英文逗号（,）分隔。上限为1000个手机号码。批量调用相对于单条调用及时性稍有延迟。
        /// 验证码类型短信，建议使用单独发送的方式。
        /// </param>
        /// <param name="message">内容</param>
        /// <returns></returns>
        public void Send(string key, string message)
        {
            IClientProfile profile = DefaultProfile.GetProfile(_options.RegionId, _options.AccessKeyId, _options.Secret);
            var client = new DefaultAcsClient(profile);
            var request = new CommonRequest
            {
                Method = MethodType.POST,
                Domain = "dysmsapi.aliyuncs.com",
                Version = "2017-05-25",
                Action = "SendSms"
            };
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", key);

            string signName;
            string templateCode;
            if (key.StartsWith("86"))
            {
                signName = _options.SignName;
                templateCode = _options.TemplateCode;
            }
            else
            {
                signName = _options.InternationalSignName;
                templateCode = _options.InternationalTemplateCode;
            }

            request.AddQueryParameters("SignName", signName);
            request.AddQueryParameters("TemplateCode", templateCode);
            request.AddQueryParameters("TemplateParam", $"{{\"code\":\"{message}\"}}");
            try
            {
                var response = client.GetCommonResponse(request);
                XTrace.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
            }
            catch (ServerException e)
            {
                XTrace.WriteException(e);
            }
            catch (ClientException e)
            {
                XTrace.WriteException(e);
            }
        }
    }

    /// <summary>
    /// 手机信息配置
    /// </summary>
    public class PhoneMessageOptions
    {
        /// <summary>
        /// 区域
        /// </summary>
        public string RegionId { get; set; }

        /// <summary>
        /// AccessKeyId
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// Secret
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 短信签名名称(公司名称)
        /// </summary>
        public string SignName { get; set; }

        /// <summary>
        /// 国际短信签名名称(公司名称)
        /// </summary>
        public string InternationalSignName { get; set; }

        /// <summary>
        /// 短信模板ID。请在控制台模板管理页面模板CODE一列查看。
        /// 必须是已添加、并通过审核的短信签名；且发送国际/港澳台消息时，请使用国际/港澳台短信模版。
        /// </summary>
        public string TemplateCode { get; set; }

        /// <summary>
        /// 国际短信模板ID
        /// </summary>
        public string InternationalTemplateCode { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class PhoneMessageExtensions
    {
        /// <summary>
        /// 添加默认手机信息模块
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddPhoneMessage(
            this IServiceCollection services,
            Action<PhoneMessageOptions> configAction)
        {
            services.Configure(configAction);
            services.TryAddSingleton<IPhoneMessage, PhoneMessage>();
            return services;
        }
    }
}
