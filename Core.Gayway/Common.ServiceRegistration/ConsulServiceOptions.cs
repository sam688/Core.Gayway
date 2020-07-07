using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ServiceRegistration
{
    class ConsulServiceOptions
    {
        /// <summary>
        /// 服务注册地址（Consul的地址）
        /// </summary>
        public string ConsulAddress { get; set; }

        /// <summary>
        /// 服务ID
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }

        public string Meta { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string HealthCheck { get; set; }

        /// <summary>
        /// 站点地址
        /// </summary>
        public string ServiceAddress { get; set; }
    }
}
