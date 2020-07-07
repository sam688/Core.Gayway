using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Common.ServiceRegistration
{
    public static class ConsulExtensions
    {
        public static IConfigurationRoot consulconfig = new ConfigurationBuilder().AddJsonFile("consulconfig.json").Build();
        public static void AddConsul(this IServiceCollection service)
        {
            // 读取服务配置文件
            service.Configure<ConsulServiceOptions>(consulconfig);
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration configuration = null)
        {

            // 获取主机生命周期管理接口
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            // 获取服务配置项
            var serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<ConsulServiceOptions>>().Value;

            // 服务ID必须保证唯一
            serviceOptions.ServiceId = Guid.NewGuid().ToString();


            var consulClient = new ConsulClient(config =>
            {
                //服务注册的地址，集群中任意一个地址
                config.Address = new Uri(serviceOptions.ConsulAddress);
            });

            // 获取当前服务地址和端口，配置方式
            var uri = new Uri(serviceOptions.ServiceAddress);
            
            if (string.IsNullOrEmpty(serviceOptions.Tags))
                serviceOptions.Tags = serviceOptions.Weight.ToString();
            else
                serviceOptions.Tags = serviceOptions.Tags + "," + serviceOptions.Weight.ToString();

            Dictionary<string, string> dictMeta = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(serviceOptions.Meta))
                dictMeta = null;
            else
            {
                string[] keyvals = serviceOptions.Meta.Split(',');
                if (keyvals != null)
                {
                    for (int i = 0; i < keyvals.Length; i++)
                    {
                        string[] keyval = keyvals[i].Split(':');
                        if (keyval != null && keyval.Length == 2)
                        {
                            string key = keyval[0].ToString();
                            string val = keyval[1].ToString();

                            dictMeta.Add(key, val);
                        }
                    }
                }
            }


            // 节点服务注册对象
            var registration = new AgentServiceRegistration()
            {
                ID = serviceOptions.ServiceId,
                Name = serviceOptions.ServiceName,// 服务名
                Address = uri.Host,
                Port = uri.Port, // 服务端口
                Check = new AgentServiceCheck
                {
                    // 健康检查地址
                    HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}{serviceOptions.HealthCheck}",
                    // 健康检查时间间隔
                    Interval = TimeSpan.FromSeconds(5),
                    // 注册超时
                    Timeout = TimeSpan.FromSeconds(10),
                    // 服务停止多久后注销服务
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5)
                },
                Tags = serviceOptions.Tags.Split(","),
                Meta = dictMeta
            };

            try
            {
                // 注册服务
                consulClient.Agent.ServiceRegister(registration).Wait();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message+ ";修复问题的建议：本异常是注册Consul服务时发生异常。可能是您未启动Consul服务器, 或配置参数未正确。");
            }

            // 应用程序终止时，注销服务
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceOptions.ServiceId).Wait();
            });

            return app;
        }
    }
}
