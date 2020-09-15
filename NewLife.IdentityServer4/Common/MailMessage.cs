using System;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dm.Model.V20151123;
using Easy.Admin.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NewLife.Log;

namespace NewLife.IdentityServer4.Common
{
    /// <summary>
    /// 发送邮箱信息模块
    /// </summary>
    public class MailMessage : IMailMessage
    {
        private readonly MailMessageOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public MailMessage(IOptions<MailMessageOptions> config)
        {
            var options = config;
            if (options == null)
                throw new ArgumentNullException(nameof(config));
            this._options = options.Value;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="key">接收信息的邮箱地址，多个 email 地址可以用逗号分隔，最多100个地址。
        /// </param>
        /// <param name="message">内容</param>
        /// <returns></returns>
        public void Send(string key, string message)
        {
            IClientProfile profile = DefaultProfile.GetProfile(_options.RegionId, _options.AccessKeyId, _options.Secret);
            var client = new DefaultAcsClient(profile);
            var request = new SingleSendMailRequest
            {
                ReplyToAddress = false,
                AccountName = _options.AccountName,
                AddressType = 1,
                ToAddress = key,
                Subject = _options.Subject,
                FromAlias = _options.FromAlias,
                TagName = "验证码",
                TextBody = _options.TextBody.Replace("${code}", message),
            };

            try
            {
                var response = client.GetAcsResponse(request);
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
    /// 邮箱信息配置
    /// </summary>
    public class MailMessageOptions
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
        /// 管理控制台中配置的发信地址。
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 邮件主题，建议填写。默认：帐户安全代码
        /// </summary>
        public string Subject { get; set; } = "帐户安全代码";

        /// <summary>
        /// 邮件text正文。可选。默认为：发信人昵称，长度小于15个字符。
        /// </summary>
        public string FromAlias { get; set; }

        /// <summary>
        /// 您的验证码为：${code}，该验证码 5 分钟内有效，请勿泄漏于他人。
        /// </summary>
        public string TextBody { get; set; } = "您的验证码为：${code}，该验证码 5 分钟内有效，请勿泄漏于他人。";
    }

    /// <summary>
    /// 
    /// </summary>
    public static class MailMessageExtensions
    {
        /// <summary>
        /// 添加默认手机信息模块
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddMailMessage(
            this IServiceCollection services,
            Action<MailMessageOptions> configAction)
        {
            services.Configure(configAction);
            services.TryAddSingleton<IMailMessage, MailMessage>();
            return services;
        }
    }
}
