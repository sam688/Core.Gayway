using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.Models
{
    /// <summary>
    /// 请求获取Token的客户端模型
    /// </summary>
    public class ClientModel
    {
        /// <summary>
        /// 客户端ID
        /// 对应Config.cs的GetClients方法中配置的ClientId
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端的密钥
        /// 对应Config.cs的GetClients方法中配置的ClientSecrets
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 客户端的范围
        /// 对应Config.cs的GetClients方法中配置的AllowedScopes
        /// </summary>
        public string Scope { get; set; }
    }
}
