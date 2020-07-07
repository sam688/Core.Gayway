using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.Models
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class TestUser
    {
        /// <summary>
        /// 客户端ID
        /// 对应Config.cs的GetClients方法中配置的ClientId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 客户端的密钥
        /// 对应Config.cs的GetClients方法中配置的ClientSecrets
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 客户端的范围
        /// 对应Config.cs的GetClients方法中配置的AllowedScopes
        /// </summary>
        public string Password { get; set; }
    }
}
